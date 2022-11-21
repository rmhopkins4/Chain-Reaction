using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class tutButton : MonoBehaviour
{
    SpriteRenderer sr;
    public Sprite[] sprites = new Sprite[2];
    AudioSource audioData;
    public GameObject transitionGrey;
    Animation anim;
    bool swapScene = false;
    bool clicked = false;
    private float newDelay = 0f;
   


    void Start()
    {
       
        audioData = GetComponent<AudioSource>();
        anim = transitionGrey.GetComponent<Animation>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
      
        if (clicked == true)
        {
            anim.Play("toGrey");
            swapScene = true;
            newDelay += Time.deltaTime;
            if (newDelay >= 1)
            {
                if (swapScene == true)
                {
                    audioData.Play(0);
                    SceneManager.LoadScene("Tutorial");
                    clicked = false;
                    swapScene = false;
                }

            }

        }
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
        clicked = true;
    }
}
