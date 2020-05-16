using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pitcher : MonoBehaviour {
    
    [Range(0.0f, 1.0f)]
    private float val;

    public bool isHot;
    float ratio;
    float area;
    void Start () {
        RectTransform rectTransform = GetComponent<RectTransform>();
        area = rectTransform.sizeDelta.x * rectTransform.sizeDelta.y * 0.812f * 0.794f * rectTransform.localScale.x* rectTransform.localScale.y;
        SetVal(0f);
    }
	
	void Update () {
        ratio = GetComponent<RectTransform>().localScale.x / GetComponent<RectTransform>().localScale.y;

        if (isHot)
        {
            GetComponent<RectTransform>().localEulerAngles = new Vector3(0f, 0f, val * 90 - 90);
            //transform.GetChild(0).GetComponent<RectTransform>().localEulerAngles = new Vector3(0f, 0f, 90f - (val * 90f));
            transform.GetChild(0).GetComponent<Image>().fillAmount = val == 0f ? 0 : 1 - Mathf.Atan(Mathf.Tan(90f * (1 - val) * Mathf.Deg2Rad) * ratio) * Mathf.Rad2Deg / 90f;

        }
        else {
            GetComponent<RectTransform>().localEulerAngles = new Vector3(0f, 0f, 90f - (val * 90f));
            //transform.GetChild(0).GetComponent<RectTransform>().localEulerAngles = new Vector3(0f, 0f, val * 90 - 90);
            transform.GetChild(0).GetComponent<Image>().fillAmount = val == 0f ? 0 : 1 - Mathf.Atan(Mathf.Tan(90f * (1 - val) * Mathf.Deg2Rad) * ratio) * Mathf.Rad2Deg / 90f;

        }
        // transform.GetChild(0).GetComponent<Image>().fillAmount = val;
    }
    public void SetVal(float emptied)
    {
        val = (0.8f*area-emptied) / (area);
    }
}
