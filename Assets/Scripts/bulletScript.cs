using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletScript : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float fallSpeed;
    ParticleSystem particle;
    public GameObject particleeffect;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        particle = GetComponent<ParticleSystem>();
        rb.velocity = transform.up * -fallSpeed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            batteryLife.batteryTime -= 20;
            trying_to_script_post_processing.HurtChromAb();

            Destroy(this.gameObject, 0.0125f);
            if (collider.GetComponent<SpriteRenderer>().flipX == false)
            {
                collider.GetComponent<Rigidbody2D>().AddForce(new Vector2(-600, 0));
            }
            else if (collider.GetComponent<SpriteRenderer>().flipX == true)
            {
                collider.GetComponent<Rigidbody2D>().AddForce(new Vector2(600, 0));
            }
        }
        if (collider.gameObject.tag == "Floor" || collider.gameObject.tag == "Player")
        {
            Instantiate(particleeffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
