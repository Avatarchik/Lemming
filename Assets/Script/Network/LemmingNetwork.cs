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
	private string serverURL = "http://52.68.233.250:8000/LemmingServerApp/";

	private void PostRequest<T> (string protocol, Dictionary<string, string> parameters, Action<LemmingNetworkResult<T>> successCallback, Action<ErrorResult> failCallback, bool sessionRequired = true)
	{
		parameters.Add ("sessionRequired", sessionRequired.ToString ());
		byte[] byteArray = Encoding.UTF8.GetBytes (ConstructQueryString (parameters));

		ObservableWWW.PostWWW (serverURL + protocol, byteArray)
			.Subscribe (
				x => ParseResponse (x.text, successCallback, failCallback),
				ex => Debug.LogException (ex));
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

	public void Login (string facebookID, Action<LemmingNetworkResult<EmptyResult>> successCallback, Action<ErrorResult> failCallback)
	{
		var parameters = new Dictionary<string, string> ();
		parameters.Add ("facebookID", facebookID);

		PostRequest<EmptyResult> ("account/login", parameters, successCallback, failCallback, sessionRequired: false);
	}
	
	public void MakeUser (string facebookID, string name, Action<LemmingNetworkResult<EmptyResult>> successCallback, Action<ErrorResult> failCallback)
	{
		var parameters = new Dictionary<string, string> ();
		parameters.Add ("facebookID", facebookID);
		parameters.Add ("name", name);

		PostRequest<EmptyResult> ("account/makeUser", parameters, successCallback, failCallback, sessionRequired: false);
	}

	public static LemmingNetwork GetInstance {
		get {
			if (network == null)
				network = new LemmingNetwork ();
			return network;
		}
	}
}
