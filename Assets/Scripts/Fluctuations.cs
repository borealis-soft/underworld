using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fluctuations : MonoBehaviour
{
    [SerializeField, Range(0, 1)]
    float range = 0.2f;

    [SerializeField, Range(1, 3)]
    float speed = 1f;

    private Vector3 startPosition;
    void Awake()
    {
        startPosition = transform.position;
    }
    void Update()
    {
        transform.position = startPosition + Vector3.up * Mathf.Sin(Time.time * speed) * range;
    }
}
