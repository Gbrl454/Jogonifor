using UnityEngine;
using UnityEngine.UI;
//using Game.Obras;
//using Game.Player;
//using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    //[System.Serializable]
    //public struct QuadroInfo
    //{
    //    public int id;
    //    public Image ui;

    //    public string Name => $"pieceID_{id}";
    //    //public Quadro quadro;
    //}

    //[Header("U.I.")]
    //[SerializeField] GameObject obrasDisplay;
    //[SerializeField] float switchTime = 0.3f;
    //[SerializeField] AnimationCurve switchCurve;
    //[SerializeField] Vector2 offPos =
    //    new(-794, 383), offSize = new(256, 256), onSize = new (1024, 1024);
    //[SerializeField] Button quadroNowBtn;
    //[SerializeField] Image quadroDisplayImage, visualizingBackdropPanel;
    //[SerializeField] TextMeshProUGUI quadroName;

    [Header("Game")]
    [SerializeField] Button getBackBtn;

    [Header("Scene Quadros")]
    [SerializeField] Transform piecesRoot;
    [SerializeField] Image[] pieces;

    //[SerializeField] Color dontHaveColor = Color.gray;
    //[SerializeField] Color haveColor = Color.white;
    //[SerializeField] QuadroInfo[] quadros;

    //private bool zooming, switchVisualization;
    //private float switchingVisLerp;
    //private RectTransform quadroRectT;


    private void Awake()
    {
        Instance = this;

        pieces = new Image[piecesRoot.childCount];
        for (int i = 0; i < piecesRoot.childCount; i++)
            pieces[i] = piecesRoot.GetChild(i).GetComponent<Image>();
    }
    private void Start()
    {
        //obrasDisplay.SetActive(false);
        RefreshAllQuadrosUI();

        // Usava quando detectava um quadro e precisava saber qual era. Agora apenas adiciona o id da peça (0 -> 8)
        //Player.Instance.OnDetectedQuadro +=
        //    (quadro) =>
        //    {
        //        obrasDisplay.gameObject.SetActive(true);
        //        quadroDisplayImage.sprite = quadro.Sprite;
        //        quadroName.text = quadro.name;
        //    };
        //Player.Instance.OnLeaveQuadro +=
        //    () =>
        //    {
        //        obrasDisplay.gameObject.SetActive(false);
        //    };

        //quadroRectT = quadroNowBtn.GetComponent<RectTransform>();
        //quadroNowBtn.onClick.AddListener(
        //    () =>
        //    {
        //        zooming = !zooming;
        //        switchVisualization = true;
        //    });
        getBackBtn.onClick.AddListener(
            () =>
            {
                SceneManager.LoadScene(0);
            });

        //visualizingBackdropPanel.gameObject.SetActive(false);
    }
    //private void Update()
    //{
    //    if (switchVisualization)
    //    {
    //        visualizingBackdropPanel.gameObject.SetActive(zooming);

    //        if (zooming)
    //        {
    //            if (switchingVisLerp < 1)
    //                switchingVisLerp += Time.deltaTime / switchTime;
    //            else
    //            {
    //                switchingVisLerp = 1;
    //                switchVisualization = false;
    //            }
    //        }
    //        else
    //        {
    //            if (switchingVisLerp > 0)
    //                switchingVisLerp -= Time.deltaTime / switchTime;
    //            else
    //            {
    //                switchingVisLerp = 0;
    //                switchVisualization = false;
    //            }
    //        }

    //        float lerp = switchCurve.Evaluate(switchingVisLerp);
    //        quadroRectT.anchoredPosition = Vector2.Lerp(offPos, Vector2.zero, lerp);

    //        Vector2 lerpedSize = Vector2.Lerp(offSize, onSize, lerp);
    //        quadroRectT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, lerpedSize.x);
    //        quadroRectT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, lerpedSize.y);
    //    }
    //}


    public static void RefreshAllQuadrosUI()
    {
        if (!Instance)
        {
            Debug.LogWarning("No instances of GameManager on scene");
            return;
        }

        var pieces = Instance.pieces;
        for (int i = 0; i < pieces.Length; i++)
            pieces[i].enabled = PlayerPrefs.HasKey(GetName(i));

        //foreach (var item in Instance.quadros)
        //    item.ui.color = item.quadro.IsTaken ? Instance.dontHaveColor : Instance.haveColor;
    }


    public static string GetName(int id) => $"piece_{id}";
    public static LayerMask ScalingLayer => LayerMask.NameToLayer("Escalavel");
}
