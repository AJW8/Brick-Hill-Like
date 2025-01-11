using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour {

	[SerializeField] private int inventoryMaxCapacity;
	[SerializeField] private GameObject buttonNewGroup, buttonCancelGroup, buttonFinishGroup, buttonUngroup, panelCreatingGroup;
	[SerializeField] private GameObject groupPrefab;

	private GameObject[] worldObjects; // the objects currently in the level
	private GameObject[] inventoryObjects; // the objects currently in the inventory

	private GameObject[] objectsToGroup;

	// Use this for initialization
	void Start ()
	{
		SelectableBlock.SetObjectManager(this);
		worldObjects = new GameObject[0];
		inventoryObjects = new GameObject[0];
	}
	
	public void SelectBlock(SelectableBlock block)
	{
		SelectableBlock hob = GetHighestOrderBlock (block);
		if (objectsToGroup == null) buttonUngroup.SetActive (hob.IsGroup());
		else AddObjectToGroup (hob.gameObject);
	}

	public void DeselectBlock(SelectableBlock block)
	{
		if (objectsToGroup != null) RemoveObjectFromGroup(GetHighestOrderBlock (block).gameObject);
	}

	public SelectableBlock GetHighestOrderBlock(SelectableBlock block)
	{
		if (block == null) return null;
		Transform parent = block.gameObject.transform;
		Transform previousParent = null;
		do
		{
			previousParent = parent;
			parent = parent.parent;
		}
		while (parent != null);
		return previousParent.gameObject.GetComponent<SelectableBlock>();
	}

	public void AddObjectToWorld(GameObject o)
	{
		List<GameObject> list = new List<GameObject> (worldObjects);
		list.Add (o);
		worldObjects = list.ToArray ();
	}

	public void StartGroup()
	{
		objectsToGroup = new GameObject[0];
	}

	public void AddObjectToGroup(GameObject o)
	{
		if (objectsToGroup == null) objectsToGroup = new GameObject[]{ o };
		else
		{
			List<GameObject> list = new List<GameObject> (objectsToGroup);
			if (!list.Contains(o)) list.Add (o);
			objectsToGroup = list.ToArray ();
		}
		buttonCancelGroup.SetActive (objectsToGroup.Length < 2);
		buttonFinishGroup.SetActive (objectsToGroup.Length >= 2);
	}

	public void RemoveObjectFromGroup(GameObject o)
	{
		if (objectsToGroup == null) return;
		List<GameObject> list = new List<GameObject> (objectsToGroup);
		if (list.Contains(o)) list.Remove (o);
		objectsToGroup = list.ToArray ();
		buttonCancelGroup.SetActive (objectsToGroup.Length < 2);
		buttonFinishGroup.SetActive (objectsToGroup.Length >= 2);
	}

	public void CancelGroup()
	{
		if (objectsToGroup == null) return;
	}

	public void CreateGroup()
	{
		if (objectsToGroup == null) return;
		if (objectsToGroup.Length < 2)
		{
			objectsToGroup = null;
			return;
		}
		Vector3 groupPosition = Vector3.zero;
		foreach (GameObject o in objectsToGroup) groupPosition += o.transform.position / objectsToGroup.Length;
		GameObject group = Instantiate (groupPrefab, groupPosition, Quaternion.identity);
		foreach (GameObject o in objectsToGroup) o.transform.parent = group.transform;
		List<GameObject> list = new List<GameObject> (worldObjects);
		list.Add (group);
		worldObjects = list.ToArray ();
		objectsToGroup = null;
		group.GetComponent<SelectableBlock> ().Deselect ();
	}

	public void RemoveGroup()
	{
		SelectableBlock block = GetHighestOrderBlock (SelectableBlock.SelectedBlock);
		block.Deselect ();
		GameObject group = block.gameObject;
		//group.transform.DetachChildren();
		foreach (Transform child in group.transform) child.parent = null;
		List<GameObject> list = new List<GameObject> (worldObjects);
		list.Remove (group);
		worldObjects = list.ToArray ();
		Destroy (group);
		buttonUngroup.SetActive (false);
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

	public bool GetGrouping()
	{
		return objectsToGroup != null;
	}
}
