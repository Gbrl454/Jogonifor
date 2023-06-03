using UnityEngine;
using Game.Obras;
using Game.Interactions;
using UnityEngine.UI;
using StarterAssets;

namespace Game.Player
{
    public class Player : MonoBehaviour
    {
        [System.Serializable]
        public struct CheckSphere
        {
            public LayerMask layers;
            public Vector3 offset, disabledOffset;
            public float radius, topCheckRadius;

            public bool Check(Transform t) =>
                Physics.CheckSphere(t.TransformPoint(offset), radius, layers, QueryTriggerInteraction.Collide);
            public bool CheckTop(Transform t) =>
                Physics.CheckSphere(t.TransformPoint(disabledOffset), topCheckRadius, layers, QueryTriggerInteraction.Collide);

            public bool AsRayCast(Transform t, out RaycastHit hit)
            {
                Ray r = new(t.TransformPoint(offset), t.forward);

                return Physics.Raycast(r, out hit, radius * 2f, layers, QueryTriggerInteraction.Collide);
            }
            public bool PositionOnTop(Transform t, ref Vector3 pos)
            {
                Ray r = new(t.TransformPoint(disabledOffset), Vector3.down);
                if (Physics.Raycast(r, out RaycastHit hit, 10, layers, QueryTriggerInteraction.Collide))
                {
                    pos = hit.point;
                    return true;
                }
                else return false;
            }

            public void DrawGizmos(Transform t)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(t.TransformPoint(offset), radius);

                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(t.TransformPoint(disabledOffset), topCheckRadius);
            }
        }

        public static Player Instance;

        [Header("Setup")]
        [SerializeField] string takeParameter = "Take";
        [SerializeField] Animator anim;
        [SerializeField] Button detectBtn;
        [Space]
        [SerializeField] string climbParam = "Climb";
        [SerializeField] string reachTopParam = "ToTop";

        [Header("Detected")]
        [SerializeField] Quadro quadro;
        [Tooltip("Distance added to the radius")]
        [SerializeField] float leaveDist = 2f;

        [Header("Detecting")]
        [SerializeField] float radius = 2;
        [SerializeField] Vector3 offset = new (0, 0.25f, 0.5f);
        [SerializeField] LayerMask layers;
        [SerializeField] AudioSource takeItemSoud;

        [Header("Climbing")]
        [SerializeField] bool climbing;
        [SerializeField] bool canClimb;
        [SerializeField] CheckSphere climbSphere;
        [Space]
        [SerializeField] Button climbBtn;
        [SerializeField] ClimbingController controller;
        [SerializeField] ThirdPersonController tController;
        [SerializeField] GameObject[] disableWhenClimb;

        [Header("Pull & Push")]
        [SerializeField] Interactor interact;

        [Header("Debugs")]
        [SerializeField] float distance;
        [SerializeField] float disableDistance;
        [SerializeField] bool gizmos;

        private bool take, climbTrack, interactTrack;
        private Vector3 positionOnTop;
        private Transform t;

        public System.Action<int> OnDetectedQuadro;
        public System.Action OnLeaveQuadro;
 
        private Vector3 Origin => t.TransformPoint(offset);


        private void Awake()
        {
            Instance = this;

            tController = GetComponent<ThirdPersonController>();
            controller = GetComponent<ClimbingController>();
            interact = GetComponent<Interactor>();

            climbBtn.onClick.AddListener(ToggleClimb);

            LoadPosition();
        }
        private void Start()
        {
            t = transform;
            disableDistance = radius + leaveDist;

            detectBtn.onClick.AddListener(Interact);
            detectBtn.gameObject.SetActive(false);

            ClimbSitChanged();
            SetClimbing(false);
        }
        private void Update()
        {
            if (canClimb && Input.GetKeyDown(KeyCode.E))
                ToggleClimb();

            detectBtn.gameObject.SetActive(quadro != null || interact.HasInteraction);
        }
        private void FixedUpdate()
        {
            CheckCanClimb();

            if (climbing)
                return;

            // Se estiver enfocando um interactable retorna
            bool hasInteractable = interact.CheckInteractables();
            if (interactTrack || (hasInteractable && tController.Grounded))
                return;

            if (quadro != null)
            {
                if (take)
                {
                    OnLeaveQuadro?.Invoke();

                    quadro.SetTaken();
                    quadro = null;
                    detectBtn.gameObject.SetActive(false);

                    GameManager.RefreshAllQuadrosUI();
                    take = false;
                }
                else
                {
                    distance = Vector3.Distance(Origin, quadro.transform.position);
                    if (distance > disableDistance)
                    {
                        quadro = null;
                        detectBtn.gameObject.SetActive(false);
                        OnLeaveQuadro?.Invoke();
                    }
                }
            }
            else
            {
                Collider[] detected = Physics.OverlapSphere(Origin, radius, layers);
                if (detected != null && detected.Length > 0)
                {
                    for (int i = 0; i < detected.Length; i++)
                    {
                        Quadro q = detected[i].GetComponent<Quadro>();
                        if (q)
                        {
                            quadro = q;
                            detectBtn.gameObject.SetActive(true);
                            OnDetectedQuadro?.Invoke(q.ID);//(q.Obra);
                            break;
                        }
                    }
                }
            }
        }
        private void OnDisable() => SavePosition();


