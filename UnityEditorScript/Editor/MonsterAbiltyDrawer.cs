using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomPropertyDrawer(typeof(MonsterAbility))]
public class MonsterAbiltyDrawer : PropertyDrawer
{
    private SerializedProperty _name;
    private SerializedProperty _damage;
    private SerializedProperty _element;

    //how to draw to the inspector window
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position,label,property);

        _name = property.FindPropertyRelative("_name");
        _damage = property.FindPropertyRelative("_damage");
        _element = property.FindPropertyRelative("_elementType");
        //drawing instruction here
        Rect foldOutBox = new Rect(position.min.x,position.min.y,position.size.x,EditorGUIUtility.singleLineHeight);
        property.isExpanded = EditorGUI.Foldout(foldOutBox,property.isExpanded,label);
        if (property.isExpanded)
        {
            //draw out properties
            DrawNameProperty(position);
            DrawDamageProperty(position);
             
            DrawElementProperty(position);
        }

        EditorGUI.EndProperty();

     }

    private void DrawNameProperty(Rect position)
    {

        float xPos = position.min.x;
        float yPos = position.min.y + EditorGUIUtility.singleLineHeight;
        float width = position.size.x*0.4f;
        float height = EditorGUIUtility.singleLineHeight;

        EditorGUIUtility.labelWidth = 40;
        Rect drawArea = new Rect(xPos, yPos, width, height);
        EditorGUI.PropertyField(drawArea, _name, new GUIContent("Name"));
    }

    private void DrawDamageProperty(Rect position)
    {
        float xPos = position.min.x + position.width / 2;
        float yPos = position.min.y + EditorGUIUtility.singleLineHeight;
        float width = position.size.x * 0.4f;
        float height = EditorGUIUtility.singleLineHeight;

        EditorGUIUtility.labelWidth = 40;
        Rect drawArea = new Rect(xPos, yPos, width, height);
        EditorGUI.PropertyField(drawArea,_damage,new GUIContent("Damage"));
    }
    private void DrawElementProperty(Rect position)
    {
        float xPos = position.min.x + position.width*0f;
        float yPos = position.min.y + EditorGUIUtility.singleLineHeight*2;
        float width = position.size.x;
        float height = EditorGUIUtility.singleLineHeight;

        EditorGUIUtility.labelWidth = 70;
        Rect drawArea = new Rect(xPos, yPos, width, height);
        EditorGUI.PropertyField(drawArea, _element, new GUIContent("Element"));
    }


    //request more vertical space
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        int totalLines = 1;
        if (property.isExpanded)
        {
            totalLines += 2;
        }

        return EditorGUIUtility.singleLineHeight * totalLines;
    }
}

