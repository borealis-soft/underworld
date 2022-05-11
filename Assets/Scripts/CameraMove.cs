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

    [SerializeField] private float minZoom = 0;
    [SerializeField] private float maxZoom = 5;
    [SerializeField] private float minX = 0;
    [SerializeField] private float maxX = 16;
    [SerializeField] private float minZ = 0;
    [SerializeField] private float maxZ = 23;
    [SerializeField] private float cameraOffset = 5;

    private float currentZoom;
    private Vector3 topPosition;
    private Quaternion moveRotation;
    private float lastLengthZoomLine;


    private void Start()
    {
        topPosition = transform.position;
        moveRotation = Quaternion.Euler(
            0,
            transform.eulerAngles.y,
            transform.eulerAngles.z
        );

        var rotate = Mathf.Abs(transform.rotation.eulerAngles.y);
        if (rotate == 180 || rotate == 0)
        {
            if (rotate == 180)
                cameraOffset = -cameraOffset;
            minZ -= cameraOffset;
            maxZ -= cameraOffset;
        }
        if (rotate == 90)
        {
            minX -= cameraOffset;
            maxX -= cameraOffset;
        }
    }
    void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector3 moveDelta = MoveOnPlatform();

        topPosition += moveRotation * moveDelta;
        topPosition.x = Mathf.Clamp(topPosition.x, minX, maxX);
        topPosition.z = Mathf.Clamp(topPosition.z, minZ, maxZ);

        transform.position = topPosition + transform.forward * currentZoom;

        //Debug.DrawLine(topPosition, transform.position, Color.red);
    }

    private Vector3 MoveOnPlatform()
    {
        Vector3 moveDelta = Vector3.zero;
        float scaleFactor = 0;

#if UNITY_STANDALONE
        if (Input.GetKey(UpKey)) moveDelta.z += Speed * Time.deltaTime;
        if (Input.GetKey(DownKey)) moveDelta.z -= Speed * Time.deltaTime;
        if (Input.GetKey(RightKey)) moveDelta.x += Speed * Time.deltaTime;
        if (Input.GetKey(LeftKey)) moveDelta.x -= Speed * Time.deltaTime;

        scaleFactor = Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed;
#endif
#if UNITY_ANDROID
        if (Input.touchCount == 1)
		{
			Touch userTap = Input.GetTouch(0);
			moveDelta.x = -userTap.deltaPosition.x / (Speed * (currentZoom + 1) * 10);
			moveDelta.z = -userTap.deltaPosition.y / (Speed * (currentZoom + 1) * 10);
		}
		if (Input.touchCount == 2)
        {
			Touch userTap0 = Input.GetTouch(0);
			Touch userTap1 = Input.GetTouch(1);
			float lengthZoomLine = Mathf.Sqrt(
				Mathf.Pow(userTap0.position.x - userTap1.position.x, 2) + 
				Mathf.Pow(userTap0.position.y - userTap1.position.y, 2));
			scaleFactor = -((lastLengthZoomLine - lengthZoomLine) / 10);
			lastLengthZoomLine = lengthZoomLine;
		}
#endif

        currentZoom = Mathf.Clamp(currentZoom + scaleFactor, minZoom, maxZoom);
        return moveDelta;
    }

}
