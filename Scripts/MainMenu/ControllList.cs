using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllList : MonoBehaviour
{
    public Transform canvasTransform;
    public Button thisButton;
    public UIPanel thisPanel;
    public List<GameObject> controllPanels;
    public List<GameObject> activePanels;

    private void Awake()
    {
        canvasTransform = GameObject.Find("Panels").transform;
        thisButton = GetComponent<Button>();
        thisPanel = GetComponentInParent<UIPanel>();

        thisButton.onClick.AddListener(()=> {
            thisPanel.HideAllPanels(this);
            ShowPanels();
        });
    }


    
    public void ShowPanels()
    {
        foreach (GameObject _panel in controllPanels)
        {

            if (activePanels.Count < controllPanels.Count)
            {
                activePanels.Add(GameObject.Instantiate(_panel, canvasTransform));
            }
            else
            {
                foreach (GameObject _activePanel in activePanels)
                {
                    _activePanel.SetActive(true);
                }
            }
        }
    }
    public void HidePanels()
    {
        foreach (GameObject _panel in activePanels)
        {
            _panel.SetActive(false);
        }
    }

}
