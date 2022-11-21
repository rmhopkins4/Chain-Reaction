using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class trying_to_script_post_processing : MonoBehaviour
{
    public static ChromaticAberration chromAb;
    private Vignette vig;
    public static PostProcessVolume volume;

    public static bool counting = false;
    public static float chromTimer = 0;

    private float initialVignetteValue;
    // Start is called before the first frame update
    void Start()
    {
        volume = gameObject.GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings(out chromAb);
        volume.profile.TryGetSettings(out vig);

        initialVignetteValue = vig.intensity.value;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //chromatic aberration
        if(counting == true)
        {
            chromTimer -= Time.deltaTime * 4;
        }
        chromAb.intensity.value = chromTimer;
        Mathf.Clamp(chromTimer, 0, 1);

        //vignette
        if (batteryLife.batteryTime <= 20)
        {
            if (vig.intensity.value <= 0.4f)
            {
                vig.intensity.value += 0.02f;
            }
        }
        if(batteryLife.batteryTime > 20)
        {
            if(vig.intensity.value > initialVignetteValue)
            {
                vig.intensity.value -= 0.02f;
            }
            else if(vig.intensity.value < initialVignetteValue)
            {
                vig.intensity.value = initialVignetteValue;
            }
        }
    }
    public static void HurtChromAb()
    {
        counting = true;
        chromTimer = 0;
        print("running hurtChromAb");
        chromAb.enabled.value = true;
        chromTimer = 0.55f;
    }
}
