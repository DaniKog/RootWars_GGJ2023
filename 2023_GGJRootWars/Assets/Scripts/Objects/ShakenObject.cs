using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakenObject : MonoBehaviour
{
    // Start is called before the first frame update

    // Transform of the GameObject you want to shake
    private Transform transform;

    // Desired duration of the shake effect
    private float shakeDuration = 0f;

    // A measure of magnitude for the shake. Tweak based on your preference
    public float shakeMagnitude = 1f;

    // A measure of how quickly the shake effect should evaporate
    private float dampingSpeed = 1.0f;

    // The initial position of the GameObject
    Vector3 initialPosition;
    // Start is called before the first frame update
    void Start()
    {
        if (transform == null)
        {
            transform = GetComponent(typeof(Transform)) as Transform;
        }
        initialPosition = transform.localPosition;
    }

    public void TriggerShake(float duration)
    {
        shakeDuration = duration;
    }

    // Update is called once per frame
    void Update()
    {
        if (shakeDuration > 0)
        {
            transform.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;

            shakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            shakeDuration = 0f;
            transform.localPosition = initialPosition;
        }
    }
}
