using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapManager))]
public class MapEditor : Editor
{
    private SerializedProperty _alwayUpdate;

    private void OnEnable()
    {
        _alwayUpdate = serializedObject.FindProperty("_alwayUpdate");
    }

    public override void OnInspectorGUI()
    {
        //serializedObject.UpdateIfRequiredOrScript();

        base.OnInspectorGUI();
        MapManager mapManager = target as MapManager;

        GUILayout.Space(10);

        EditorGUILayout.PropertyField(_alwayUpdate);
        GUILayout.BeginHorizontal();
        bool generateMap = GUILayout.Button("Genrate Map");
        //GUILayout.Toggle(false,"Alway Update");
        if ((_alwayUpdate.boolValue) || generateMap)
        {
            mapManager.InitializeMapManager(0);
            mapManager.GenerateMap();
        }
        GUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
    }

}
