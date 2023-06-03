using UnityEngine;
using UnityEngine.Events;

namespace Game.Interactions
{
    [ExecuteAlways]
    public class Interactable : MonoBehaviour
    {
        [SerializeField] Transform target;
        [SerializeField] string label;
        [SerializeField] UnityEvent OnEndReached;
        [Space]
        [SerializeField] Vector3 start;
        [SerializeField] Vector3 end;
        [Space]
        [Tooltip("Direction horizontal only")]
        [SerializeField] Vector3 direction;
        [SerializeField] Vector3 anchor;
        
        [Header("Dragging")]
        [SerializeField] float speed = 0.5f;
        [SerializeField] float contactOffset = 0.5f;
        [SerializeField] bool invertPosition;
        [SerializeField] AudioSource audioSource;

        [Space]
        [SerializeField, Range(0, 1)] float position;
        [SerializeField] Vector3 offsetTarget;

        [Header("Debugging")]
        [SerializeField] bool updateEditor;
        [SerializeField] Interactor controller;

        bool calledEvent;

        [ContextMenu("Reset Position")]
        private void ResetPositions()
        {
            start = transform.position;

            offsetTarget = target.position - transform.position;
            anchor = target.position - transform.position;

            direction = (Flatten(target.position) - Flatten(transform.position)).normalized;

            end = start - (2 * direction);
        }
        private Vector3 Flatten(Vector3 source) => new(source.x, 0, source.z);


        private void Start()
        {
            if (!Application.isPlaying)
                return;

            direction = (start - end).normalized;
            RestorePosition();
        }
        private void Update()
        {
#if UNITY_EDITOR
            if (Application.isPlaying)
                UpdatePositions();
            else if (updateEditor)
                CalculateSetPosition(out _);
#else
            UpdatePositions();
#endif
        }


        public void StartControl(Interactor controller)
        {
            this.controller = controller;
            controller.Direction = invertPosition ? -direction : direction;
        }
        public void StopControl()
        {
            this.controller = null;
            audioSource.Stop();
            SavePosition();
        }
        public void SetMovement(float movement)
        {
            if (invertPosition)
                position -= Mathf.Clamp(Time.deltaTime * -movement * speed, -1, 1);
            else
                position += Mathf.Clamp(Time.deltaTime * -movement * speed, -1, 1);

            position = Mathf.Clamp(position, 0, 1);
            bool shoulPlay = movement != 0;
            if (audioSource.isPlaying != shoulPlay)
            {
                if (shoulPlay)
                    audioSource.Play();
                else
                    audioSource.Stop();
            }
        }

        private void UpdatePositions()
        {
            if (!controller)
                return;

            CalculateSetPosition(out Vector3 contactPosition);
            controller.Position = contactPosition - direction * (invertPosition ? -contactOffset : contactOffset);
        }
        private void CalculateSetPosition(out Vector3 contactPosition)
        {
            contactPosition = Vector3.Lerp(start, end, position);
            if (!calledEvent && position >= 1)
            {
                OnEndReached.Invoke();
                calledEvent = true;
            }

            target.position = contactPosition + offsetTarget;
            transform.position = contactPosition + direction * (invertPosition ? -contactOffset : contactOffset); //+ anchor;
        }

        private void SavePosition() => PlayerPrefs.SetFloat($"{label}_pos", position);
        private void RestorePosition()
        {
            string key = $"{label}_pos";
            if (PlayerPrefs.HasKey(key))
            {
                position = PlayerPrefs.GetFloat(key);
                CalculateSetPosition(out _);
            }
        }


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(start, end);

            Gizmos.DrawSphere(Vector3.Lerp(start, end, position), 0.1f);
        }
    }
}