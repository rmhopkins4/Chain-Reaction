using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class batteryLife : MonoBehaviour
{
    public static float batteryTime = 99;
    float timeCounter = 0;
    public float timeLoss;
    SpriteRenderer sr;
    public Sprite[] sprites =new Sprite[6];

    private float batteryScale = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        batteryTime = Mathf.Clamp(batteryTime, -100, 99);
        
        batteryScale += 0.01f;
        batteryScale = Mathf.Clamp(batteryScale, 0, 0.5f);
        //print(batteryTime + "");

        timeCounter += Time.deltaTime;
        if(timeCounter >= timeLoss)
        {
            timeCounter = 0;
            //batteryTime -= (batteryScale);

        }

        if (batteryTime <= 0)
        {
            sr.sprite = sprites[0];
        }
        if (batteryTime >= 0)
        {
            sr.sprite = sprites[1];
        }
        if (batteryTime >= 20)
        {
            sr.sprite = sprites[2];
        }
        if (batteryTime >= 40)
        {
            sr.sprite = sprites[3];
        }
        if (batteryTime >= 60)
        {
            sr.sprite = sprites[4];
        }
        if (batteryTime >= 80)
        {
            sr.sprite = sprites[5];
        }
    }
}
