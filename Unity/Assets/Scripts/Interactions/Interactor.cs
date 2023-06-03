using UnityEngine;
using StarterAssets;

namespace Game.Interactions
{
    public class Interactor : MonoBehaviour
    {
        [SerializeField] StarterAssetsInputs inputs;
        [SerializeField] Interactable interactingWith;
        
        [Header("Animations")]
        [SerializeField] Animator anim;
        [SerializeField] string pullPushParam = "PullPush";
        [SerializeField] string moveParam = "Vert";

        [Header("Detection")]
        [SerializeField] Vector3 originOffset;
        [SerializeField] float radius = 1;
        [SerializeField] LayerMask layers;

        [Header("Direction")]
        [SerializeField] Transform cameraTarget;

        [Space]
        [SerializeField] bool gizmos;

        private bool track;

        public bool HasInteraction => interactingWith != null;
        public Vector3 Position { set => transform.position = value; }
        public Vector3 Direction
        {
            set
            {
                var angle = Mathf.Atan2(value.x, value.z);
                angle *= Mathf.Rad2Deg;

                transform.eulerAngles = new(0, angle, 0);
                cameraTarget.localEulerAngles = Vector3.zero;
            }
        }

        private Vector3 Origin => transform.TransformPoint(originOffset);


        public void Update()
        {
            if (track && interactingWith)
            {
                float movement = Mathf.Clamp(inputs.move.y, -1, 1);
                interactingWith.SetMovement(movement);
                anim.SetFloat(moveParam, movement);
            }
        }


        public bool CheckInteractables()
        {
            bool atLeastOne = false;
            Collider[] detected = Physics.OverlapSphere(Origin, radius, layers);
            if (detected != null && detected.Length > 0)
            {
                for (int i = 0; i < detected.Length; i++)
                {
                    Interactable inter = detected[i].GetComponent<Interactable>();
                    if (inter != null)
                    {
                        atLeastOne = true;
                        interactingWith = inter;
                        break;
                    }
                }
            }

            if (!atLeastOne)
                interactingWith = null;

            return HasInteraction;
        }
        public void StartInteraction()
        {
            interactingWith.StartControl(this);
            anim.SetBool(pullPushParam, true);
            track = true;
        }
        public void StopInteraction()
        {
            interactingWith.StopControl();
            interactingWith = null;
            anim.SetBool(pullPushParam, false);
            track = false;
        }


        private void OnDrawGizmosSelected()
        {
            if (!gizmos) return;

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(Origin, radius);
        }
    }
}