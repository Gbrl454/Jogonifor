using UnityEngine;

namespace Game.Obras
{
    public class Quadro : MonoBehaviour
    {
        //public GameObject guiPegarQuadro;
        //private GameManager gameManager;
        //private GameObject quadroObra;
        //private Material materialQuadro;
        //public int idObra = 0;
        //[SerializeField] QuadroData data;

        [SerializeField] int pieceID;
        [SerializeField] Texture tex; 
        [SerializeField] MeshRenderer screenRenderer;
        [Space]
        [SerializeField] Transform makeParent;
        [SerializeField] Vector3 localPosition;

        //private string Name => $"pieceID_{pieceID}";

        public bool IsTaken => !PlayerPrefs.HasKey(GameManager.GetName(pieceID));
        public int ID => pieceID;


        private void Start()
        {
            screenRenderer.material.mainTexture = tex;// data.Texture;
            UpdateState();

            if (makeParent)
            {
                transform.position = makeParent.position + localPosition;
                transform.parent = makeParent;
            }
        }


        [ContextMenu("Save Local Bias")]
        private void SaveLocalPosition()
        {
            if (!makeParent)
                return;

            localPosition = transform.position - makeParent.position;
        }

        public void SetTaken()
        {
            PlayerPrefs.SetInt(GameManager.GetName(pieceID), pieceID);
            Finisher.RefreshGameStats();
            UpdateState();
        }

        private void UpdateState()
        {
            bool isTaken = IsTaken;
            
            var renders = GetComponentsInChildren<Renderer>();
            foreach (var item in renders)
                item.enabled = isTaken;

            var colliders = GetComponentsInChildren<Collider>();
            foreach (var item in colliders)
                item.enabled = isTaken;

            var rigBodies = GetComponentsInChildren<Rigidbody>();
            foreach (var item in rigBodies)
                item.useGravity = !isTaken;
        }
    }
}