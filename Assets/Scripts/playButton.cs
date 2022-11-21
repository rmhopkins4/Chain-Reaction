using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playButton : MonoBehaviour
{
    SpriteRenderer sr;
    public Sprite[] sprites = new Sprite[2];
    AudioSource audioData;
    public GameObject transitionOut;
    Animation anim;
    int timer;
    bool swapScene = false;

    private float newDelay = 0f;
    bool clicked = false;
    string sceneName;
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;
        audioData = GetComponent<AudioSource>();
        sr = GetComponent<SpriteRenderer>();
        anim = transitionOut.GetComponent<Animation>();
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        //if (sceneName == "Start" || sceneName == "End")
        //{
            if (clicked == true)
            {
                anim.Play("fadetoBlack");
                anim.Play("blacktowhite");
                swapScene = true;
                newDelay += Time.deltaTime;
                if (newDelay >= 1)
                {
                    if (swapScene == true)
                    {
                        scoreScript.scoreValue = 0;
                        scoreScript.comboValue = 0;
                        regainUI.UICOLLECTED = 0;
                        RandomSpawn.maxTime = 4;
                        batteryLife.batteryTime = 99;
                        elecPickup.elecCollected = 0;
                        audioData.Play(0);

                    Cursor.visible = false;

                    SceneManager.LoadScene("Play");
                        clicked = false;
                        swapScene = false;
                    }

                }

          //  }
        }
        //else
        //{
        //    SceneManager.LoadScene("SampleScene");
        //}
       
        
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
       
        // GameObject.Find("fade out").GetComponent<Animation>().Play();
       
    }
}
