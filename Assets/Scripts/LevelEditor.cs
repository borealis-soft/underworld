using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class LevelEditor : MonoBehaviour
{
	[HideInInspector] public GameObject Parent;
	public GameObject cellPrefab;
	public int Width = 15, Length = 15;
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
			for (int i = 0; i < editor.Width; i++)
			{
				for (int j = 0; j < editor.Length; j++)
				{
					Instantiate(editor.cellPrefab, InstPos, Quaternion.identity, editor.Parent.transform);
					InstPos.x += 1;
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