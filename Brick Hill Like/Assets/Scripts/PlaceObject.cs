using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceObject : MonoBehaviour
{
	public GameObject objectToSpawn;

	public void OnClick()
	{
		Instantiate(objectToSpawn, Vector3.zero, Quaternion.identity);
	}
}
