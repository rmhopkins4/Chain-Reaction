using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HParticle : MonoBehaviour
{
    ParticleSystem healparticle;
    // Start is called before the first frame update
    void Start()
    {
        healparticle = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
      if(elecPickup.if60 == true)
        {
            elecPickup.if60 = false;
            healparticle.Play();
        }
    }
}
