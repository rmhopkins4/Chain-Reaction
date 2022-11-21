using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dropperParticle : MonoBehaviour
{
    ParticleSystem particle;
    float time;
    // Start is called before the first frame update
    void Start()
    {

        particle = GetComponent<ParticleSystem>();

    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        particle.Play();
       
    }

}
