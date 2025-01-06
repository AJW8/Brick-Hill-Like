using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceObject : MonoBehaviour
{
	[SerializeField] private GameObject[] prefabs;

	private int objectToSpawn;

	public void OnClick()
	{
		Instantiate(prefabs[objectToSpawn], Vector3.zero, Quaternion.identity);
	}

	public void SetObjectToSpawn(int index)
	{
		objectToSpawn = index;
	}
}
