using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PattRotator : MonoBehaviour {

	private int SIZE;
	private const float ALPHA_CHANGE_SPEED = 0.02f;
	private float ALPHA_DIRECTION;
	private const float ROTATE_SPEED = 0.05f;
	private const float PATTERN_CHANGE_TIME=30f;
	private const float ALPHA_CHANGE_TIME = 10f;
    private const float MIN_ALPHA = 0.3f;//0.05f;
    private const float MAX_ALPHA = 0.5f;//0.15f;
	private const float MIN_SCALE = 0.9f;
	private const float MAX_SCALE = 1.3f;
	private const float SCALE_CHANGE_SPEED = 0.02f;
	private const float SCALE_CHANGE_TIME = 10f;
	private float SCALE_DIRECTION;

	public Sprite[] patt;

	private bool transitionStable=true, directionDown=true;
	private Color temp;

	void Start () {
		ALPHA_DIRECTION = 1f;
		SCALE_DIRECTION = 1f;
		SIZE = patt.Length;
		GetComponent<Image> ().sprite = patt [Random.Range (0, SIZE)];
	//	InvokeRepeating ("pattChanger", 0f, PATTERN_CHANGE_TIME);
		InvokeRepeating ("directionChanger", 0f, ALPHA_CHANGE_TIME);
		temp = GetComponent<Image> ().color;
	}

	void FixedUpdate () {
		if (transitionStable) {
			temp.a = Mathf.Min (MAX_ALPHA, temp.a + ALPHA_CHANGE_SPEED * Time.deltaTime * ALPHA_DIRECTION);
			temp.a = Mathf.Max (MIN_ALPHA, temp.a);
		} else if (directionDown) {
			temp.a = Mathf.Max (0f, temp.a + ALPHA_CHANGE_SPEED * Time.deltaTime * -1);
			if (temp.a == 0) {
				directionDown = !directionDown;
				Sprite prevSprite = GetComponent<Image> ().sprite, curSprite=null;
				do {
					curSprite = patt [Random.Range (0, SIZE)];
				} while(curSprite == prevSprite);
				GetComponent<Image> ().sprite = curSprite;
			}
		} else if (!directionDown) {
			temp.a = Mathf.Min (0.1f, temp.a + ALPHA_CHANGE_SPEED * Time.deltaTime * 1);
			if (temp.a == 0.1f) {
				transitionStable = true;
				directionDown = !directionDown;
			}
		}	
		transform.Rotate (new Vector3 (0f, 0f, ROTATE_SPEED));
		GetComponent<Image> ().color = temp;
		Vector3 tempScale = transform.localScale;
		tempScale.x = Mathf.Min (MAX_SCALE, tempScale.x + SCALE_CHANGE_SPEED * Time.deltaTime * SCALE_DIRECTION);
		tempScale.x = Mathf.Max (MIN_SCALE, tempScale.x);
		tempScale.y = tempScale.x;
		transform.localScale = tempScale;
	}

	void directionChanger()
	{
		ALPHA_DIRECTION *= -1;
		SCALE_DIRECTION *= -1;
	}

	public void pattChanger()
	{
		transitionStable = false;
	}
}
