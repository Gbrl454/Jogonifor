using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum Scenes { MainMenu = 0, Bedroom = 1 }

public class MenuPrincipal : MonoBehaviour
{
    //[SerializeField] GameObject cameraGO;
    [SerializeField] GameObject loadingScreen;
    [SerializeField] CanvasGroup canvasGrounp;
    [Space]
    [SerializeField] Button startBtn;
    [SerializeField] Button resetBtn;

    AsyncOperation loadingOperation;


    private void Start()
    {
        startBtn.onClick.AddListener(
            () =>
            {
                loadingScreen.SetActive(true);

                int sceneID = (int)Scenes.Bedroom;
                loadingOperation = SceneManager.LoadSceneAsync(sceneID, LoadSceneMode.Additive);
                
                Loading(SceneManager.GetSceneAt(sceneID));
            });
        resetBtn.onClick.AddListener(
            () => PlayerPrefs.DeleteAll());

        loadingScreen.SetActive(false);
        SetMainCanvasState(true);
    }
    

    private async void Loading(Scene loading)
    {
        while (!loadingOperation.isDone)
            await Task.Yield();

        //DestroyImmediate(cameraGO);
        loadingScreen.SetActive(false);

        SceneManager.SetActiveScene(loading);
        SetMainCanvasState(false);
    }
    private void SetMainCanvasState(bool state)
    {
        canvasGrounp.interactable = state;
        canvasGrounp.blocksRaycasts = state;
        canvasGrounp.alpha = state ? 1 : 0;
    }
}
