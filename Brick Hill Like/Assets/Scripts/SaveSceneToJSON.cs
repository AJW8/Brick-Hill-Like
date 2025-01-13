using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveSceneToJSON : MonoBehaviour
{
	[System.Serializable]
	public class BlockData
	{
		public string type;
		public string name;
		public Vector3 position;
		public Vector3 rotation;
		public Vector3 scale;
		public string materialName;
		public string colorHex;
	}

	[System.Serializable]
	public class SceneData
	{
		public List<BlockData> blocks = new List<BlockData>();
	}

	public string fileName = "sceneData.json";

	public void SaveScene()
	{
		SceneData sceneData = new SceneData();

		GameObject[] blocks = GameObject.FindGameObjectsWithTag("Block");
		foreach (GameObject block in blocks)
		{
			string type = DetectBlockType(block);

			string colorHex = "FFFFFF";
			Renderer renderer = block.GetComponent<Renderer>();
			if (renderer != null)
			{
				Color color = renderer.sharedMaterial.color;
				colorHex = ColorUtility.ToHtmlStringRGB(color);
			}

			BlockData blockData = new BlockData
			{
				type = type,
				name = block.name,
				position = block.transform.position,
				rotation = block.transform.eulerAngles,
				scale = block.transform.localScale,
				materialName = block.GetComponent<Renderer>()?.sharedMaterial.name.Replace(" (Instance)", ""),
				colorHex = colorHex
			};

			sceneData.blocks.Add(blockData);
		}

		string json = JsonUtility.ToJson(sceneData, true);
		string path = Path.Combine(Application.persistentDataPath, fileName);
		File.WriteAllText(path, json);

		Debug.Log($"Scene saved to {path}");
	}

	private string DetectBlockType(GameObject block)
	{
		if (block.name.Contains("Rod")) return "rod";
		if (block.name.Contains("Plane")) return "plane";
		if (block.name.Contains("Brick")) return "brick";
		if (block.name.Contains("Triangle")) return "triangle";
		if (block.name.Contains("Pyramid")) return "pyramid";
		if (block.name.Contains("InversePyramid")) return "inverse_pyramid";

		return "unknown";
	}
}
