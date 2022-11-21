using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controlsMenu : MonoBehaviour
{
    [SerializeField] Sprite[] sprites;
    [SerializeField] string keyId;

    SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(keyId == "W")
        {
            if(Input.GetKey(KeyCode.W))
            {
                sr.color = new Color(.8f,.8f,.8f);
                sr.sprite = sprites[1];
            }
            if(Input.GetKeyUp(KeyCode.W))
            {
                sr.color = Color.white;
                sr.sprite = sprites[0];
            }
        }
        if (keyId == "A")
        {
            if (Input.GetKey(KeyCode.A))
            {
                sr.color = new Color(.8f,.8f,.8f);
                sr.sprite = sprites[1];
            }
            if (Input.GetKeyUp(KeyCode.A))
            {
                sr.color = Color.white;
                sr.sprite = sprites[0];
            }
        }
        if (keyId == "S")
        {
            if (Input.GetKey(KeyCode.S))
            {
                sr.color = new Color(.8f,.8f,.8f);
                sr.sprite = sprites[1];
            }
            if (Input.GetKeyUp(KeyCode.S))
            {
                sr.color = Color.white;
                sr.sprite = sprites[0];
            }
        }
        if (keyId == "D")
        {
            if (Input.GetKey(KeyCode.D))
            {
                sr.color = new Color(.8f,.8f,.8f);
                sr.sprite = sprites[1];
            }
            if (Input.GetKeyUp(KeyCode.D))
            {
                sr.color = Color.white;
                sr.sprite = sprites[0];
            }
        }
        if (keyId == "J")
        {
            if (Input.GetKey(KeyCode.J))
            {
                sr.color = new Color(.8f,.8f,.8f);
                sr.sprite = sprites[1];
            }
            if (Input.GetKeyUp(KeyCode.J))
            {
                sr.color = Color.white;
                sr.sprite = sprites[0];
            }
        }
        if (keyId == "SP")
        {
            if (Input.GetKey(KeyCode.Space))
            {
                sr.color = new Color(.8f,.8f,.8f);
                sr.sprite = sprites[1];
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                sr.color = Color.white;
                sr.sprite = sprites[0];
            }
        }
    }
}
