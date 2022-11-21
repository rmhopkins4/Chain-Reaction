using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class scuttleSounds : MonoBehaviour
{
    //[SerializeField] GameObject abberator;
    AudioSource audioSource;
    [SerializeField] AudioClip jumpClip, landClip, slashClip, deathClip, hurtClip, initialDeathClip, walkClip, collectClip, gainClip;
    private AudioClip randomWalktoPlay;

    bool grounded = false;
    float timeUp;
    bool initialPlayed = false;
    bool deathPlayed = false;

    [SerializeField] float footStepDelay;
    float timerFoot;
    // Start is called before the first frame update
    void Start()
    {

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(elecPickup.soundPlay == true)
        {
            audioSource.PlayOneShot(gainClip);
            elecPickup.soundPlay = false;
            //print("HEWEEEEEEEEEEEEEEEE");
        }
        if (!(Input.GetAxisRaw("Horizontal") == 0))
        {
            if (grounded == true)
            {
                timerFoot += Time.deltaTime;
                if (timerFoot >= footStepDelay)
                {
                    timerFoot = 0;
                    randomWalktoPlay = walkClip;
                    if (grounded == true)
                    {
                        audioSource.PlayOneShot(randomWalktoPlay);
                    }
                }
            }
        }
        if(Input.GetAxisRaw("Horizontal") == 0)
        {
            timerFoot = 0;
        }

        if (Input.GetButtonDown("Jump"))
        {
            if (grounded == true)
            {
                audioSource.PlayOneShot(jumpClip);
            }
            grounded = false;
        }
        if (Input.GetButton("Jump"))
        {
            //audioSource.PlayOneShot(jumpClip);
            grounded = false;
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            audioSource.PlayOneShot(slashClip);
        }
        
        //if death
        if(batteryLife.batteryTime <= 0)
        {
            if (initialPlayed == false)
            {
                audioSource.PlayOneShot(initialDeathClip);
                audioSource.PlayOneShot(initialDeathClip);
                initialPlayed = true;
            }
            timeUp+= Time.deltaTime;
        }
        //death explosion
        if(timeUp >= 0.7f)
        {
            if (deathPlayed == false)
            {
                audioSource.PlayOneShot(deathClip);
                deathPlayed = true;
                screenshake.shake = 0.52f;
                screenshake.shakeAmount = 0.5f;
            }
        }
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Floor")
        {
            audioSource.PlayOneShot(landClip);
            grounded = true;
            timerFoot = -0.2f;
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name.Contains("la"))
        {
            if (batteryLife.batteryTime >= 0)
            {
                audioSource.PlayOneShot(hurtClip);
                screenshake.shake = 0.25f;
            }
        }
        if (col.gameObject.name.Contains("quare"))
        {
            audioSource.PlayOneShot(collectClip);
        }

        if (col.gameObject.name.Contains("Bull"))
        {
            if (batteryLife.batteryTime >= 0)
            {
                audioSource.PlayOneShot(hurtClip);
                screenshake.shake = 0.175f;
            }
        }
        if (col.gameObject.name.Contains("spin"))
        {
            if (batteryLife.batteryTime >= 0)
            {
                audioSource.PlayOneShot(hurtClip);
                screenshake.shake = 0.175f;
            }
        }
        if (col.gameObject.name.Contains("fire"))
        {
            if (batteryLife.batteryTime >= 0)
            {
                audioSource.PlayOneShot(hurtClip);
                screenshake.shake = 0.175f;
            }
        }
    }
}
