using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class LevelEditor : MonoBehaviour
{
	[HideInInspector] public GameObject Parent;
	public GameObject CellPrefab;
	public GameObject RoadPrefab;
	public int Width = 15, Length = 15;
	public Texture2D PixelMap;
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}
}

[CustomEditor(typeof(LevelEditor))]
public class LevelEditorEditor : Editor
{
	int DeleteCounter = 0;

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		var editor = target as LevelEditor;

		if (GUILayout.Button("Generate"))
		{
			if (!editor.Parent)
			{
				editor.Parent = new GameObject();
				editor.Parent.name = "Tiles";
			}



			Vector3 InstPos = new Vector3(0.5f, 0, 0.5f);
			for (int y = 0; y < editor.Length; y++)
			{
				for (int x = 0; x < editor.Width; x++, InstPos.x += 1)
				{
					Color pixelColor = editor.PixelMap.GetPixel(x, y);
					if (pixelColor != Color.blue)
					{
						GameObject newObj = pixelColor == Color.black ? editor.RoadPrefab : editor.CellPrefab;
						Instantiate(newObj, InstPos, Quaternion.Euler(pixelColor == Color.black ? -90 : 0, 0, 0), editor.Parent.transform);
					}
				}
				InstPos.z += 1;
				InstPos.x = 0.5f;
			}
		}
		if (GUILayout.Button("Delete Map"))
		{
			if (++DeleteCounter == 6)
			{
				DeleteCounter = 0;
				if (editor.Parent)
				{
					DestroyImmediate(editor.Parent);
				}
			}
			Debug.Log(DeleteCounter);
		}
	}
}