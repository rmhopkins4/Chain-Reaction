using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireController : MonoBehaviour
{
    Animator animator;
    float fireTimer;
    AudioSource audioData;
    // Start is called before the first frame update
    void Start()
    {
        audioData = GetComponent<AudioSource>();
        audioData.Play(0);
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        fireTimer += Time.deltaTime;
        if (fireTimer >= 2)
        {
            this.GetComponent<BoxCollider2D>().enabled = false;
            animator.Play("fireDissipate");
        }
        if (fireTimer >= 2.25f)
        {
            Destroy(this.gameObject);
            this.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {

        if (col.gameObject.tag == "Player")
        {
            batteryLife.batteryTime -= 20;
            if (fireTimer <= 2)
            {
                fireTimer = 2;
            }
            if (col.GetComponent<SpriteRenderer>().flipX == false)
            {
                col.GetComponent<Rigidbody2D>().AddForce(new Vector2(-600, 0));
            }
            else if (col.GetComponent<SpriteRenderer>().flipX == true)
            {
                col.GetComponent<Rigidbody2D>().AddForce(new Vector2(600, 0));
            }
            //Destroy(this.gameObject);
        }
    }
}
