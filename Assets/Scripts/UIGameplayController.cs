using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIGameplayController : MonoBehaviour {
    public Text time;
    public GameObject[] activeEndings;

    GameObject gameOverPanel;

    void Start()
    {
        gameOverPanel = transform.FindChild("GameOverPanel").gameObject;
    }

    void OnApplicationQuit() {
        PlayerPrefs.DeleteAll();
    }

    public void ActiveGameOverUI(float time) {
        gameOverPanel.SetActive(true);
        string minutes = Mathf.Floor(time / 60).ToString("00");
        string seconds = (time % 60).ToString("00");
        this.time.text = minutes + ":" + seconds; ;
        ActiveUnlockedEndings();
    }

    public void ActiveUnlockedEndings() {
        for (int i = 0; i < activeEndings.Length; i++)
        {
            activeEndings[i].SetActive(PlayerPrefs.GetInt("Ending" + i) == 1 ? true : false);
        }
    }

    public void RestartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu() {
        FMODManager.SINGLETON.StopAllSounds();
        SceneManager.LoadScene(0);
    }


}
