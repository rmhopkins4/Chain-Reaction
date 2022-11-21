using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slashScript : MonoBehaviour
{
    Animator animator;
    public float aliveTime = 0;
    public float hitboxTime = 0;
    float currentTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if(currentTime >= aliveTime)
        {
            //print("die");
            Destroy(this.gameObject);
        }
        if(currentTime >= hitboxTime)
        {
            //print("no hitbox");
            this.GetComponent<Collider2D>().enabled = false;
        }
    }
}
