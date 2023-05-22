using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_Quadro : MonoBehaviour
{
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
            fecharGUI();
    }

    public void SetMenuIdObra(int idObra)
    {
        print(idObra.ToString());
    }

    public void fecharGUI()
    {
        gameManager.setIsGamePaused(false);
        gameObject.SetActive(false);
    }
}