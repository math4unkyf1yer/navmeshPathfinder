using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public GameObject volumePage;
    public GameObject creditPage;
    public GameObject[] closeItem;
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
        foreach (GameObject item in closeItem)
        {
            item.SetActive(false);
        }
    }
    public void OnClickCredit()
    {
        creditPage.SetActive(true);
        foreach (GameObject item in closeItem)
        {
            item.SetActive(false);
        }
    }
    public void OnClickBackToMenu()
    {
        foreach (GameObject item in closeItem)
        {
            item.SetActive(true);
        }
        creditPage.SetActive(false);
        volumePage.SetActive(false);
    }
}
