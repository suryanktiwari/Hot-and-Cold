using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.

public class BackButton : MonoBehaviour, IPointerDownHandler// required interface when using the OnPointerDown method.
{

    public GameObject restartButton;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Exit();
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        Exit();
    }
    void Exit()
    {

        if (GameManager.Instance.hotPitcher.transform.parent.GetComponent<FallAnimation>().isRunning)
        {
            GameManager.Instance.hotPitcher.transform.parent.GetComponent<FallAnimation>().StopAnimation();
        }
        if (GameManager.Instance.coldPitcher.transform.parent.GetComponent<FallAnimation>().isRunning)
        {
            GameManager.Instance.coldPitcher.transform.parent.GetComponent<FallAnimation>().StopAnimation();
        }
        if (UIController.Instance.homePanel.transform.GetChild(0).gameObject.activeInHierarchy)
        {
            Application.Quit();
        }
        else
        {
            restartButton.SetActive(false);
            GameManager.Instance.isRunning = false;
            UIController.Instance.gameToHome();
            UIController.Instance.effectSystem.GetComponent<AudioSource>().volume = 0f;
        }
    }
}