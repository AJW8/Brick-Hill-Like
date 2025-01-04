using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragSystem : MonoBehaviour
{
	public float gridSize = 0.5f;

	private Vector3 offset;
	private Vector3 mousePosition;
	private bool isDragging = false;

	private Vector3 GetMousePos()
	{
		return Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.WorldToScreenPoint(transform.position).z));
	}

	private void OnMouseDown()
	{
		mousePosition = GetMousePos();
		offset = transform.position - mousePosition;
		isDragging = true;
	}

	private void OnMouseDrag()
	{
		if (isDragging)
		{
			Vector3 newPos = GetMousePos() + offset;

			newPos.x = Mathf.Round(newPos.x / gridSize) * gridSize;
			newPos.y = Mathf.Round(newPos.y / gridSize) * gridSize;
			newPos.z = Mathf.Round(newPos.z / gridSize) * gridSize;

			if (!IsColliding(newPos))
			{
				transform.position = newPos;
			}
		}
	}

	private void OnMouseUp()
	{
		isDragging = false;
	}

	private bool IsColliding(Vector3 position)
	{
		Collider[] colliders = Physics.OverlapBox(position, transform.localScale / 2, Quaternion.identity);
		foreach (Collider collider in colliders)
		{
			if (collider.gameObject != gameObject)
			{
				return true;
			}
		}
		return false;
	}
}
