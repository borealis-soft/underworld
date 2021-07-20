using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour
{
    [SerializeField, Range(1f, 200f)]
    float speed = 10f;
    void Update()
    {
        transform.Rotate(Vector3.up, speed * Time.deltaTime);
    }
}
