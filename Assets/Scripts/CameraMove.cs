using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public float Speed;
    public float ZoomSpeed;
    public KeyCode UpKey = KeyCode.W;
    public KeyCode DownKey = KeyCode.S;
    public KeyCode LeftKey = KeyCode.A;
    public KeyCode RightKey = KeyCode.D;

    private Vector3 lastPos;

    void Update()
    {
        if (Input.GetKey(UpKey))
            transform.Translate(Speed * Time.deltaTime * -transform.forward);
        if (Input.GetKey(DownKey))
            transform.Translate(Speed * Time.deltaTime * transform.forward);
        if (Input.GetKey(LeftKey))
            transform.Translate(Speed * Time.deltaTime * transform.right);
        if (Input.GetKey(RightKey))
            transform.Translate(Speed * Time.deltaTime * -transform.right);

        float scrollDelta = Input.GetAxis("Mouse ScrollWheel");
        if (scrollDelta > 0.1)
            transform.position += transform.forward * ZoomSpeed * Time.deltaTime;
        if (scrollDelta < -0.1)
            transform.position -= transform.forward * ZoomSpeed * Time.deltaTime;
    }
}
