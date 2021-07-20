using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceletonSkill : MonoBehaviour
{
    public float SpeedBoost = 1;
    public float HPBoost = 100;
    private bool isAngry = false; 

    void Update()
    {
        if (SunControl.instance.IsDay == isAngry)
        {
            isAngry = !isAngry;
            GetComponent<Animator>().SetBool("isNight", !SunControl.instance.IsDay);
            GetComponent<Enemy>().Speed += SpeedBoost;
            GetComponent<Enemy>().HP += HPBoost;
            SpeedBoost *= -1;
            HPBoost *= -1;
        }
    }
}
