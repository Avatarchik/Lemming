using UnityEngine;
using System;
using System.Collections;
using Facebook;

public class User
{
	public static User user;
	private string userID;
	private string nickName;
	private LoginType loginType;
	private Action loginSuccessCallback;
	private float record;

	public string UserID
	{
		get { return userID; }
	}

	public LoginType GetLoginType
	{
		get { return loginType; }
	}
	
	public string NickName
	{
		get { return nickName; }
	}

	public float Record
	{
		get { return record; }
		set { record = value; }
	}

	public enum LoginType
	{
		Guest,
		Facebook,
	}

	public void LoginWithFacebook (Action successCallback)
	{
		loginSuccessCallback = successCallback;
		FB.Init (() =>
		{
			FB.Login ("", (x) => {
				if (FB.IsLoggedIn) {
					Debug.Log (FB.UserId);
					loginType = LoginType.Facebook;
					userID = FB.UserId;
					LoginToGameServer();
				} else {
					Debug.Log ("User cancelled login");
				}
			});
		});
	}

	private void LoginToGameServer ()
	{
		LemmingNetwork.GetInstance.Login (userID, (response) => {
			this.nickName = response.GetFirstResult().nickName;
			this.record = response.GetFirstResult().record;

			Debug.Log("Loing success");
			if (loginSuccessCallback != null)
				loginSuccessCallback ();
		}, (error) => {
			Debug.LogError(error.text);
			if (error.text == "invalidUser")
			{
				ShowMakeUserPopup();
			}
		});
	}

	private void ShowMakeUserPopup()
	{
		// FIXME : Find to better solution.
		var menuController = GameObject.Find ("MenuController").GetComponent<MenuController> ();
		menuController.ShowSignUpPanel (() => {
			LemmingNetwork.GetInstance.Login (userID, (response) => {
				this.nickName = response.GetFirstResult().nickName;
				this.record = response.GetFirstResult().record;

				Debug.Log("Loing success");
				if (loginSuccessCallback != null)
					loginSuccessCallback ();
			}, (error) => {
				Debug.LogError(error.text);
			});
		});
	}

	public void StartWithoutLogin (Action successCallback)
	{
		loginSuccessCallback = successCallback;
		loginType = LoginType.Guest;
		userID = SystemInfo.deviceUniqueIdentifier;

		LoginToGameServer ();
	}

	public static User GetInstance {
		get {
			if (user == null)
				user = new User ();
			return user;
		}
	}
}
