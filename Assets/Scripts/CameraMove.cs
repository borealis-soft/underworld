using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
	public float Speed;
	public float ZoomSpeed;

	public KeyCode UpKey    = KeyCode.W;
	public KeyCode DownKey  = KeyCode.S;
	public KeyCode LeftKey  = KeyCode.A;
	public KeyCode RightKey = KeyCode.D;

	[SerializeField] private float minZoom = 0;
	[SerializeField] private float maxZoom = 5;
	[SerializeField] private float minX    = 0;
	[SerializeField] private float maxX    = 16;
	[SerializeField] private float minZ    = 0;
	[SerializeField] private float maxZ    = 23;

	private float currentZoom;
	private Vector3 topPosition;
	private Quaternion moveRotation;

	private void Start()
	{
		topPosition = transform.position;
		moveRotation = Quaternion.Euler(
			0,
			transform.eulerAngles.y,
			transform.eulerAngles.z
		);
	}
	void Update()
	{
		Vector3 moveDelta = default;
		if (Input.GetKey(UpKey)) moveDelta.z += Speed * Time.deltaTime;
		if (Input.GetKey(DownKey)) moveDelta.z -= Speed * Time.deltaTime;
		if (Input.GetKey(RightKey)) moveDelta.x += Speed * Time.deltaTime;
		if (Input.GetKey(LeftKey)) moveDelta.x -= Speed * Time.deltaTime;
		topPosition += moveRotation * moveDelta;
		topPosition.x = Mathf.Clamp(topPosition.x, minX, maxX);
		topPosition.z = Mathf.Clamp(topPosition.z, minZ, maxZ);

		currentZoom = Mathf.Clamp(currentZoom + Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed, minZoom, maxZoom);

		transform.position = topPosition + transform.forward * currentZoom;

		Debug.DrawLine(topPosition, transform.position, Color.red);
	}
}
