using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

[ExecuteAlways]
public class Finisher : MonoBehaviour
{
    public interface IAnimatable
    {
        void Play();
        void Animate(float time);
    }

    [System.Serializable]
    public struct AnimatedVideo : IAnimatable
    {
        public VideoPlayer video;
        public VideoClip clip;
        public ushort track;
        [Space]
        public RawImage imageRef;
        public AnimationCurve volume;
        [Range(0, 1)] public float triggersAt;
        public float volumeMul;

        [SerializeField] private bool hasTrigger, triggered;

        public void Play()
        {
            hasTrigger = triggersAt > 0;
            if (hasTrigger)
            {
                imageRef.enabled = false;
                return;
            }
            else
                SetAndPlay();
        }
        public void Animate(float time)
        {
            if (hasTrigger)
            {
                var trigger = time > triggersAt;
                if (!triggered && trigger)
                {
                    imageRef.enabled = true;
                    SetAndPlay();
                    triggered = true;
                }
                if (triggered)
                    video.SetDirectAudioVolume(track, volume.Evaluate(time) * volumeMul);
            }
            else
                video.SetDirectAudioVolume(track, volume.Evaluate(time) * volumeMul);
        }
        
        private void SetAndPlay()
        {
            video.clip = clip;
            video.Play();
        }
    }
    [System.Serializable]
    public struct AnimatedCanvas : IAnimatable
    {
        public CanvasGroup canvas;
        public AnimationCurve alpha;

        public void Play() => canvas.alpha = alpha.Evaluate(0);
        public void Animate(float time) => canvas.alpha = alpha.Evaluate(time);
    }

    public static Finisher Instance;

    [SerializeField, Range(0, 1)] float time;
    [SerializeField] float animationTime = 15;
    [Space]
    [SerializeField] AudioSource mainAudio;
    [Space]
    [SerializeField] bool editorUpdate;
    [SerializeField] AnimatedCanvas endCanvas;
    [SerializeField] AnimatedCanvas videoCanvas;
    [SerializeField] AnimatedVideo parabains;
    [SerializeField] AnimatedVideo para_bens;

    private bool playing;


    private void Awake() => Instance = this;
    private void Update()
    {
        if (Application.isPlaying)
        {

            if (playing)
            {
                time += Time.deltaTime / animationTime;

                if (time > 1)
                    time = 1;
                AnimateAll();

                if (time == 1)
                {
                    PlayerPrefs.DeleteAll();
                    SceneManager.LoadScene(0);
                }
            }
#if UNITY_EDITOR
            else if (Input.GetKeyDown(KeyCode.Y))
                PlayWin();
#endif
        }
        else if (editorUpdate)
        {
            AnimateAll();
        }
    }


    private void PlayWin()
    {
        playing = true;
        mainAudio.Stop();

        parabains.Play();
        para_bens.Play();

        videoCanvas.Play();
        endCanvas.Play();
    }
    private void AnimateAll()
    {
        endCanvas.Animate(time);
        videoCanvas.Animate(time);

        parabains.Animate(time);
        para_bens.Animate(time);
    }

    public static void RefreshGameStats()
    {
        bool hasAll = true;
        for (int i = 0; i < 9; i++)
        {
            string key = GameManager.GetName(i);
            if (!PlayerPrefs.HasKey(key))
            {
                hasAll = false;
                break;
            }
        }

        if (hasAll)
            Instance.PlayWin();
    }
}
