using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MonsterSelectorWindow : EditorWindow
{
    private MonsterType _selectedMonsterType = MonsterType.None;

    [MenuItem("Window/Monster Selector")]
    public static void ShowWindow()
    {
        GetWindow<MonsterSelectorWindow>("Monster Selector");
    }

    private void OnGUI()
    {
        //window code goes here
        EditorGUILayout.Space(10);
        GUILayout.Label("Selection Filters:", EditorStyles.boldLabel);
        _selectedMonsterType = (MonsterType)EditorGUILayout.EnumPopup("MonsterType to select:",_selectedMonsterType);

        EditorGUILayout.Space(5);
        bool isClicked = GUILayout.Button("Select all");
        if (isClicked)
        {
            SelectAllMonsters();
        }

        
    }
    private void SelectAllMonsters()
    {
        //collect all the monster in our scene
        Monster[] monsters = FindObjectsOfType<Monster>();
        
        //create a temporary list to store valid monsters as we check
        List<GameObject> finalSelection = new List<GameObject>();

        //check each monster, store if type matches
        foreach (Monster monster in monsters)
        {
            if (monster.ThisType == _selectedMonsterType)
            {
                finalSelection.Add(monster.gameObject);
            }
        }
        Selection.objects = finalSelection.ToArray();
        //create a selection for valid monster
    }

}
