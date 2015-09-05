using UnityEngine;
using System;
using System.Collections;
using Facebook;
using Newtonsoft.Json;

public class User
{
	public static User user;
	private string userID;
	private string userName;
	private LoginType loginType;
	private Action loginSuccessCallback;

	public enum LoginType
	{
		None,
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
					FB.API ("/me?fields=first_name", Facebook.HttpMethod.GET, (result) => {
						IDictionary dict = Facebook.MiniJSON.Json.Deserialize (result.Text) as IDictionary;
						userName = dict ["first_name"].ToString ();
						Debug.Log ("your name is: " + userName);
						LoginToGameServer();
					});
				} else {
					Debug.Log ("User cancelled login");
				}
			});
		});
	}

	private void LoginToGameServer ()
	{
		LemmingNetwork.GetInstance.Login (userID, (e) => {
			Debug.Log("Loing success");
			if (loginSuccessCallback != null)
				loginSuccessCallback ();
		}, (error) => {
			Debug.LogError(error.text);
			if (error.text == "invalidUser")
			{
				RegisterUser(() => {
					LemmingNetwork.GetInstance.Login (userID, (e) => {
						Debug.Log("Loing success");
						if (loginSuccessCallback != null)
							loginSuccessCallback ();
					}, (erorr) => {
						Debug.LogError(error.text);
					});
				});
			}
		});
	}

	private void RegisterUser(Action callback)
	{
		LemmingNetwork.GetInstance.MakeUser (userID, userName, (e) => {
			callback();
		}, (error) => {
			Debug.LogError(error.text);
		});
	}

	public void StartWithoutLogin (Action successCallback)
	{
		loginSuccessCallback = successCallback;
		loginType = LoginType.None;

		// TODO: Setting default user.
		if (loginSuccessCallback != null)
			loginSuccessCallback ();
	}

	public static User GetInstance {
		get {
			if (user == null)
				user = new User ();
			return user;
		}
	}
}
