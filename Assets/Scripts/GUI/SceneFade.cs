using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SceneFade : MonoBehaviour {
    public delegate void FadeFinishedEventHandler();
    public event FadeFinishedEventHandler OnFadeFinished;
    

    private CanvasGroup canvasGroup;
    
	// Use this for initialization
	void Start () {
	   gameObject.SetActive(true);
       canvasGroup = GetComponent<CanvasGroup>();
       StartCoroutine(FadeIn(1.5f));
	}
    
    public IEnumerator FadeIn (float fadeDuration) {
        setCanvasActive(true);
        while (canvasGroup.alpha > 0) {
            canvasGroup.alpha -= Time.deltaTime/fadeDuration;
            yield return 0;
        }
        setCanvasActive(false);
        if (OnFadeFinished != null)
            OnFadeFinished();
    }
    
    public IEnumerator FadeOut (float fadeDuration) {
        setCanvasActive(true);
        while (canvasGroup.alpha < 1) {
            canvasGroup.alpha += Time.deltaTime/fadeDuration;
            yield return 0;
        }
        if (OnFadeFinished != null)
            OnFadeFinished();
    }
    
    private void setCanvasActive (bool active) {
        canvasGroup.blocksRaycasts = active;
        canvasGroup.interactable = active;
    }
}
