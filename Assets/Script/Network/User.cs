using UnityEngine;
using System;
using System.Collections;
using Facebook;

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

    public void LoginWithFacebook(Action successCallback)
    {
        loginSuccessCallback = successCallback;
        FB.Init(() =>
        {
            FB.Login("", AuthCallback);
        });
    }

    private void AuthCallback(FBResult result)
    {
        if (FB.IsLoggedIn)
        {
            Debug.Log(FB.UserId);
            loginType = LoginType.Facebook;
            userID = FB.UserId;
            if (loginSuccessCallback != null)
                loginSuccessCallback();
        }
        else
        {
            Debug.Log("User cancelled login");
        }
    }

    public void StartWithoutLogin(Action successCallback)
    {
        loginSuccessCallback = successCallback;
        loginType = LoginType.None;
        // TODO: Setting default user.
        if (loginSuccessCallback != null)
            loginSuccessCallback();
    }

    public static User GetInstance
    {
        get
        {
            if (user == null)
                user = new User();
            return user;
        }
    }
}
