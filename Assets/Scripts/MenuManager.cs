using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject firstScreen;
    public GameObject secondScreen;

    public void GoToNextScreen()
    {
        firstScreen.SetActive(false);
        secondScreen.SetActive(true);
    }

    public void EnterGame()
    {
        SceneManager.LoadScene("New Scene");
    }
}
