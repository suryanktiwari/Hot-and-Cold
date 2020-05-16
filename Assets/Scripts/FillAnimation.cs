using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FillAnimation : MonoBehaviour {

    float startTime;
    const float DURATION = 1f;
    // Use this for initialization
	void Start () {
		
	}
	public void Animate() {
        startTime = Time.time;
        gameObject.SetActive(true);
        print("Animation is happening");
    }
    private void Update()
    {
        if (Time.time < startTime + DURATION)
        {
            GetComponent<Image>().fillAmount = Time.time - startTime;

        }
        else
        {
            //gameObject.SetActive(false);

        }
    }
}
