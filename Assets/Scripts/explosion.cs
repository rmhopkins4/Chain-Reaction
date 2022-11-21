using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosion : MonoBehaviour
{
    float timer;
    Animator anim;

    AudioSource audio;
    private float randomPitch;

    CircleCollider2D circleCol;
    SpriteRenderer sr;
    //public Sprite[] sprites = new Sprite[3];
    int whichSprite;
    // Start is called before the first frame update
    void Start()
    {
        whichSprite = Random.Range(0, 3);

        randomPitch = Random.Range(0.8f, 1f);
        audio = GetComponent<AudioSource>();
        audio.pitch = randomPitch;

        scoreScript.scoreValue += 1;
        anim = GetComponent<Animator>();
        circleCol = GetComponent<CircleCollider2D>();
       
    }

    // Update is called once per frame
    void Update()
    {
        if (whichSprite == 0)
        {
            anim.Play("explosionCombined");
        }
        else if (whichSprite == 1)
        {
            anim.Play("explosionDiagonal");
        }
        else if (whichSprite == 2)
        {
            anim.Play("explosionStraight");
        }
        timer += Time.deltaTime;

        if(timer >= 0.2)
        {
            circleCol.enabled = true;
        }
        if (timer >= .45)
        {
            Destroy(gameObject);
        }
        circleCol.radius += Time.deltaTime * 3;
    }    
}
