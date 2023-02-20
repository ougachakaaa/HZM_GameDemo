using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllList : MonoBehaviour
{
    Transform panelsHolderTransform;
    public Transform backGroundBlocker;
    Button thisButton;
    public NavPanel navPanel;
    public List<GameObject> controllPanels;
    public List<GameObject> activePanels;

    private void Awake()
    {
        panelsHolderTransform = GameObject.Find("Panels").transform;
        //backGroundBlocker = GameObject.Find("BackgroundBlocker").transform;
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
                GameObject p = Instantiate(_panel, panelsHolderTransform);
                p.name = _panel.name;
                activePanels.Add(p);
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
