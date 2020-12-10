using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject mainmenucomponents;
    public GameObject rulescomponents;

    private void Start()
    {
        //play.enabled = false;
    }

    public void ShowRules()
    {
        rulescomponents.gameObject.SetActive(true);
        mainmenucomponents.gameObject.SetActive(false);
    }

    public void HideRules()
    {
        rulescomponents.gameObject.SetActive(false);
        mainmenucomponents.gameObject.SetActive(true);
    }

}
