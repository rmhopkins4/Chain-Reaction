using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explodeChildBomber : MonoBehaviour
{
    GameObject player;
    GameObject combo;
    public GameObject explosion;
    public GameObject explosion1;
    public GameObject explosion2;
    public GameObject elecCollect;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("scuttle boy_0");
        combo = GameObject.Find("ComboEmpty");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D collision)
    {

        if(collision.name.Contains("xplosion"))
        {
            //print("COMBO!!");


            scoreScript.comboValue += 1;
            scoreScript.comboTimer = 3;
            combo.GetComponent<Animator>().Play("comboNumber", -1, 0f);
            //play sound pitch is increased
            combo.GetComponent<AudioSource>().pitch += 0.1f;
            combo.GetComponent<AudioSource>().Play(0);
            //

            scoreScript.scoreValue += 2;
        }

        Explode();

        if (collision.gameObject.name.Contains("Slash"))
        {
            if(scoreScript.realTimer >= 0)
            {
                scoreScript.comboValue += 1;
                //play sound pitch is increased
                combo.GetComponent<AudioSource>().pitch += 0.1f;
                combo.GetComponent<AudioSource>().Play(0);
                //
            }
            else {
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
        GameObject boomer3 = Instantiate(elecCollect, transform.position, Quaternion.identity);
        boomer3.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-1f, 1f) * 5000, Random.Range(-1f, 1f) * 5000));
        GameObject boomer4 = Instantiate(elecCollect, transform.position, Quaternion.identity);
        boomer4.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-1f, 1f) * 5000, Random.Range(-1f, 1f) * 5000));

        Destroy(transform.parent.gameObject);
        screenshake.shakeAmount = 0.4f;
        screenshake.shake = 0.18f;
    }

}
