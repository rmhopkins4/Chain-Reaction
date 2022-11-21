using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dustScript : MonoBehaviour
{
    //Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        //Destroy(gameObject, .3f);
        //animator = GetComponent<Animator>();
        Destroy(gameObject, this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length - 0.05f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }
}
