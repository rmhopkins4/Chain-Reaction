using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class creditsScript : MonoBehaviour
{
    SpriteRenderer sr;
    public Sprite[] sprites = new Sprite[2];
    AudioSource audioData;
    // Start is called before the first frame update
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
        //enable credits overlay
        GameObject.Find("CreditsOverlay").GetComponent<Canvas>().enabled = true;
        //turn on spriterenderer for credits tint
        GameObject.Find("CreditsTint").GetComponent<SpriteRenderer>().enabled = true;
    }
}
