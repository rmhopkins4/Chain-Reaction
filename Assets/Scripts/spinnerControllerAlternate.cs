using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spinnerControllerAlternate : MonoBehaviour
{
    int moveDir;
    GameObject player;
    GameObject combo;
    public GameObject explosion;
    public GameObject elecCollect;
    public GameObject explosionParticle;
    public GameObject firePre;
    public float speed;
    private bool rings = true;
    Animator animator;
    CircleCollider2D circleCol;
    float startY;

    SpriteRenderer sr;
    float spriteTimer = 0;
    bool jumping = true;
    bool jumpinginAction = false;
    bool hopped = false;

    Rigidbody2D rb;

    AudioSource audioData;

    // Start is called before the first frame update
    void Start()
    {
        audioData = GetComponent<AudioSource>();

        rb = GetComponent<Rigidbody2D>();
        sr = this.GetComponent<SpriteRenderer>();
        startY = Random.Range(-2.8f, -2.6f);
        if (transform.position.x > 0)
        {
            moveDir = -1;
        }
        else
        {
            moveDir = 1;
        }

        player = GameObject.Find("scuttle boy_0");
        combo = GameObject.Find("ComboEmpty");
        animator = this.GetComponent<Animator>();
        circleCol = GetComponent<CircleCollider2D>();
        speed = Random.Range(3f, 6f);
        transform.position = new Vector3(transform.position.x,startY, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x + (speed * moveDir * Time.deltaTime), transform.position.y, 0);
        if (jumpinginAction == true)
        {
            if (hopped == false)
            {
                rb.AddForce(transform.up * 30, ForceMode2D.Impulse);
                rb.gravityScale = 3f;
                hopped = true;
            }
            else if(rb.position.y <= startY)
            {
                print("hey all, scott here");
                if (rings == true)
                {
                    Instantiate(explosionParticle, transform.position, Quaternion.identity);
                    Instantiate(firePre, new Vector3(this.transform.position.x, -3.5f, 0), Quaternion.identity);
                }
                rb.gravityScale = 0;
                jumpinginAction = false;
            }
        }
        if (transform.position.y <= startY+0.2f)
        {
            transform.position = new Vector3(transform.position.x, startY, 0);
        }
        //jump sometimes...{}
        if (jumping == true)
        {
            spriteTimer += Time.deltaTime;
            if (spriteTimer >= 0.15)
            {
                //sr.enabled = false;
                sr.color = new Color(0.3f, 0.3f, 0.3f);
            }
            if (spriteTimer >= 0.3)
            {
                //sr.enabled = true;
                sr.color = new Color(1, 1, 1);
            }
            if (spriteTimer >= 4.5)
            {
                sr.color = new Color(0.3f, 0.3f, 0.3f);
            }
            if (spriteTimer >= 0.6)
            {
                sr.color = new Color(1, 1, 1);
            }
            if (spriteTimer >= 0.75)
            {
                //jump
                spriteTimer = 0;
                jumping = false;
                jumpinginAction = true;
            }
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        //if i touch the player he takes damage. good
        if (collision.gameObject.tag == "Player")
        {
            print("hey all, scott here!");
            batteryLife.batteryTime -= 20;
            trying_to_script_post_processing.HurtChromAb();


            if (collision.transform.position.x < this.transform.position.x)
            {
                collision.GetComponent<Rigidbody2D>().velocity = (new Vector2(0, 0));
                collision.GetComponent<Rigidbody2D>().AddForce(new Vector2(-1100, 900));
            }
            else if (collision.transform.position.x > this.transform.position.x)
            {
                collision.GetComponent<Rigidbody2D>().velocity = (new Vector2(0, 0));
                collision.GetComponent<Rigidbody2D>().AddForce(new Vector2(1100, 900));
            }
        }

        if (rings == false)
        {
            //if it touches anything and has no rings, do all the things
            if (!collision.gameObject.name.Contains("Slash"))
            {
                if (scoreScript.realTimer >= 0)
                {
                    scoreScript.comboValue += 1;
                    //play sound pitch is increased
                    combo.GetComponent<AudioSource>().pitch += 0.1f;
                    combo.GetComponent<AudioSource>().Play(0);
                    //
                }
                else
                {
                    scoreScript.comboValue = 1;
                    //play sound pitch = 1
                    combo.GetComponent<AudioSource>().pitch = 1f;
                    combo.GetComponent<AudioSource>().Play(0);
                    //
                }
                scoreScript.comboTimer = 3;
                scoreScript.realTimer = 1.5f;
                combo.GetComponent<Animator>().Play("comboNumber", -1, 0f);
            }
            Explode();
        }
        if (rings == true)
        {
            //if i have rings and am touched by anything that isnt the player, lose the rings. 
            if (collision.gameObject.tag != "Player")
            {
                rings = false;
                audioData.Play(0);

                animator.Play("damageTakenAlternate");

                circleCol.radius = 0.7f;
            }

        }
        //if im touched by an explosion and have rings, run combo.
        if (collision.name.Contains("xplosion"))
        {
            if (rings == true)
            {
                print("COMBO!!");

                scoreScript.comboValue += 1;
                scoreScript.comboTimer = 3;
                combo.GetComponent<Animator>().Play("comboNumber", -1, 0f);
                //play sound pitch is increased
                combo.GetComponent<AudioSource>().pitch += 0.1f;
                combo.GetComponent<AudioSource>().Play(0);
                //

                scoreScript.scoreValue += 2;
            }
        }

        //if im slashed, run combo
        if (collision.gameObject.name.Contains("Slash"))
        {
            if (scoreScript.realTimer >= 0)
            {
                scoreScript.comboValue += 1;
                //play sound pitch is increased
                combo.GetComponent<AudioSource>().pitch += 0.1f;
                combo.GetComponent<AudioSource>().Play(0);
                //
            }
            else
            {
                scoreScript.comboValue = 1;
                //play sound pitch = 1
                combo.GetComponent<AudioSource>().pitch = 1f;
                combo.GetComponent<AudioSource>().Play(0);
                //
            }
            scoreScript.comboTimer = 3;
            scoreScript.realTimer = 1.5f;
            combo.GetComponent<Animator>().Play("comboNumber", -1, 0f);

            if (player.GetComponent<SpriteRenderer>().flipX == false)
            {
                //add force to the left 
                player.GetComponent<Rigidbody2D>().AddForce(transform.right * -900);
                //print("gaming");
            }
            else if (player.GetComponent<SpriteRenderer>().flipX == true)
            {
                //add force to the right
                player.GetComponent<Rigidbody2D>().AddForce(transform.right * 900);
                //print("gamingflipped");
            }

            collision.gameObject.GetComponent<PolygonCollider2D>().enabled = false;
        }
    }
    void Explode()
    {


        Instantiate(explosion, transform.position, Quaternion.identity);

        GameObject boomer1 = Instantiate(elecCollect, transform.position, Quaternion.identity);
        boomer1.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-1f, 1f) * 5000, Random.Range(-1f, 1f) * 5000));
        GameObject boomer2 = Instantiate(elecCollect, transform.position, Quaternion.identity);
        boomer2.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-1f, 1f) * 5000, Random.Range(-1f, 1f) * 5000));

        Destroy(this.gameObject);
        screenshake.shakeAmount = 0.4f;
        screenshake.shake = 0.18f;

        //run combo chec
    }
}
