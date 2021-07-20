using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyBullet : MonoBehaviour
{
    public float Speed, Damage;
    public Transform Target;
    void Start()
    {
        Destroy(gameObject, 3);
    }

    void Update()
    {
        float deltaSpeed = Speed * Time.deltaTime;
        if (Target)
        {
            transform.LookAt(Target);
            transform.position = Vector3.MoveTowards(transform.position, Target.position, deltaSpeed);// Говно переделать
        }
        else
            transform.Translate(Vector3.forward * deltaSpeed);
    }
}
