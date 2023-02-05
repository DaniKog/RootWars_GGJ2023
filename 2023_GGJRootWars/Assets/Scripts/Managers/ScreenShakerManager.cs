using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShakerManager : MonoBehaviour
{
    public static ScreenShakerManager instance;

    // Desired duration of the shake effect
    private float defaultShakeDuraction = 0.5f;
    ShakenObject[] shakenObjects;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        shakenObjects = GameObject.FindObjectsOfType<ShakenObject>();
    }

    public void TriggerShake(float duration)
    {
        foreach (ShakenObject shakenObj in shakenObjects)
        {
            shakenObj.TriggerShake(defaultShakeDuraction);
        }
    }
    public void TriggerShake(float duration, float magnitute)
    {
        foreach (ShakenObject shakenObj in shakenObjects)
        {
            shakenObj.shakeMagnitude = magnitute;
            shakenObj.TriggerShake(defaultShakeDuraction);
        }
    }
}
