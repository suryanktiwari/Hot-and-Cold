using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class colorTransitioner : MonoBehaviour {

    private const float MAX_COLOR_THRESHOLD = 1f;
    private const float MIN_COLOR_THRESHOLD = 0.4f;
    private const float COLOR_CHANGE_SPEED = 7f;

    private const float TRANSPARENCY = 1f;

    private List<Color> colors = new List<Color>();

    private Image backgroundPanelImage;

    private int curColor, direction = 1;

    void Start () {
        backgroundPanelImage = GetComponent<Image>();
        // targetBackground = PlayerPrefs.GetInt("targetBg");
/*        colors.Add(new Color(0.74f, 0.59f, 0.90f, TRANSPARENCY));
        colors.Add(new Color(0.90f, 0.89f, 0.59f, TRANSPARENCY));
        colors.Add(new Color(0.90f, 0.70f, 0.63f, TRANSPARENCY));
        colors.Add(new Color(0.53f, 0.90f, 0.89f, TRANSPARENCY));
        colors.Add(new Color(0.95f, 1f, 0.72f, TRANSPARENCY));
*/
        colors.Add(new Color(1f, 0.80f, 0f, TRANSPARENCY));
        colors.Add(new Color(1f, 0.63f, 0.24f, TRANSPARENCY));
        colors.Add(new Color(1f, 0.33f, 0.27f, TRANSPARENCY));
        colors.Add(new Color(0.65f, 0.24f, 0.65f, TRANSPARENCY));
        colors.Add(new Color(0.34f, 0.89f, 0.79f, TRANSPARENCY));


        curColor = 1;

        //InvokeRepeating("colorLerper", 0f, COLOR_CHANGE_SPEED*2);
        //    InvokeRepeating("directionChanger", 0f, DIRECTION_CHANGE_SPEED);
        StartCoroutine(ChangeColor(backgroundPanelImage, colors[0], colors[1], COLOR_CHANGE_SPEED));
    }
	
	void Update () {
		
	}
    private IEnumerator ChangeColor(Image image, Color from, Color to, float duration)
    {
        float timeElapsed = 0.0f;

        float t = 0.0f;
        while (t < 1.0f)
        {
            timeElapsed += Time.deltaTime;

            t = timeElapsed / duration;

            image.color = Color.Lerp(from, to, t);

            yield return null;
        }
        //  print("duration= "+duration);
        from = to;
        int randInt;
        do
        {
            randInt = Random.Range(0, colors.Count);
            to = colors[randInt];
        } while (randInt == curColor);
        curColor = randInt;
        StartCoroutine(ChangeColor(backgroundPanelImage, from, to, COLOR_CHANGE_SPEED));
    }
}
