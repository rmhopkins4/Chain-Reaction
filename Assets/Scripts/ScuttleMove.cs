using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScuttleMove : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;
    Animation anim2; //for fadeout;
    SpriteRenderer sr;
    public GameObject fadeToBlack;
    public GameObject slashEffect;
    public GameObject jumpEffects;
    public GameObject particleDeath;

    float maxSpeed = 9f;
    float speedCoefficient = 4;

    public float fallSpeed;
    public float riseForce;

    bool grounded = false;

    float risePeakSpeed = 1;
    float fallPeakSpeed = 1;
    public float peakFallCoefficient;
    bool peakedUp = false;
    bool peakedDown = false;

    float slashTimer;
    public float slashDelay;

    bool momentumCancelled = false;

    bool dead = false;

    bool ducking = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        anim2 = fadeToBlack.GetComponent<Animation>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        //momentumCancel
        if (Input.GetButtonUp("Jump"))
        {
            if (momentumCancelled == false)
            {
                if (rb.velocity.y >= -2)
                {
                    rb.velocity = new Vector2(rb.velocity.x, -0.4f);
                    momentumCancelled = true;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (grounded == true)
            {
                ducking = true;
                anim.Play("duckStart");
                rb.velocity = new Vector2(rb.velocity.x * 0.4f, rb.velocity.y);
            }
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            ducking = false;
            anim.Play("idle");
        }
        if (ducking == true)
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionX;
            rb.GetComponent<BoxCollider2D>().offset = new Vector2(0, -0.22f);
            rb.GetComponent<BoxCollider2D>().size = new Vector2(0.8094169f, 0.45f);
        }
        else
        {
            rb.constraints = RigidbodyConstraints2D.None;
            rb.GetComponent<BoxCollider2D>().offset = new Vector2(0, -0.01408231f);
            rb.GetComponent<BoxCollider2D>().size = new Vector2(0.8094169f, 0.8640459f);
        }

    }
    void FixedUpdate()
    {

        //death
        if (batteryLife.batteryTime <= 0)
        {
            GameOver();

        }
        //hori
        if (ducking == false)
        {
            int x = (int)Input.GetAxisRaw("Horizontal");
            if (x == -1)
            {
                if (rb.velocity.x >= -maxSpeed)
                {

                    rb.velocity -= new Vector2(maxSpeed / speedCoefficient, 0);
                    anim.SetBool("Walking", true);
                }
            }
            if (x == 1)
            {

                if (rb.velocity.x <= maxSpeed)
                {

                    rb.velocity += new Vector2(maxSpeed / speedCoefficient, 0);
                    anim.SetBool("Walking", true);
                }
            }
        }
            if (rb.velocity.x == 0)
            {
                anim.SetBool("Walking", false);
            }
        
        //deccelerate
        if (rb.velocity.x != 0 && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            if (rb.velocity.x > 0)
            {
                rb.velocity -= new Vector2(maxSpeed / (speedCoefficient * 2), 0);
                if (rb.velocity.x <= 0)
                {
                    rb.velocity = new Vector2(0, rb.velocity.y);
                }
            }
            if (rb.velocity.x < 0)
            {
                rb.velocity += new Vector2(maxSpeed / (speedCoefficient * 2), 0);
                if (rb.velocity.x >= 0)
                {
                    rb.velocity = new Vector2(0, rb.velocity.y);
                }
            }
        }
        //flip
        if (dead == false) { if (rb.velocity.x > 0) { if (Input.GetKey(KeyCode.D)) { sr.flipX = false; } } if (rb.velocity.x < 0) { if (Input.GetKey(KeyCode.A)) { sr.flipX = true; } } }
        //jump
        if (Input.GetButton("Jump"))
        {
            
            if (grounded == true)
            {
                rb.AddForce(new Vector2(0, riseForce), ForceMode2D.Impulse);
                anim.Play("jumprise");

                if (sr.flipX == false) { GameObject dust = Instantiate(jumpEffects, new Vector2(this.transform.position.x, this.transform.position.y + .05f), Quaternion.identity); }
                else { GameObject dust = Instantiate(jumpEffects, new Vector2(this.transform.position.x, this.transform.position.y + .05f), Quaternion.Euler(0, 180, 0)); }
                grounded = false;
            }
        }
        //peak
        if (grounded == false)
        {
            if (rb.velocity.y <= risePeakSpeed && peakedUp == false)
            {
                fallSpeed = fallSpeed * peakFallCoefficient;
                peakedUp = true;
                anim.SetBool("Peak", true);

                if (!anim.GetCurrentAnimatorStateInfo(0).IsName("rightswing") && !anim.GetCurrentAnimatorStateInfo(0).IsName("backswing") && !anim.GetCurrentAnimatorStateInfo(0).IsName("upswing") && !anim.GetCurrentAnimatorStateInfo(0).IsName("stomp"))
                    anim.Play("airtransition");

            }
            if (-rb.velocity.y >= -fallPeakSpeed && peakedDown == false)
            {
                fallSpeed = fallSpeed / peakFallCoefficient;
                peakedDown = true;
                anim.SetBool("Peak", false);
            }

        }
        //fall
        if (grounded == false)
        {
            rb.velocity += new Vector2(0, -fallSpeed);
            //sr.flipX = false;
        }
        //attacks
        if (Input.GetKey(KeyCode.J))
        {
            if (dead == false)
            {
                if (slashTimer >= slashDelay)
                {
                    slashTimer = 0;
                    anim.Play("rightswing");
                    screenshake.shake = 0.1f;
                    screenshake.shakeAmount = 0.05f;

                    if (sr.flipX == false)
                    {
                        GameObject slashChild = Instantiate(slashEffect, this.GetComponentInChildren<Transform>().position, Quaternion.identity);
                        slashChild.transform.parent = gameObject.transform;
                        //print("forward | flipX == false");
                    }
                    else
                    {
                        GameObject slashChild = Instantiate(slashEffect, this.GetComponentInChildren<Transform>().position, Quaternion.Euler(0, 180, 0));
                        slashChild.transform.parent = gameObject.transform;
                        //print("back | flipX == true");
                    }
                }
            }
        }
        slashTimer += Time.deltaTime;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag.ToString() == ("Floor"))
        {
            anim.Play("jumpfall");
            anim.SetBool("Grounded", true);
            grounded = true; 
            peakedUp = false;
            peakedDown = false;
            momentumCancelled = false;

            //animate

            GameObject dust = Instantiate(jumpEffects, new Vector2(this.transform.position.x, this.transform.position.y + .05f), Quaternion.identity);
            dust.GetComponent<Animator>().Play("dustLand");

            
        }
        
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name.Contains("la"))
        {

            ducking = false;
            anim.Play("hurt");
            //chrom ab
            //trying_to_script_post_processing.HurtChromAb();
            print("hit by laser");

        }
        if (col.gameObject.name.Contains("Bull"))
        {

            ducking = false;
                anim.Play("hurt");
            //trying_to_script_post_processing.HurtChromAb();
            //screen shake
            //
            print("hit by bullet");

        }
        if (col.gameObject.name.Contains("spin"))
        {

            ducking = false;
            anim.Play("hurt");
            //trying_to_script_post_processing.HurtChromAb();
            //screen shake
            //
            print("hit by spinner");

        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag.ToString() == ("Floor"))
        {
            grounded = false;
            anim.SetBool("Grounded", false);
        }
    }
    void GameOver()
    {
        dead = true;
        anim.Play("death");
        Instantiate(particleDeath, transform.position, Quaternion.identity);
        Destroy(this.gameObject, 1.1f);

        rb.constraints = RigidbodyConstraints2D.FreezeAll;
       
        //bring up death screen, ask player to type in name for score. include button to return to menu to restart or quit.
        GuiCode.isGameOver = true;
        anim2.Play("fadetoBlack");
    }
}
