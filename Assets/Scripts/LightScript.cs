using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightScript : MonoBehaviour
{
    public LightSwitch lightSwitch;
    public SpriteRenderer lightSource;
    public Sprite lightOff;
    public Sprite lightOn;
    public GameObject lightBeam;
    void Update()
    {
        if (lightSwitch.isSwitched)
        {
            lightSource.sprite = lightOn;
        }
        else
        {
            lightSource.sprite = lightOff;

        }
        lightBeam.SetActive(lightSwitch.isSwitched);
    }
}
