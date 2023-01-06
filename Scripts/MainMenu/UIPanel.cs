using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanel : MonoBehaviour
{
    public ControllList[] buttonControllLists;
    // Start is called before the first frame update
    private void Awake()
    {
        buttonControllLists = GetComponentsInChildren<ControllList>();
    }

    public void HideAllPanels(ControllList exception)
    {
        foreach (ControllList ctrlList in buttonControllLists)
        {
            if (!ctrlList.Equals(exception))
            {
                ctrlList.HidePanels();
            }
        }
    }

}
