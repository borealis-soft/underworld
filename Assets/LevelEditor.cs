using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class LevelEditor : MonoBehaviour
{
    [HideInInspector] 
    public GameObject Parent;
    public GameObject CellPrefab;
    public bool isNetworkCell;
    public GameObject L_RoadPref;
    public GameObject T_RoadPref;
    public GameObject X_RoadPref;
    public GameObject RoadPref;
    public int Width = 15, Length = 15;
    public Texture2D PixelMap;
    public Towers[] buildDefenceTowers;
    public Towers[] buildUnitTowers;

    public float RadiantColorG = 128;
    public Color DireColor = Color.green;
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
            if (editor.Parent)
                DestroyImmediate(editor.Parent);
            editor.Parent = new GameObject();
            editor.Parent.name = "Tiles";

            Vector3 InstPos = new Vector3(0.5f, 0, 0.5f);
            for (int y = 0; y < editor.Length; y++)
            {
                for (int x = 0; x < editor.Width; x++, InstPos.x++)
                {
                    Color pixelColor = editor.PixelMap.GetPixel(x, y);

                    if (pixelColor ==  Color.blue)
                        continue;
                    GameObject newObj;
                    float quatY = 0;
                    float quatX = 0;
                    if (pixelColor == Color.black)
                    {
                        quatX = -90;
                        int roadDirection = RoadOnPexels(new Color[]{
                            editor.PixelMap.GetPixel(x + 1, y),// 1 - справа
							editor.PixelMap.GetPixel(x - 1, y),// 2 - слева
							editor.PixelMap.GetPixel(x, y + 1),// 4 - сверху
							editor.PixelMap.GetPixel(x, y - 1),// 8 - снизу
						});

                        if (roadDirection == 15)
                            newObj = editor.X_RoadPref;
                        else if (roadDirection == 3 || roadDirection == 12)
                            newObj = editor.RoadPref;
                        else if (roadDirection == 5 || roadDirection == 6 || roadDirection == 9 || roadDirection == 10)
                            newObj = editor.L_RoadPref;
                        else
                            newObj = editor.T_RoadPref;

                        switch (roadDirection)
                        {
                            case 3:// справа слева
                                quatY = Random.Range(0, 2) == 0 ? 0 : 180;
                                break;
                            case 5:// справа сверху
                            case 13: // Т вправо
                                quatY = 90;
                                break;
                            case 6:// слева сверху
                            case 7: // Т вверх
                            case 15: // Перекрёсток
                                quatY = 0;
                                break;
                            case 9:// справа снизу
                            case 11: // T вниз
                                quatY = 180;
                                break;
                            case 14: // Т влево
                            case 10:// слева снизу
                                quatY = -90;
                                break;
                            case 12:// сверху снизу
                                quatY = Random.Range(0, 2) == 0 ? 90 : -90;
                                break;
                        }
                    }
                    else
                        newObj = editor.CellPrefab;
                    var obj = Instantiate(newObj, InstPos, Quaternion.Euler(quatX, quatY, 0), editor.Parent.transform);
                    if (editor.isNetworkCell && pixelColor != Color.black && pixelColor != Color.blue) {
                        var networkCell = obj.GetComponent<NetworkCell>();
                        float pixelColorG = Mathf.Round(pixelColor.g * 255);
                        float DireColorG = Mathf.Round(editor.DireColor.g * 255);

                        if (pixelColorG == editor.RadiantColorG || pixelColorG == editor.RadiantColorG - 25)
                        {
                            networkCell.side = Side.Radiant;
                        }
                        else if (pixelColor == editor.DireColor || pixelColorG == DireColorG - 25)
                        {
                            networkCell.side = Side.Dire;
                        }

                        if (pixelColorG == editor.RadiantColorG - 25 || pixelColorG == DireColorG - 25)
                        {
                            networkCell.buildTowers = editor.buildUnitTowers;
                        }
                        else
                        {
                            networkCell.buildTowers = editor.buildDefenceTowers;
                        }
                    }
                }
                InstPos.z += 1;
                InstPos.x = 0.5f;
            }
        }

        if (GUILayout.Button("Delete Map"))
        {
            if (++DeleteCounter == 2)
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

    private int RoadOnPexels(Color[] colors)
    {
        int result = 0;
        for (int i = 0; i < colors.Length; i++)
            if (colors[i] == Color.black)
                result += 1 << i;
        return result;
    }
}