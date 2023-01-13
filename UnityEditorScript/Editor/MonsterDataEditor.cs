using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Monster))]
public class MonsterDataEditor : Editor
{
    private SerializedProperty _age;
    private SerializedProperty _name;
    private SerializedProperty _health;
    private SerializedProperty _damage;
    private SerializedProperty _speed;
    private SerializedProperty _canEnterCombat;
    private SerializedProperty _abilities;



    private void OnEnable()
    {
        _age = serializedObject.FindProperty("_age");
        _name = serializedObject.FindProperty("_name");
        _health = serializedObject.FindProperty("_health");
        _damage = serializedObject.FindProperty("_damage");
        _speed = serializedObject.FindProperty("_speed");
        _canEnterCombat = serializedObject.FindProperty("_canEnterCombat");
        _abilities = serializedObject.FindProperty("_abilities");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.UpdateIfRequiredOrScript();
        float difficulty = _health.floatValue + _damage.floatValue + _speed.floatValue;
        ProgressBar(difficulty,"Difficulty");

        EditorGUILayout.LabelField(_name.stringValue.ToUpper(),EditorStyles.boldLabel);

        //baseGUI
        base.OnInspectorGUI();

        EditorGUILayout.Space(20);
        //custom GUI
        EditorGUILayout.LabelField("General Stats", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(_name,new GUIContent("Name"));
        EditorGUILayout.PropertyField(_age,new GUIContent("Age"));
        EditorGUILayout.PropertyField(_canEnterCombat,new GUIContent("Can Enter Combat"));

        _speed.floatValue = EditorGUILayout.Slider(_speed.floatValue,0,100);
        if (_canEnterCombat.boolValue == true)
        {
            EditorGUI.indentLevel ++;
            EditorGUILayout.BeginHorizontal();
            EditorGUIUtility.labelWidth = 50;
            EditorGUILayout.PropertyField(_health, new GUIContent("Health"));
            EditorGUILayout.PropertyField(_damage, new GUIContent("Damge"));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(_speed, new GUIContent("Speed"));
            bool clicked = GUILayout.Button("Random Speed");
            if (clicked)
            {

                RandomizeSpeed();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUI.indentLevel --;
        }
        EditorGUILayout.PropertyField(_abilities, new GUIContent("Abilities"));


        serializedObject.ApplyModifiedProperties();
    }

    void ProgressBar(float value , string label)
    {
        Rect rect = GUILayoutUtility.GetRect(40,18,"TextField");

        EditorGUI.ProgressBar(rect, value, label);

        //EditorGUILayout.Space(10);
    }
    void RandomizeSpeed()
    {
        _speed.floatValue = UnityEngine.Random.Range(0, 100);


        Debug.Log("Here we are");
    }
}
