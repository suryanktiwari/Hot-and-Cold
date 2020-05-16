using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class colorTransitionerOld : MonoBehaviour
{

    private const float MAX_COLOR_THRESHOLD = 1f;
    private const float MIN_COLOR_THRESHOLD = 0.4f;
    private const float COLOR_CHANGE_SPEED = 30f;

    private const float TRANSPARENCY = 1f;

    private Vector4[] red = { new Vector4(MAX_COLOR_THRESHOLD, 0f, 0f, TRANSPARENCY), new Vector4(MIN_COLOR_THRESHOLD, 0f, 0f, TRANSPARENCY) };
    private Vector4[] blue = { new Vector4(0f, 0f, MAX_COLOR_THRESHOLD, TRANSPARENCY), new Vector4(0f, 0f, MIN_COLOR_THRESHOLD, TRANSPARENCY) };
    private Vector4[] yellow = { new Vector4(MAX_COLOR_THRESHOLD, MIN_COLOR_THRESHOLD, MIN_COLOR_THRESHOLD, TRANSPARENCY), new Vector4(MAX_COLOR_THRESHOLD-0.05f, MAX_COLOR_THRESHOLD - 0.05f, MIN_COLOR_THRESHOLD, TRANSPARENCY) };

    private List<Vector4[]> colors = new List<Vector4[]>();

    private Color color1, color2;

    private Image backgroundPanelImage;

    private int targetBackground, direction = 1;

    void Start()
    {
        backgroundPanelImage = GetComponent<Image>();
        colors.Add(red); colors.Add(blue); colors.Add(yellow);
        // targetBackground = PlayerPrefs.GetInt("targetBg");
        targetBackground = 2;
        color1 = new Color(colors[targetBackground][0].x, colors[targetBackground][0].y, colors[targetBackground][0].z, colors[targetBackground][0].w);
        color2 = new Color(colors[targetBackground][1].x, colors[targetBackground][1].y, colors[targetBackground][1].z, colors[targetBackground][1].w);
        //InvokeRepeating("colorLerper", 0f, COLOR_CHANGE_SPEED*2);
        //    InvokeRepeating("directionChanger", 0f, DIRECTION_CHANGE_SPEED);
        StartCoroutine(ChangeColor(backgroundPanelImage, color1, color2, 5f));
    }

    void Update()
    {

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
        StartCoroutine(ChangeColor(backgroundPanelImage, to, from, COLOR_CHANGE_SPEED / 2));
    }
}