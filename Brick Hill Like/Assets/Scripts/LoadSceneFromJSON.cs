using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadSceneFromJSON : MonoBehaviour
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
		public string colorHex; // Couleur en hexadécimal
	}

	[System.Serializable]
	public class SceneData
	{
		public List<BlockData> blocks = new List<BlockData>();
	}

	public List<BlockPrefab> blockPrefabs; // Liste des prefabs pour chaque type de bloc
	public string fileName = "sceneData.json";

	[System.Serializable]
	public class BlockPrefab
	{
		public string type;
		public GameObject prefab;
	}

	public void LoadScene()
	{
		string path = Path.Combine(Application.persistentDataPath, fileName);

		if (!File.Exists(path))
		{
			return;
		}

		string json = File.ReadAllText(path);
		SceneData sceneData = JsonUtility.FromJson<SceneData>(json);

		foreach (BlockData blockData in sceneData.blocks)
		{
			// Recherche du prefab pour le type de bloc
			GameObject prefab = blockPrefabs.Find(p => p.type == blockData.type)?.prefab;
			if (prefab == null)
			{
				Debug.LogWarning($"Prefab not found for type: {blockData.type}, skipping...");
				continue;
			}

			GameObject block = Instantiate(prefab, blockData.position, Quaternion.Euler(blockData.rotation));
			block.transform.localScale = blockData.scale;

			Renderer renderer = block.GetComponent<Renderer>();
			if (renderer != null)
			{
				Material material = GetMaterialByName(blockData.materialName);
				if (material == null)
				{
					material = new Material(Shader.Find("Standard"));
				}

				renderer.sharedMaterial = material;

				Color color;
				if (ColorUtility.TryParseHtmlString("#" + blockData.colorHex, out color))
				{
					renderer.sharedMaterial.color = color;
				}
			}
		}

		Debug.Log($"Scene loaded from {path}");
	}

	private Material GetMaterialByName(string materialName)
	{
		Material material = Resources.Load<Material>(materialName);

		if (material == null || materialName == "Default-Material")
		{
			return new Material(Shader.Find("Standard"));
		}

		return material;
	}
}
