using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FallAnimation : MonoBehaviour {

    // Use this for initialization
    [HideInInspector]
    public GameObject container, pitcher;
    public GameObject[] animImages;
    public bool isRunning;
    public const float DURATION=0.1f;
    int counter;
    public bool reverse;
    
    public void Start()
    {
        //StartAnimation();
        InvokeRepeating("MyUpdate", DURATION, DURATION);
        counter = 0;
        
    }
    void CalibrateHeight()
    {
        RectTransform imageTransform = animImages[0].GetComponent<RectTransform>();
        RectTransform glassTransform = GameManager.Instance.currentContainer.gameObject.GetComponent<RectTransform>();


 //       GameManager.Instance.SetTest(imageTransform.position);

        Vector3[] v = new Vector3[4];
        imageTransform.GetWorldCorners(v);
        float oldImageLength = imageTransform.position.y - v[3].y;
//        GameManager.Instance.SetTest((v[3]));

        glassTransform.GetWorldCorners(v);

        float glassLength = Mathf.Abs(v[0].y - v[2].y);//glassTransform.position.y - v[2].y;
        float glassHeight = v[0].y+ (1 -GameManager.Instance.currentContainer.actualHeightRatio)* glassLength;

        float newLength = (imageTransform.position.y) - glassHeight;
        float ratio = newLength / oldImageLength;
        foreach(GameObject go in animImages)
        {
            go.GetComponent<RectTransform>().localScale = new Vector3(1f, ratio, 1f);
        }
  //      GameManager.Instance.SetTest(new Vector3(glassTransform.position.x,glassHeight,glassTransform.position.z));
        //GameManager.Instance.SetTest(glassTransform.position);
        //GameManager.Instance.SetTest(v[0]);
    }
    public void StartAnimation()
    {
        foreach (GameObject go in animImages)
        {
            go.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
        }
        CalibrateHeight();
        isRunning = true;
    }

    public void StopAnimation()
    {
        
        for (int i = 0; i < animImages.Length; i++)
        {
            animImages[i].SetActive(false);
        }
        isRunning = false;
        
    }
    public void MyUpdate()
    {
        if (isRunning)
        {
            
            counter = (counter == animImages.Length - 1) ? 0 : counter + 1;
            for (int i = 0; i < animImages.Length; i++)
            {
                if (counter == i)
                {
                    animImages[i].SetActive(true);
                }
                else
                {
                    animImages[i].SetActive(false);
                }
            }
        }
    }
}
