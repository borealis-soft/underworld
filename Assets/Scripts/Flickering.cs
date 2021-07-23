using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flickering : MonoBehaviour
{
    private new Light light;

    void Start()
    {
        light = GetComponent<Light>();
    }

    void FixedUpdate()
    {
        light.intensity = Mathf.Lerp(light.intensity, Random.Range(0, 10) != 0 ? 1 : 0, Time.deltaTime * 20f);
    }
}
