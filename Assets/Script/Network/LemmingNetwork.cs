using UnityEngine;
using UniRx;
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

public class LemmingNetwork
{
	private static LemmingNetwork network = null;
	private Dictionary<string, string> header = new Dictionary<string, string>();
	//private string serverURL = "http://52.68.233.250:8000/LemmingServerApp/";
	private string serverURL = "http://localhost:8000/LemmingServerApp/";

	private void PostRequest<T> (string protocol, Dictionary<string, string> parameters, Action<LemmingNetworkResult<T>> successCallback, Action<ErrorResult> failCallback, bool sessionRequired = true)
	{
		parameters.Add ("sessionRequired", sessionRequired ? "true" : "false");
		byte[] byteArray = Encoding.UTF8.GetBytes (ConstructQueryString (parameters));

		ObservableWWW.PostWWW (serverURL + protocol, byteArray, header)
			.Subscribe (
			(x) => {
				if (x.responseHeaders.ContainsKey("SET-COOKIE"))
				{
					string[] v = x.responseHeaders["SET-COOKIE"].Split(';');
					foreach (string s in v)
					{
						if (string.IsNullOrEmpty(s)) continue;
						if (s.Contains("sessionid"))
						{
							header.Add("Cookie", s);
							break;
						}
					}
				}
				ParseResponse (x.text, successCallback, failCallback);
			},
			(ex) => {
				Debug.LogException (ex);
			});
	}

	private static void ParseResponse<T> (string responseText, Action<LemmingNetworkResult<T>> successCallback, Action<ErrorResult> failCallback)
	{
		LemmingNetworkResult<T> lemmingNetworkResult = null;
		Debug.Log (responseText);
		try {
			lemmingNetworkResult = JsonConvert.DeserializeObject<LemmingNetworkResult<T>> (responseText);
		} catch (Exception e) {
			Debug.LogError("response parsing error...");
			Debug.LogError(e.StackTrace);
		}

		if (lemmingNetworkResult.status == "ok") {
			successCallback (lemmingNetworkResult);
		} else if (lemmingNetworkResult.status == "fail") {
			var errorResult = new ErrorResult (lemmingNetworkResult.errorResult as string);
			failCallback (errorResult);
		} else {
			Debug.LogError ("Invalid network response");
		}
	}

	private static String ConstructQueryString (Dictionary<string, string> parameters)
	{
		List<string> items = new List<string> ();
		parameters.Keys.ToList ().ForEach (key => items.Add (string.Concat (key, "=", WWW.EscapeURL (parameters [key]))));
		return string.Join ("&", items.ToArray ());
	}

	public void Login (string userID, Action<LemmingNetworkResult<LoginResult>> successCallback, Action<ErrorResult> failCallback)
	{
		var parameters = new Dictionary<string, string> ();
		parameters.Add ("userID", userID);

		PostRequest<LoginResult> ("account/login", parameters, successCallback, failCallback, sessionRequired: false);
	}
	
	public void MakeUser (string userID, User.LoginType loginType, string nickName, Action<LemmingNetworkResult<EmptyResult>> successCallback, Action<ErrorResult> failCallback)
	{
		var parameters = new Dictionary<string, string> ();
		parameters.Add ("userID", userID);
		parameters.Add ("nickName", nickName);

		if (loginType == User.LoginType.Facebook) {
			parameters.Add ("userType", "facebook");
		} else {
			parameters.Add ("userType", "guest");
		}

		PostRequest<EmptyResult> ("account/makeUser", parameters, successCallback, failCallback, sessionRequired: false);
	}

	public void SetRecord (float record, Action<LemmingNetworkResult<EmptyResult>> successCallback, Action<ErrorResult> failCallback)
	{
		var parameters = new Dictionary<string, string> ();
		parameters.Add ("record", record.ToString());

		PostRequest<EmptyResult> ("account/setRecord", parameters, successCallback, failCallback, sessionRequired: true);
	}

	public static LemmingNetwork GetInstance {
		get {
			if (network == null)
				network = new LemmingNetwork ();
			return network;
		}
	}
}
