using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(SeparatorAttribute))]
public class SeparatorDrawer : DecoratorDrawer
{
    public override void OnGUI(Rect position)
    {
        //get a refference to the attribute
        SeparatorAttribute separatorAttribute = attribute as SeparatorAttribute;

        Rect separatorRect = new Rect(position.xMin,position.yMin+separatorAttribute.Spacing,position.width,separatorAttribute.Height);

        EditorGUI.DrawRect(separatorRect,Color.white);
    }

    public override float GetHeight()
    {
        SeparatorAttribute separatorAttribute = attribute as SeparatorAttribute;
        float totalSpacing = separatorAttribute.Spacing*2 + separatorAttribute.Height;

        return totalSpacing;
    }
}
