using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopoutController : MonoBehaviour
{
    RectTransform panelHolderTransform;
    public RectTransform routinePanelHolder;

    public List<Button> buttonPrefabs;
    List<Button> buttons;
    List<Button> panelButtons;

    public List<GameObject> panelPrefabs;
    Dictionary<Button,GameObject> pairs;

    public float popoutTime;

    private void Awake()
    {
        //generate routinePanelsHolder
        panelHolderTransform = GameObject.Find("Panels").transform.GetComponent<RectTransform>();
        routinePanelHolder = new GameObject("RoutinePanelHolder").AddComponent<RectTransform>();
        routinePanelHolder.position = panelHolderTransform.position;
        routinePanelHolder.sizeDelta = new Vector2(Screen.width,Screen.height);
        routinePanelHolder.SetParent(transform as RectTransform);


        buttons = new List<Button>();
        panelButtons = new List<Button>();
        pairs = new Dictionary<Button, GameObject>();

        for (int i = 0; i < buttonPrefabs.Count; i++)
        {
            //generate button
            Button thisButton = Instantiate<Button>(buttonPrefabs[i], transform);
            RectTransform thisButtonTransform = thisButton.transform as RectTransform;
            thisButtonTransform.position = transform.position + i * new Vector3(120, 0, 0);
            Transform notificationTransform = thisButton.transform.Find("NotificationMark");


            //generate panel
            GameObject thisPanel = Instantiate<GameObject>(panelPrefabs[i], routinePanelHolder);
            TextMeshProUGUI title = thisPanel.transform.Find("RoutineTitleText").GetComponent<TextMeshProUGUI>();
            title.text = $"ROUTINE No.{i + 1}";

            thisButton.onClick.AddListener(() =>
            {
                thisPanel.SetActive(true);
                StartCoroutine(PopoutPanel(thisPanel, 1));

                if (notificationTransform.gameObject.activeSelf)
                    notificationTransform.gameObject.SetActive(false);
            });

            Button panelButton = thisPanel.transform.Find("ConfirmButton").GetComponent<Button>();
            panelButton.onClick.AddListener(() =>
            {
                thisPanel.SetActive(false);
                //StartCoroutine(PopoutPanel(thisPanel,-1));
            });

            buttons.Add(thisButton);
            panelButtons.Add(panelButton);
            pairs.Add(thisButton, thisPanel);
            pairs[buttons[i]].gameObject.SetActive(false);
        }
        routinePanelHolder.SetAsLastSibling();


    }
    IEnumerator PopoutPanel(GameObject _panel,int dir)
    {

        if (_panel != null)
        {
            Vector3 startScale;
            Vector3 endScale;
            if (dir < 0)
            {
                startScale = Vector3.one;
                endScale = Vector3.zero;
            }
            else
            {
                startScale = Vector3.zero;
                endScale = Vector3.one;
            }
            _panel.transform.localScale = startScale;

            float timer = 0f;
            while (timer<popoutTime)
            {
                timer += Time.deltaTime;
                _panel.transform.localScale = Vector3.Lerp(startScale,endScale,timer/popoutTime);
                yield return null;
            }
            if (dir < 0)
                _panel.SetActive(false);

            yield return null;
        }
    }
}

