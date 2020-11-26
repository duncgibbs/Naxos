using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    private Animator animator;
    private int levelToLoad = 0;
    void Start() {
        animator = gameObject.GetComponent<Animator>();
        GameObject canvasObject = gameObject.transform.GetChild(0).gameObject;
        GameObject textObject = canvasObject.transform.GetChild(1).gameObject;
        
        int currentLevel = SceneManager.GetActiveScene().buildIndex;

        if (currentLevel == 0) {
            textObject.GetComponent<Text>().text = "NAXOS";
        } else if (currentLevel == 1) {
            textObject.GetComponent<Text>().text = "LEVEL ONE COMPLETE";
        } else if (currentLevel == 2) {
            textObject.GetComponent<Text>().text = "LEVEL TWO COMPLETE";
        }
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void FadeToLevel(int level) {
        if (level < 3) {
            levelToLoad = level;
        } else {
            Application.Quit();
        }

        string fadeText = "NAXOS";
        if (level == 1) {
            fadeText = "LEVEL ONE COMPLETE";
        } else if (level == 2) {
            fadeText = "LEVEL TWO COMPLETE";
        } else if (level == 3) {
            fadeText = "LEVEL THREE COMPLETE";
        }

        GameObject canvasObject = gameObject.transform.GetChild(0).gameObject;
        GameObject textObject = canvasObject.transform.GetChild(1).gameObject;
        textObject.GetComponent<Text>().text = fadeText;
        animator.SetTrigger("FadeOut");
    }

    public void OnFadeComplete() {
        SceneManager.LoadScene(levelToLoad);
    }
}