        private void CheckCanClimb()
        {
            canClimb = climbSphere.Check(t);

            if (climbing && !climbSphere.CheckTop(t))
            {
                // Going to STOP climbing by reaching top
                anim.SetBool(reachTopParam, true);
                if (climbSphere.PositionOnTop(t, ref positionOnTop))
                    transform.position = positionOnTop;

                SetClimbing(false);
            }

            if (climbTrack != canClimb)
                ClimbSitChanged();
        }
        public void ToggleClimb()
        {
            if (!canClimb)
                return;
                
            // Going to STOP climbing by dropping
            if (climbing)
                anim.SetBool(climbParam, false);

            SetClimbing(!climbing);

            // Just STARTED climb
            if (climbing)
            {
                if (climbSphere.AsRayCast(t, out RaycastHit h))
                    t.forward = -h.normal;

                controller.turninCamera = true;
                anim.SetBool(climbParam, true);
            }
        }
        private void SetClimbing(bool active)
        {
            climbing = active;

            tController.enabled = !climbing;
            controller.enabled = climbing;

            for (int i = 0; i < disableWhenClimb.Length; i++)
                disableWhenClimb[i].SetActive(!climbing);
        }
        private void ClimbSitChanged()
        {
            climbBtn.gameObject.SetActive(canClimb);
            climbTrack = canClimb;
        }
        public void Interact()
        {
            if (quadro)
            {
                take = true;
                takeItemSoud.Play();
                anim.SetTrigger(takeParameter);
                GameManager.RefreshAllQuadrosUI();
            }

            // Se tiver algo com o que interagir no momento
            if (interact.HasInteraction)
            {
                if (interactTrack)
                {
                    // Stoping interaction
                    interact.StopInteraction();

                    tController.enabled = true;
                    controller.enabled = false;
                    interactTrack = false;
                }
                else
                {
                    // Starting interaction
                    interact.StartInteraction();

                    tController.enabled = false;
                    controller.enabled = false;
                    interactTrack = true;
                }
            }
        }

        private void ReachedTop()
        {
            anim.SetBool(climbParam, false);
            anim.SetBool(reachTopParam, false);
        }
        private void SavePosition()
        {
            Vector3 pos = transform.position;
            PlayerPrefs.SetFloat("pl_pos_x", pos.x);
            PlayerPrefs.SetFloat("pl_pos_y", pos.y);
            PlayerPrefs.SetFloat("pl_pos_z", pos.z);
        }
        private void LoadPosition()
        {
            bool hasPositions =
                PlayerPrefs.HasKey("pl_pos_x") &&
                PlayerPrefs.HasKey("pl_pos_y") &&
                PlayerPrefs.HasKey("pl_pos_z");

            if (hasPositions)
            {
                Vector3 pos = new(
                    PlayerPrefs.GetFloat("pl_pos_x"),
                    PlayerPrefs.GetFloat("pl_pos_y"),
                    PlayerPrefs.GetFloat("pl_pos_z"));

                transform.position = pos;
            }
        }


        private void OnDrawGizmosSelected()
        {
            if (!gizmos) return;

            if (t == null)
                t = transform;

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.TransformPoint(offset), radius);

            climbSphere.DrawGizmos(t);
        }
    }
}