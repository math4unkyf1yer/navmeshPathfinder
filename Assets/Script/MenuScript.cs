using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public GameObject volumePage;
    public GameObject creditPage;
    public void OnClickExit()
    {
        Application.Quit();//exit the game
    }
    public void OnClickStart()
    {
        SceneManager.LoadScene(1);
    }
    public void OnClickVolume()
    {
        volumePage.SetActive(true);
    }
    public void OnClickCredit()
    {
        creditPage.SetActive(true);
    }
}
