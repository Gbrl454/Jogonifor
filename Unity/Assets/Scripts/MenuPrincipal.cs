using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    private string cenaJogo;
    public void btnJogar()
    {
        SceneManager.LoadScene(cenaJogo);
    }
}