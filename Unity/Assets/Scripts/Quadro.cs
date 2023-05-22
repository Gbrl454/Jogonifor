using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quadro : MonoBehaviour
{
    public GameObject guiPegarQuadro;
    private GameManager gameManager;
    private GameObject quadroObra;
    private Material materialQuadro;
    public int idObra = 0;

    private void Start()
    {
        quadroObra = transform.Find("Obra").gameObject;
        gameManager = FindObjectOfType<GameManager>();

        materialQuadro = new Material(Shader.Find("Standard"));
        materialQuadro.mainTexture = gameManager.getImage(idObra).texture;
        MeshRenderer meshRenderer = quadroObra.GetComponent<MeshRenderer>();
        meshRenderer.material = materialQuadro;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            guiPegarQuadro.SetActive(true);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && Input.GetKey(KeyCode.E))
        {
            guiPegarQuadro.SetActive(false);
            guiInfosQuadro(idObra);
            gameManager.setIsGamePaused(true);
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
            guiPegarQuadro.SetActive(false);
    }

    public GameObject guiInfos;

    public void guiInfosQuadro(int idObra)
    {
        guiInfos.SetActive(true);
        GUI_Quadro menuUI = guiInfos.GetComponent<GUI_Quadro>();
        menuUI.SetMenuIdObra(idObra);
    }
}
