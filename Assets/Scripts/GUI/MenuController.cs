using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour {
    [SerializeField]
    private SceneFade sceneFade;
    
    [SerializeField]
    private GameObject mainPanel;
    
    [SerializeField]
    private GameObject creditsPanel;
    
    void Start () {
        mainPanel.SetActive(true);
        creditsPanel.SetActive(false);
    }
    
    void Update () {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (mainPanel.activeInHierarchy)
                OnPlayerQuit();
            else
                OnBackClicked();
        }
    }
    
	public void OnPlayClicked () {
        sceneFade.OnFadeFinished += OnFadeToGameplayFinished;
        StartCoroutine(sceneFade.FadeOut(0.5f));
    }
    
    public void OnCreditsClicked () {
        sceneFade.OnFadeFinished += OnFadeToCreditsFinished;
        StartCoroutine(sceneFade.FadeOut(0.5f));
    }
    
     public void OnHighScoresClicked () {
        sceneFade.OnFadeFinished += OnFadeToHighScoresFinished;
        StartCoroutine(sceneFade.FadeOut(0.5f));
    }
    
    public void OnBackClicked () {
        sceneFade.OnFadeFinished += OnFadeToMainFinished;
        StartCoroutine(sceneFade.FadeOut(1));
    }
    
    public void OnPlayerQuit () {
        Application.Quit();
    }
    
     public void OnFadeToGameplayFinished () {
        sceneFade.OnFadeFinished -= OnFadeToGameplayFinished;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
    }
    
    public void OnFadeToCreditsFinished () {
        sceneFade.OnFadeFinished -= OnFadeToCreditsFinished;
        mainPanel.SetActive(false);
        creditsPanel.SetActive(true);
        StartCoroutine(sceneFade.FadeIn(1));
    }
    
    public void OnFadeToHighScoresFinished () {
        sceneFade.OnFadeFinished -= OnFadeToHighScoresFinished;
        mainPanel.SetActive(false);
        StartCoroutine(sceneFade.FadeIn(1));
    }
    
    public void OnFadeToMainFinished () {
        sceneFade.OnFadeFinished -= OnFadeToMainFinished;
        mainPanel.SetActive(true);
        creditsPanel.SetActive(false);
        StartCoroutine(sceneFade.FadeIn(1));
    }
}
