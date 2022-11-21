using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class screenshake : MonoBehaviour
{
    [SerializeField] Camera Camera; // set this via inspector
    public static float shake = 0f;
    public static float shakeAmount = 0.2f;
    float decreaseFactor = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (shake > 0)
        {
            Camera.transform.localPosition = Random.insideUnitCircle * shakeAmount;
            Camera.transform.position = new Vector3(Camera.transform.position.x, Camera.transform.position.y + 0.97f, -10);
            shake -= Time.deltaTime * decreaseFactor;

        }
        else
        {
            shake = 0.0f;
            Camera.transform.position = new Vector3(0, 0.97f, -10);
            shakeAmount = 0.2f;
        }
    }
}
