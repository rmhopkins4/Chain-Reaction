using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bomberMoveAlternate : MonoBehaviour
{
    public static bool explodecontact2 = false;
    int moveDir;
    [SerializeField] float sinHeight;
    float startY;
    float speed;

    AudioSource audioData;

    public GameObject explosion;
    public GameObject explosion1;
    public GameObject explosion2;
    public GameObject elecCollect;

    [SerializeField] GameObject bulletObj;
    bool allDone = false;
    bool shotAlready1 = false;
    bool shotAlready2 = false;
    bool shotAlready3 = false;
    float shootCallibrate;
    bool shoot = false;

    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        audioData = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

        startY = Random.Range(3f, 4f);
        startY = transform.position.y;
        sinHeight = Random.Range(3.121312312f, 4f);
        speed = Random.Range(2f, 4f);
        if (transform.position.x > 0)
        {
            moveDir = -1;
        }
        else
        {
            moveDir = 1;
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x + (moveDir * speed * Time.deltaTime), startY * 1 + Mathf.Sin(gameObject.transform.position.x) * 2 + sinHeight, 0);
        if (explodecontact2 == true)
        {
            Explode();
            explodecontact2 = false;
        }

        if (Mathf.Abs((transform.position.x) - (GameObject.Find("scuttle boy_0").transform.position.x)) <= 4)
        { shoot = true; }
        if (shoot == true)
        {
            if (allDone == false)
            {
               animator.Play("bomberShootAlt");
                allDone = true;
            }
            shootCallibrate += Time.deltaTime;
            if (shootCallibrate >= 0.4f)
            {
                if (shotAlready1 == false)
                {
                    audioData.Play(0);
                    Instantiate(bulletObj, new Vector3(transform.position.x, transform.position.y - 0.8f, 0), Quaternion.identity);
                    Instantiate(bulletObj, new Vector3(transform.position.x, transform.position.y - 0.8f, 0), Quaternion.Euler(0f, 0f, -25f));
                    Instantiate(bulletObj, new Vector3(transform.position.x, transform.position.y - 0.8f, 0), Quaternion.Euler(0f, 0f, 25f));
                    shotAlready1 = true;
                }
            }
            if (shootCallibrate >= 0.7f)
            {
                if (shotAlready2 == false)
                {
                    audioData.Play(0);
                    Instantiate(bulletObj, new Vector2(transform.position.x, transform.position.y - 0.8f), Quaternion.identity);
                    Instantiate(bulletObj, new Vector2(transform.position.x, transform.position.y - 0.8f), Quaternion.Euler(0f, 0f, -35f));
                    Instantiate(bulletObj, new Vector2(transform.position.x, transform.position.y - 0.8f), Quaternion.Euler(0f, 0f, 35f));
                    shotAlready2 = true;
                }
            }
            if (shootCallibrate >= 1f)
            {
                if (shotAlready3 == false)
                {
                    audioData.Play(0);
                    Instantiate(bulletObj, new Vector2(transform.position.x, transform.position.y - 0.8f), Quaternion.identity);
                    Instantiate(bulletObj, new Vector2(transform.position.x, transform.position.y - 0.8f), Quaternion.Euler(0f, 0f, -35));
                    Instantiate(bulletObj, new Vector2(transform.position.x, transform.position.y - 0.8f), Quaternion.Euler(0f, 0f, 35));
                    shotAlready3 = true;
                }
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "despawn")
        {
            Destroy(gameObject);
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

        Destroy(gameObject);

    }
}