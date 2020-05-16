using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class glassColorController : MonoBehaviour {

    private const float COLOR_CHANGE_SPEED = 30f;

    private const float TRANSPARENCY = 1f;

    private Vector4 orange = new Vector4(1f, 0.69f, 0.15f, TRANSPARENCY);
    private Vector4 blue = new Vector4(0.15f, 0.72f, 1f, TRANSPARENCY);

    private Image backgroundPanelImage;

    private int targetBackground, direction;

    private bool directionChanged;

    private float hotVal, coldVal;
    private float m_Saturation, m_Hue, m_Value, h_Val_Blue, cur_Hue;         // orange color hsv values

    private Color color1;

    void Start () {
        h_Val_Blue = 0.55f;
        backgroundPanelImage = GetComponent<Image>();
        Color.RGBToHSV(backgroundPanelImage.color, out m_Hue, out m_Saturation, out m_Value);
        direction = -1;
        directionChanged = false;
        backgroundPanelImage.color = Color.HSVToRGB(m_Hue, m_Saturation, m_Value);
        //StartCoroutine(ChangeColor(backgroundPanelImage, color1, color2, 5f));
    }
	
    public void MoheRangDe(float val)
    {
        m_Saturation += 0.01f*direction;
        if(m_Saturation<0.1f&&!directionChanged)
        {
            direction *= -1;
            directionChanged = true;
            backgroundPanelImage.color = blue;
            cur_Hue = h_Val_Blue;
        }
        backgroundPanelImage.color = Color.HSVToRGB(cur_Hue, m_Saturation, m_Value);
    }

    public void GetHotting(float val)
    {
        hotVal = val;
        direction = -1;
        cur_Hue = m_Hue;
        directionChanged = false;
    }

	void Update () {
		
	}

   /* private IEnumerator ChangeColor(Image image, Color from, Color to, float duration)
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
    */
}
