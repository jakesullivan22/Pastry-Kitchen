using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gm;

    int currentScene;

    public GameObject pMainMenu;

    cMainMenu c_MainMenu;

    private void Awake()
    {
        if(gm == null)
        {
            DontDestroyOnLoad(gameObject);
            gm = this;
        }
        else if(gm != this)
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentScene = scene.buildIndex;
        switch(scene.buildIndex)
        {
            case 0:
                c_MainMenu = Instantiate(pMainMenu).GetComponent<cMainMenu>();
                break;
            case 1:
                break;
        }
    }

    public void LoadScene(int _idx)
    {
        SceneManager.LoadScene(_idx);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
