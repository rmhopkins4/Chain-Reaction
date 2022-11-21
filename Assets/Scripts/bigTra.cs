using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bigTra : MonoBehaviour
{
    Animator Anim;
    // Start is called before the first frame update
    void Start()
    {
        Anim = GetComponent<Animator>();
        Anim.Play("bigTransition");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
