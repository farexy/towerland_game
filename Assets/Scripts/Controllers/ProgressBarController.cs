using UnityEngine;

public class ProgressBarController : MonoBehaviour
{	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetProgressRate(float rate)
	{
		var newWidth = rate * transform.parent.GetComponent<RectTransform>().sizeDelta.x;
		transform.GetComponent<RectTransform>().sizeDelta = new Vector2(newWidth, transform.GetComponent<RectTransform>().sizeDelta.y);
		transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(newWidth / 2,0);
	}
}
