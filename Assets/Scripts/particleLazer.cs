using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleLazer : MonoBehaviour
{
    [SerializeField] GameObject particles;
    float time;
    bool samtech=true;
    // Start is called before the first frame update
    void Start()
    {

        particles.SetActive(samtech);

    }

    // Update is called once per frame
    void Update()
    {
        
       
    }
    
}
