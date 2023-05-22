using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMenu : MonoBehaviour
{

    public AudioSource audioSourceMusicaDeFundo;
    public AudioClip[] musicasDeFundo;

    void Start()
    {
        int IndexMusica = Random.Range(0, musicasDeFundo.Length);
        AudioClip musicaDeFundoMenu = musicasDeFundo[IndexMusica];
        audioSourceMusicaDeFundo.clip = musicaDeFundoMenu;
        audioSourceMusicaDeFundo.Play();
    }
}
