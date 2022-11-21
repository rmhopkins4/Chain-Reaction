using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scoreScript : MonoBehaviour
{
    public static int scoreValue = 0;
    public static int comboValue = 0;
    Text text;
    public static float comboTimer = 0;
    public static float realTimer = 0;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        Storage.score = scoreValue;
        if (this.name.Contains("Score"))
        {
            text.text = "Score: " + scoreValue;
        }
        if (this.name.Contains("Combo"))
        {
            text.text = "" + comboValue;
            if(comboValue == 0)
            {
                this.text.enabled = false;
            }
            if(comboValue != 0)
            {
                this.text.enabled = true;
            }
        }
        comboTimer -= Time.deltaTime;
        realTimer -= Time.deltaTime;
        if(comboTimer <= 0)
        {
            comboValue = 0;
        }
        if(realTimer <= 0)
        {
            //add combo to score
            //print("add combo to score");
        }
    }
}
