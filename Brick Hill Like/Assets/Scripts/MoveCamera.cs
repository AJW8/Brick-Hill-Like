using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{

	float rotationX = 0f;
	float rotationY = 0f;

	float sensitivity = 10f;

	public float speed = 5f;

	// Update is called once per frame
	void Update()
	{
		if (Input.GetMouseButton(1))
		{
			rotationY += Input.GetAxis("Mouse X") * sensitivity;
			rotationX += Input.GetAxis("Mouse Y") * -1 * sensitivity;
			transform.localEulerAngles = new Vector3(rotationX, rotationY, 0);
		}

		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");

		Vector3 forward = transform.forward;
		Vector3 right = transform.right;

		Vector3 moveDirection = (forward * v + right * h).normalized;
		transform.position += moveDirection * speed * Time.deltaTime;
	}
}
