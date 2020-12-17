using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cMainMenu : MonoBehaviour
{
    public Button bStart;
    public Button bLoadGame;
    public Button bOptions;
    public Button bAbout;

    public GameObject pOptions;

    GameManager gm;
    cOptions c_Options;

    private void Start()
    {
        gm = GameManager.gm;
    }

    public void OnStartGameClicked()
    {
        gm.LoadScene(1);
    }

    public void OnOptionsClicked()
    {
        c_Options = Instantiate(pOptions).GetComponent<cOptions>();
    }

    public void OnExitGameClicked()
    {
        gm.ExitGame();
    }

}
