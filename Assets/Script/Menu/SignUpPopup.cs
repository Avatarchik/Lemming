using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class SignUpPopup : MonoBehaviour
{
	[SerializeField]
	private InputField nickNameField;
	private Action callback;

	private void SignUp()
	{
		var nickName = nickNameField.text;

		if (nickName.Trim() == string.Empty)
			return;

		LemmingNetwork.GetInstance.MakeUser (User.GetInstance.UserID, User.GetInstance.GetLoginType, nickName, (e) => {
			gameObject.SetActive(false);
			callback();
		}, (ErrorResult) => {});
	}

	public void OpenPopup(Action successCallback)
	{
		callback = successCallback;
		gameObject.SetActive (true);
	}
}
