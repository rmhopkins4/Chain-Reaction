using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class celesteBerry : MonoBehaviour
{
    private bool touched = false;
    AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(touched == true)
        {
            //noice
            this.transform.position = new Vector2(transform.position.x + 0.05f, transform.position.y - 0.05f);
        }
    }
    private void OnMouseOver()
    {
        if (touched == false)
        {
            audio.Play(0);
        }
        print("celeste");
        touched = true;
    }
}
