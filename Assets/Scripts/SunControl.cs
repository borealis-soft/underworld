using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SunControl : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private Light[] lights;
    private bool doneNight = true;
    public static SunControl instance;
    [HideInInspector]
    public bool IsDay 
    { 
        get => transform.eulerAngles.x >= 0 && transform.eulerAngles.x <= 90;
    }

    void Start()
    {
        instance = this;
        lights = GameObject.FindGameObjectsWithTag("Light").Select(obj => obj.GetComponent<Light>()).ToArray();
    }

    private void NightScript()
    {
        foreach (var light in lights)
            light.enabled = !light.enabled;
        doneNight = !doneNight;
    }

    void Update()
    {
        transform.Rotate(Vector3.right, Time.deltaTime * speed);
        if (doneNight == IsDay)
            NightScript();
    }
}