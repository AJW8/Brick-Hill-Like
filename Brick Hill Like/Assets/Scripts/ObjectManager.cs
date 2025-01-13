using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    [SerializeField] private int inventoryMaxCapacity; // Max capacity of inventory.
    [SerializeField] private GameObject buttonCancelGroup, buttonFinishGroup, buttonUngroup, panelCreatingGroup;
    [SerializeField] private GameObject groupPrefab; // Prefab used for creating groups.

    private GameObject[] worldObjects; // Objects in the world.
    private GameObject[] inventoryObjects; // Objects stored in inventory.
    private GameObject[] objectsToGroup; // Objects selected to form a group.

    void Start()
    {
        // Initialize empty object arrays and set the ObjectManager in SelectableBlock.
        worldObjects = new GameObject[0];
        inventoryObjects = new GameObject[0];
        SelectableBlock.SetObjectManager(this);
    }

    // Selects a block and updates its state.
    public void SelectBlock(SelectableBlock block)
    {
        if (block == null) return;

        // Deselect the current block if another is selected.
        if (SelectableBlock.SelectedBlock != null && SelectableBlock.SelectedBlock != block)
        {
            DeselectBlock(SelectableBlock.SelectedBlock);
        }

        // Highlight the new block and mark it as selected.
        SelectableBlock.SelectedBlock = block;
        block.Highlight();

        // Show or hide the ungroup button based on whether this block is a group.
        buttonUngroup?.SetActive(block.IsGroup());
    }

    // Deselects a block and restores its initial state.
    public void DeselectBlock(SelectableBlock block)
    {
        if (block == null) return;

        // Find the top-most parent (root) block for deselection.
        SelectableBlock highestOrderBlock = GetHighestOrderBlock(block);

        // Ensure it's the same block before deselecting.
        if (SelectableBlock.SelectedBlock == highestOrderBlock)
        {
            highestOrderBlock.Deselect();
            SelectableBlock.SelectedBlock = null;
        }
    }

    // Returns the top-most parent of a block.
    public SelectableBlock GetHighestOrderBlock(SelectableBlock block)
    {
        if (block == null) return null;

        // Navigate up the hierarchy to find the highest block.
        Transform parent = block.transform;
        while (parent.parent != null && parent.parent.GetComponent<SelectableBlock>() != null)
        {
            parent = parent.parent;
        }

        return parent.GetComponent<SelectableBlock>();
    }

    // Adds an object to the world.
    public void AddObjectToWorld(GameObject obj)
    {
        List<GameObject> list = new List<GameObject>(worldObjects);
        list.Add(obj);
        worldObjects = list.ToArray();
    }

    // Starts a new group (automatically adding the currently selected object if any).
    public void StartGroup()
    {
        objectsToGroup = new GameObject[0];
		if (SelectableBlock.SelectedBlock != null) AddObjectToGroup (SelectableBlock.SelectedBlock.gameObject);
    }

    // Adds an object to the current group selection.
    public void AddObjectToGroup(GameObject obj)
    {
        // Initialize group if needed.
        if (objectsToGroup == null)
        {
            objectsToGroup = new GameObject[] { obj };
        }
        else
        {
            List<GameObject> list = new List<GameObject>(objectsToGroup);
            if (!list.Contains(obj)) list.Add(obj);
            objectsToGroup = list.ToArray();
        }

        // Update UI state based on group size.
        bool hasEnoughObjects = objectsToGroup.Length >= 2;
        buttonCancelGroup.SetActive(!hasEnoughObjects);
        buttonFinishGroup.SetActive(hasEnoughObjects);
    }

    // Removes an object from the current group selection.
    public void RemoveObjectFromGroup(GameObject obj)
    {
        if (objectsToGroup == null) return;

        List<GameObject> list = new List<GameObject>(objectsToGroup);
        if (list.Contains(obj)) list.Remove(obj);
        objectsToGroup = list.ToArray();

        // Update UI state based on group size.
        bool hasEnoughObjects = objectsToGroup.Length >= 2;
        buttonCancelGroup.SetActive(!hasEnoughObjects);
        buttonFinishGroup.SetActive(hasEnoughObjects);
    }

    // Creates a new group of selected objects.
    public void CreateGroup()
    {
        if (objectsToGroup == null || objectsToGroup.Length < 2)
        {
            objectsToGroup = null;
            return;
        }

        // Determine the group's center position based on its objects.
        Vector3 groupPosition = Vector3.zero;
        foreach (GameObject obj in objectsToGroup)
        {
            groupPosition += obj.transform.position / objectsToGroup.Length;
        }

        // Instantiate the group and add each object as a child.
        GameObject group = Instantiate(groupPrefab, groupPosition, Quaternion.identity);
        foreach (GameObject obj in objectsToGroup)
        {
            obj.transform.parent = group.transform;
        }

        // Add the group to the world and clear the current selection.
        List<GameObject> list = new List<GameObject>(worldObjects);
        list.Add(group);
        worldObjects = list.ToArray();

        objectsToGroup = null;
        group.GetComponent<SelectableBlock>().Deselect();
    }

    // Ungroups a selected group by releasing its objects.
    public void RemoveGroup()
    {
		if (SelectableBlock.SelectedBlock == null) return;
        SelectableBlock block = GetHighestOrderBlock(SelectableBlock.SelectedBlock);
        if (block == null) return;

        block.Deselect();
        GameObject group = block.gameObject;

        // Detach all child objects.
        foreach (Transform child in group.transform)
        {
            child.parent = null;
        }

        // Remove the group from the world and destroy it.
        List<GameObject> list = new List<GameObject>(worldObjects);
        list.Remove(group);
        worldObjects = list.ToArray();

        Destroy(group);
    }

    // Adds a copy of an object to the inventory.
    public void AddObjectToInventory(GameObject obj)
    {
        GameObject copy = Instantiate(obj, Vector3.zero, Quaternion.identity);
        copy.SetActive(false);

        List<GameObject> list = new List<GameObject>(inventoryObjects);
        list.Add(copy);
        inventoryObjects = list.ToArray();
    }

    // Retrieves an object from the inventory and activates it.
    public GameObject GetObjectFromInventory(int index)
    {
        GameObject obj = Instantiate(inventoryObjects[index], Vector3.zero, Quaternion.identity);
        obj.SetActive(true);
        return obj;
    }

    // Removes an object from the inventory by index.
    public void RemoveObjectFromInventory(int index)
    {
        List<GameObject> list = new List<GameObject>(inventoryObjects);
        list.RemoveAt(index);
        inventoryObjects = list.ToArray();
    }

    // Returns true if grouping is currently active.
    public bool GetGrouping()
    {
        return objectsToGroup != null;
    }
}