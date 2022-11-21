using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class feedbackDisguise : MonoBehaviour
{
    SpriteRenderer sr;
    public Sprite[] sprites = new Sprite[2];
    AudioSource audioData;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        audioData = GetComponent<AudioSource>();

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
    }
}
