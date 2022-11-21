using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLazer : MonoBehaviour
{
    GameObject player;
    Rigidbody2D rb;
    int timer, time, playerx, playery;
    public int laserSpeed;
    public GameObject particleeffect;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("scuttle boy_0");
        rb = GetComponent<Rigidbody2D>();
        //transform.right = this.transform.position - player.transform.position;
    }
            
    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = transform.right * -laserSpeed;
    }

    void OnTriggerEnter2D(Collider2D col)
    {

        if (col.gameObject.tag == "Player")
        {
            batteryLife.batteryTime -= 20;
            trying_to_script_post_processing.HurtChromAb();

            Destroy(gameObject, 0.0125f);
            if (col.GetComponent<SpriteRenderer>().flipX == false)
            {
                col.GetComponent<Rigidbody2D>().AddForce(new Vector2(-600, 0));
            }
            else if (col.GetComponent<SpriteRenderer>().flipX == true)
            {
                col.GetComponent<Rigidbody2D>().AddForce(new Vector2(600, 0));
            }
            Instantiate(particleeffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        if (col.gameObject.name.Contains("spinner"))
        {
            Destroy(gameObject, 0.0125f);
            Instantiate(particleeffect, transform.position, Quaternion.identity);
        }
        if (col.gameObject.tag == "Floor")
        {
            Instantiate(particleeffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
