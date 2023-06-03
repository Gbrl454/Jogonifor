using UnityEngine;

namespace Game.Interactions
{
    [System.Serializable] 
    public struct AnimatableVec3
    {
        public Vector3 inicial;
        public Vector3 final;

        public Vector3 Evaluate(float time)
        {
            time = Mathf.Clamp01(time);
            return Vector3.Lerp(inicial, final, time);
        }
    }

    [ExecuteAlways]
    public class TrashCanAnimation : MonoBehaviour
    {
        [SerializeField] bool editorEdit;
        
        [Header("Animation")]
        [SerializeField, Range(0, 1)] float currentTime;
        [SerializeField] float totalTime = 3;
        [SerializeField] AnimationCurve curve;
        [SerializeField] AnimatableVec3 localEulers;

        [Header("Ending")]
        [SerializeField] GameObject toActivate;
        [SerializeField] AudioSource audioSource;

        private readonly string saveParameter = "chair_drop";
        private bool playing, played;


        private void Start()
        {
            if (PlayerPrefs.HasKey(saveParameter))
            {
                played = true;
                currentTime = 1;
            }
            else
            {
                currentTime = 0;
                toActivate.SetActive(false);
            }
            Animate();
        }
        private void Update()
        {
            if (Application.isPlaying)
            {
                if (playing)
                {
                    currentTime += Time.deltaTime / totalTime;
                    if (currentTime > 1)
                    {
                        currentTime = 1;
                        audioSource.Play();
                        playing = false;

                        toActivate.SetActive(true);
                        played = true;
                        PlayerPrefs.SetInt(saveParameter, 1);
                    }

                    Animate();
                }
            }
            else if (editorEdit)
            {
                Animate();
            }
        }


        private void Animate() =>
            transform.localEulerAngles =
            localEulers.Evaluate(curve.Evaluate(currentTime));

        public void Play()
        {
            if (played)
                return;

            currentTime = 0;
            playing = true;
            played = false;
        }
    }
}