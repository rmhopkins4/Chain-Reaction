using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class elecPickup : MonoBehaviour
{
    public static int elecCollected = 0;
    public static bool if60 = false;
    public static bool soundPlay = false;
    Animator animator;
   
    AudioSource audioSource;
  
    GameObject player;
    Vector3 rtoward;
    Rigidbody2D rb;

    float moveTimer;
    public float moveWhen;
    bool collected = false;

    // Start is called before the first frame update
    void Start()
    {
        
        animator = this.GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        regainUI.UICOLLECTED = elecCollected;
        moveTimer += Time.deltaTime;
        if (moveTimer >= moveWhen)
        {
            this.transform.right = this.transform.position - rtoward;
            rb.AddForce(-700 * transform.right);
            rtoward = new Vector3(player.gameObject.transform.position.x, player.gameObject.transform.position.y, 0);
        }
        if(collected == true)
        {
            Destroy(this.gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name.Contains("scuttle"))
        {
            elecCollected++;

            if (elecCollected >= 50)
            {
                soundPlay = true;
                //print("healthBoost");
                batteryLife.batteryTime += 31;
                elecCollected -=50;
                if60 = true;
                
            }
            collected = true;
            //Destroy(this.gameObject);

        }
    }
}
