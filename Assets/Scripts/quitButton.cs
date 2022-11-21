using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class quitButton : MonoBehaviour
{
    SpriteRenderer sr;
    public Sprite[] sprites = new Sprite[2];
    AudioSource audioData;
    void Start()
    {
        audioData = GetComponent<AudioSource>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnMouseOver()
    {
        sr.sprite = sprites[1];
        sr.color = new Color(.8f, .8f, .8f);
    }
    void OnMouseExit()
    {
        sr.sprite = sprites[0];
        sr.color = Color.white;
    }
    void OnMouseDown()
    {
        audioData.Play(0);
        //UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
}
