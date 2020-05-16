using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour {

    public static TutorialManager Instance { get { return instance; } }
    private static TutorialManager instance;

    private const int NO_OF_PANELS = 3;

    public GameObject tutorialPanel, baseText;
    private GameObject holderPanel, curActivePanel;
    private List<GameObject> panelList = new List<GameObject>();

    private bool tutorialRunning;

    private int curPanel;

    void Start() {
        if (instance == null)
        {
            instance = this;

        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Update()
    {
        if(tutorialRunning)
        {
            if(Input.GetMouseButtonDown(0))
            {
                
                curActivePanel.SetActive(false);
                curPanel++;
                if (curPanel == NO_OF_PANELS)
                {
                    tutorialPanel.SetActive(false);
                    UIController.Instance.TutorialOver();
                    tutorialRunning = false;
                    return;
                }
                baseText.GetComponent<Text>().text = (curPanel + 1).ToString() + "/" + NO_OF_PANELS.ToString();
                curActivePanel = panelList[curPanel];
                curActivePanel.SetActive(true);
            }
        }
    }

    public void StartTutorial()
    {
        tutorialPanel.SetActive(true);
        curPanel = 0;
        baseText.GetComponent<Text>().text = "1/" + NO_OF_PANELS.ToString();
        holderPanel = tutorialPanel.transform.GetChild(0).gameObject;
        foreach (Transform t in holderPanel.transform)
        {
            panelList.Add(t.gameObject);
        }
        curActivePanel = panelList[0];
        curActivePanel.SetActive(true);
        tutorialRunning = true;
    }

}
