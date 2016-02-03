using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Bubble : MonoBehaviour {
	public Image image;

	public void ActivateBubble (SpriteRenderer renderer) {
		image.sprite = renderer.sprite;
		gameObject.SetActive (true);
	}

	public void DisableBubble () {
		gameObject.SetActive (false);
	}
}
