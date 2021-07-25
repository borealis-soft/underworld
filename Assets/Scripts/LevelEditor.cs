using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class LevelEditor : MonoBehaviour
{
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
    public GameObject cellPrefab;
    public int Width = 15, Length = 15;

    public override void OnInspectorGUI()
	{
        base.OnInspectorGUI();

        var editor = target as LevelEditor;

        if (GUILayout.Button("Generate"))
		{
            Vector3 InstPos = Vector3.zero;
            for (int i = 0; i < Width; i++)
			{
                for (int j = 0; j < Length; j++)
				{
                    Instantiate(cellPrefab, InstPos, Quaternion.identity);
                    InstPos += Vector3.right;
				}
                InstPos += Vector3.forward;
			}
		}
    }
}