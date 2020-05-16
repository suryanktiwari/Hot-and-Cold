using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeUIRect : MonoBehaviour
{
    void Start()
    {
        RectTransform prt = transform.parent.GetComponent<RectTransform>();
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(prt.rect.width, prt.rect.height);
    }

}
