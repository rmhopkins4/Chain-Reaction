using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movementEnemy : MonoBehaviour
{
    int moveDir;
    float sinHeight;
    float startY;
    float speed;
    //float slowSpeed;
    float timer;

    AudioSource audioData;

    public static GameObject explosion;
    public GameObject explosion1;
    public GameObject explosion2;
    public GameObject laser;
    public static GameObject elecCollect;

    Vector3 rtoward;
    bool encounterPlayer = false;
    ParticleSystem particle;
    public float laserDelay;
    float delayTick;
    public bool encountered = false;
    RaycastHit2D explosioncircle;
    Animator anim;
    float Amplitudepositivity;
    float CosineorSine;
    public static bool explodecontact = false;

    public int elecChance;

    // Start is called before the first frame update
    void Start()
    {
        audioData = GetComponent<AudioSource>();
        CosineorSine = Random.Range(1, 2);
        startY = transform.position.y;
        startY = Random.Range(-1.5f, 4f);
        sinHeight = Random.Range(1.5f, 3f);
        speed = Random.Range(3f, 6f);
        //rtoward = GameObject.Find("scuttle boy_0").transform.position;
        particle = this.GetComponent<ParticleSystem>();
        anim = GetComponent<Animator>();
        
        if (transform.position.x > 0)
        {
            moveDir = -1;
        }
        else
        {
            moveDir = 1;
        }

        Amplitudepositivity = Random.Range(-1, 1);
        if (Amplitudepositivity == 0)
        {
            Amplitudepositivity = -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (CosineorSine == 2)
        {
            if (encounterPlayer == false)
            {
                transform.position = new Vector3(transform.position.x + (moveDir * speed * Time.deltaTime), startY + Amplitudepositivity * Mathf.Sin(gameObject.transform.position.x) * 2, 0);
            }
            else
            {
                transform.position = new Vector3(transform.position.x + (moveDir * 0.1f * Time.deltaTime), startY + (Amplitudepositivity * Mathf.Sin(gameObject.transform.position.x) * 2), 0);
            }
        }
        if (CosineorSine == 1)
        {
            if (encounterPlayer == false)
            {
                transform.position = new Vector3(transform.position.x + (moveDir * speed * Time.deltaTime), startY + Amplitudepositivity * Mathf.Cos(gameObject.transform.position.x) * 2, 0);
            }
            else
            {
                transform.position = new Vector3(transform.position.x + (moveDir * 0.1f * Time.deltaTime), startY + (Amplitudepositivity * Mathf.Cos(gameObject.transform.position.x) * 2), 0);
            }
        }
        if (encounterPlayer == true)
        {
            delayTick += Time.deltaTime;
            anim.SetBool("shoot", true);
            if (delayTick >= laserDelay)
            {
                if (encountered == false)
                {
                    //play shoot
                    audioData.Play(0);

                    GameObject projectile = Instantiate(laser, this.transform.position, Quaternion.identity);
                    anim.SetBool("shoot", false);

                    projectile.transform.right = this.transform.position - rtoward;
                    //instantiated object's right is (ssinstatiater position - target position)

                    encounterPlayer = false;
                    particle.Stop();
                    encountered = true;
                }

            }

        }

        if (Input.GetKey(KeyCode.K))
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
      

    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "despawn")
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Player")
        {
            //fire
            if (encounterPlayer == false && encountered == false)
            {
                encounterPlayer = true;
                rtoward = new Vector3(collision.gameObject.transform.position.x, collision.gameObject.transform.position.y, 0);
                particle.Play();
            }

        }

    }
     void Explode()
    {

        Instantiate(explosion, transform.position, Quaternion.identity);
        
        GameObject boomer1 = Instantiate(elecCollect, transform.position, Quaternion.identity);
        boomer1.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-1f, 1f) * 5000, Random.Range(-1f, 1f) * 5000));
        GameObject boomer2 = Instantiate(elecCollect, transform.position, Quaternion.identity);
        boomer2.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-1f, 1f) * 5000, Random.Range(-1f, 1f) * 5000));
        GameObject boomer3 = Instantiate(elecCollect, transform.position, Quaternion.identity);
        boomer3.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-1f, 1f) * 5000, Random.Range(-1f, 1f) * 5000));

        //screenshake
        
        

        Destroy(gameObject);

        
    }
}

