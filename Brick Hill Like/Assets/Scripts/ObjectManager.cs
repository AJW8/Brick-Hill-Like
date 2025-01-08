using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour {

	[SerializeField] private int inventoryMaxCapacity;
	[SerializeField] private GameObject groupPrefab;

	private GameObject[] worldObjects; // the objects currently in the level
	private GameObject[] worldGroups; // the groups currently in the level
	private GameObject[] inventoryObjects; // the objects currently in the inventory

	// Use this for initialization
	void Start ()
	{
		worldObjects = new GameObject[0];
		worldGroups = new GameObject[0];
		inventoryObjects = new GameObject[0];
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public GameObject GetHighestOrderObject(GameObject o)
	{
		if (o == null) return null;
		GameObject parent = o;
		GameObject previousParent = null;
		do
		{
			previousParent = parent;
			parent = parent.transform.parent.gameObject;
		}
		while (parent != null);
		return previousParent;
	}

	public void AddObjectToWorld(GameObject o)
	{
		List<GameObject> list = new List<GameObject> (worldObjects);
		list.Add (o);
		worldObjects = list.ToArray ();
	}

	public void CreateGroup(GameObject[] objectsToGroup)
	{
		Vector3 groupPosition = Vector3.zero;
		foreach (GameObject o in objectsToGroup) groupPosition += o.transform.position / objectsToGroup.Length;
		GameObject group = Instantiate (groupPrefab, groupPosition, Quaternion.identity);
		foreach (GameObject o in objectsToGroup) GetHighestOrderObject (o).transform.parent = group.transform;
		List<GameObject> list = new List<GameObject> (worldGroups);
		list.Add (group);
		worldGroups = list.ToArray ();
	}

	public void RemoveGroup(GameObject o)
	{
		GameObject group = GetHighestOrderObject (o);
		group.transform.DetachChildren();
		List<GameObject> list = new List<GameObject> (worldGroups);
		list.Remove (group);
		worldGroups = list.ToArray ();
		Destroy (group);
	}

	public void AddObjectToInventory(GameObject o)
	{
		GameObject copy = Instantiate (o, Vector3.zero, Quaternion.identity);
		List<GameObject> list = new List<GameObject> (inventoryObjects);
		list.Add (copy);
		inventoryObjects = list.ToArray ();
		copy.SetActive (false);
	}

	public GameObject GetObjectFromInventory(int index)
	{
		GameObject o = Instantiate (inventoryObjects[index], Vector3.zero, Quaternion.identity);
		o.SetActive (true);
		return o;
	}

	public void RemoveObjectFromInventory(int index)
	{
		List<GameObject> list = new List<GameObject> (inventoryObjects);
		list.RemoveAt (index);
		inventoryObjects = list.ToArray ();
	}
}
