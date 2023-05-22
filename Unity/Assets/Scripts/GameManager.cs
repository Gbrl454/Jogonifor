using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private bool isGamePaused = false;
   public List<Sprite> obras;

    public void TogglePause()
    {
        isGamePaused = !isGamePaused;
    }

    public bool getIsGamePaused()
    {
        return isGamePaused;
    }

    public void setIsGamePaused(bool isGamePaused)
    {
        this.isGamePaused = isGamePaused;
    }

    public Sprite getImage(int idObra)
    {
        return obras[idObra];
    }
}
