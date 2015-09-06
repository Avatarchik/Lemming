using UnityEngine;
using System.Collections;
using Facebook;
using System;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenu;
    [SerializeField]
    private GameObject loginUI;
	[SerializeField]
	private GameObject signUpPanel;

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
            HideLoginAndShowMainMenu();
        });
	}
    
    [UnityEventListener]
    private void StartWithoutLogin()
    {
    	User.GetInstance.StartWithoutLogin(() => {
            HideLoginAndShowMainMenu();
        });
    }
    
    private void HideLoginAndShowMainMenu()
    {
        loginUI.SetActive(false);
        mainMenu.SetActive(true);
    }

	public void ShowSignUpPanel(Action callback)
	{
		signUpPanel.GetComponent<SignUpPopup> ().OpenPopup (callback);
	}
}
