using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {
    public static UIController Instance{ get { return instance; } }
    private static UIController instance;

    public Text compositionValText, levelText, goMessage, messageWindow, highScore;

    public GameObject deetsPanel, homePanel, gameElements, messageBox, soundOffObject, soundSystem, effectSystem, privacyPanel;

    public int soundState=1;
    private bool firstTimeOpeningApp;

    void Start()
    {
    //    PlayerPrefs.DeleteAll();
        if (instance == null)
        {
            instance = this;

        }
        else
        {
            Destroy(gameObject);
        }

        if(!PlayerPrefs.HasKey("privacyPolicy"))
        {
            privacyPanel.SetActive(true);
        }

        if(PlayerPrefs.HasKey("soundState"))
        {
            firstTimeOpeningApp = false;
            soundState = PlayerPrefs.GetInt("soundState");
            GetComponent<TutorialManager>().enabled = false;
        }
        else
        {
            firstTimeOpeningApp = true;
            PlayerPrefs.SetInt("soundState", soundState);
        }

        if (soundState==1)
        {
            soundSystem.SetActive(true);
            soundOffObject.SetActive(true);
        }
        else
        {
            soundSystem.SetActive(false);
            soundOffObject.SetActive(false);
        }
        effectSystem.GetComponent<AudioSource>().volume = 0f;
        UpdateHighScore(PlayerPrefs.GetInt("HighestScoreReached"));
    }

    public void Play()
    {
        if (firstTimeOpeningApp)
        {
            firstTimeOpeningApp = false;
            TutorialManager.Instance.StartTutorial();
        }
        else
        {
            homeToGame();
            GetComponent<GameManager>().StartGame();
        }
    }

    public void Restart()
    {
        GetComponent<GameManager>().StartGame();
        gameElements.GetComponent<Animation>().Play("gamesPanelAnimationHomeToGame");
        homePanel.GetComponent<Animation>().Play("restartGame");
    }

    void homeToGame()
    {
        deetsPanel.GetComponent<Animation>().Play("deetsAnimationHomeToGame");
        homePanel.GetComponent<Animation>().Play("homePanelAnimationHomeToGame");
        gameElements.GetComponent<Animation>().Play("gamesPanelAnimationHomeToGame");
    }

    public void gameToHome()
    {
        deetsPanel.GetComponent<Animation>().Play("deetsAnimationGameToHome");
        homePanel.GetComponent<Animation>().Play("homePanelAnimationGameToHome");
        gameElements.GetComponent<Animation>().Play("gamesPanelAnimationGameToHome");
    }

    public void PrivacyPolicy()
    {
        Application.OpenURL("https://esteev.github.io/privacy_policy_hnc.html");
    }

    public void UnityPrivacyPolicy()
    {
        Application.OpenURL("https://unity3d.com/legal/privacy-policy");
    }

    public void AcceptPrivacyPolicy()
    {
        PlayerPrefs.SetInt("privacyPolicy", soundState);
        privacyPanel.SetActive(false);
    }

    public void Back()
    {
     /*   if(homePanel.transform.Find("Start").gameObject.activeInHierarchy|| homePanel.transform.Find("Restart").gameObject.activeInHierarchy)
        {
            Application.Quit();
        }*/
    }

    public void MessagePopUp(string message)
    {
        messageWindow.text = message;
        messageBox.SetActive(true);
        
        messageBox.GetComponent<Animation>().Play("popUp");
        StartCoroutine(HoldIt(2f));                             //message duration
    }

    IEnumerator HoldIt(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        PopDown();
    }

    private void PopDown()
    {
        messageBox.GetComponent<Animation>().Play("popDown");
        Invoke("SetMessageBoxFalse", 1f);
    }

    void SetMessageBoxFalse()
    {
        messageBox.SetActive(false);
    }

    public void URLLinks()
    {
     //   switch(id)
        {
            Application.OpenURL("https://play.google.com/store/apps/dev?id=7152583459674782300");
      //      case 1: Application.OpenURL("https://play.google.com/store/apps/details?id=com.bizarreGameStudios.dodoVsDino"); break;
        //    case 2: Application.OpenURL("http://www.bizarregamestudios.com"); break;
       //     case 3: Application.OpenURL("https://www.facebook.com/bizarregamestudios"); break;
       //     case 4: Application.OpenURL("https://www.instagram.com/bizarregamestudios"); break;
     //       case 5: Application.OpenURL(""); break;
        }
    }

    public void LeaderBoard()
    {
 //       LeaderBoard leaderBoard = new LeaderBoard();
   //     leaderBoard.show();
    }

    public void SoundFunc()
    {
        if (soundState == 1)
        {
            soundState = 0;
            soundSystem.SetActive(false);
            soundOffObject.SetActive(false);
        }
        else
        {
            soundState = 1;
            soundSystem.SetActive(true);
            soundOffObject.SetActive(true);
        }
        PlayerPrefs.SetInt("soundState", soundState);
    }

    public void TutorialOver()
    {
        GetComponent<TutorialManager>().enabled = false;
        Play();
    }

    public void UpdateHighScore(int val)
    {
        highScore.text = val.ToString();
    }

    public void Help()
    {
        GetComponent<TutorialManager>().enabled = true;
        TutorialManager.Instance.StartTutorial();
    }

}
