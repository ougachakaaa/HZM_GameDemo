using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllList : MonoBehaviour
{
    public Transform panelsHolderTransform;
    public static Transform backGroundBlocker;
    public Button thisButton;
    public NavPanel navPanel;
    public List<GameObject> controllPanels;
    public List<GameObject> activePanels;

    private void Awake()
    {
        panelsHolderTransform = GameObject.Find("Panels").transform;
        backGroundBlocker = GameObject.Find("BackgroundBlocker").transform;
        thisButton = GetComponent<Button>();
        navPanel = GetComponentInParent<NavPanel>();

        thisButton.onClick.AddListener(()=> {
            navPanel.HideAllPanels(this);
            ShowPanels();
        });
    }


    
    public void ShowPanels()
    {
        foreach (GameObject _panel in controllPanels)
        {

            if (activePanels.Count < controllPanels.Count)
            {
                activePanels.Add(GameObject.Instantiate(_panel, panelsHolderTransform));
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
        HideBackground();
        foreach (GameObject _panel in activePanels)
        {
            _panel.SetActive(false);
        }
    }

    public void HideBackground()
    {
        if (backGroundBlocker != null && !backGroundBlocker.gameObject.activeSelf)
        {
            backGroundBlocker.gameObject.SetActive(true);
        }
    }

}
