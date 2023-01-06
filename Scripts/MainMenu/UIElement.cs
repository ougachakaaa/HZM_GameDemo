using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class UIElement
{
    [Header("NavigationBar")]
    public static GameObject NavigationBar;

    [Header("Panels")]
    public static List<GameObject> Panels = new List<GameObject>();

}

[System.Serializable]
public class ButtonPanelPair
{
    public Transform canvasTransform;

    public Button button;

    public List<GameObject> panels;

    


    public void ShowPanel()
    {
        foreach (GameObject _panel in panels)
        {
            GameObject p = GameObject.Find(_panel.name);
            if (p == null)
            {
                p = GameObject.Instantiate(_panel , canvasTransform);
                p.name = _panel.name;
            }
            else
            {
                p.SetActive(true);
            }
        }
    }

    public void HidePanel()
    {
        foreach (GameObject _panel in panels)
        {
            GameObject p = GameObject.Find(_panel.name);
            if (p == null)
            {
                Debug.Log($"this panel{_panel.name} does not exit in current game");
            }
            else
            {
                p.SetActive(false);
            }
        }
    }
}
