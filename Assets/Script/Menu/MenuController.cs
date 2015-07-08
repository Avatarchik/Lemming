using UnityEngine;
using System.Collections;
using Facebook;
using System;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject ingameUI;
    [SerializeField]
    private GameObject menuUI;

    void Start()
    {

    }

    void Update()
    {

    }
	
    [UnityEventListener]
	private void LoginWithFacebook()
	{
    	User.GetInstance.LoginWithFacebook(() => {
            HideMenuAndShowInGame();
        });
	}
    
    [UnityEventListener]
    private void StartWithoutLogin()
    {
    	User.GetInstance.StartWithoutLogin(() => {
            HideMenuAndShowInGame();
        });
    }
    
    private void HideMenuAndShowInGame()
    {
        menuUI.SetActive(false);
        ingameUI.SetActive(true);
    }
}
