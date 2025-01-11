using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;

public class PlaceObject : MonoBehaviour
{
	[SerializeField] private GameObject[] prefabs;
	[SerializeField] private ObjectManager objectManager;

	private int objectToSpawn;

	public void OnClick()
	{
		objectManager.AddObjectToWorld (Instantiate(prefabs[objectToSpawn], Vector3.zero, Quaternion.identity));
	}

	public void SetObjectToSpawn(int index)
	{
		objectToSpawn = index;
	}

	void Start()
	{
		/*
		GameObject o = Instantiate(prefabs[0], Vector3.up * 10, Quaternion.identity);
		MeshFilter filter = o.GetComponent<MeshFilter>();
		filter.mesh = new Mesh()
		{
			vertices = new Vector3[]{ 
				new Vector3(-0.5f, 0.5f, 0.5f),
				new Vector3(0.5f, 0.5f, 0.5f),
				new Vector3(-0.5f, 0.5f, -0.5f),
				new Vector3(-0.5f, -0.5f, 0.5f),
				new Vector3(0.5f, -0.5f, 0.5f),
				new Vector3(-0.5f, -0.5f, -0.5f),
				new Vector3(0.5f, -0.5f, -0.5f)
			},
			triangles = new int[]{ 
				0, 1, 6,
				0, 6, 2,
				1, 4, 6,
				2, 6, 5,
				0, 4, 1,
				0, 3, 4,
				0, 2, 3,
				2, 5, 3
			}
		};
		filter.mesh.RecalculateNormals();
		AssetDatabase.CreateAsset(filter.mesh, "Assets/Anti-pyramid.Asset");
		*/
	}
}
