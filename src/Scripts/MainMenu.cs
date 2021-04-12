using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public Animator transition;
    public GameObject[] textMeshes;
    void  Awake() {
        FindObjectOfType<AudioManager>().Play("MenuTheme");
    }
    public void PlayGame()
    {
        FindObjectOfType<AudioManager>().Play("Start");
        foreach (GameObject textMesh in textMeshes)
        {
            textMesh.GetComponent<TextMeshPro>().SetText("");
        }
        StartCoroutine(LoadScene(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void Over()
    {
        FindObjectOfType<AudioManager>().Play("ButtonHoover");
    }
    IEnumerator LoadScene(int sceneIndex)
    {
        transition.SetTrigger("start");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(sceneIndex);
    }
}
