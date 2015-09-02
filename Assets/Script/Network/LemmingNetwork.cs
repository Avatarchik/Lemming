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

	private void PostRequest<T> (string protocol, Dictionary<string, string> parameters, Action<T> successCallback, Action<ErrorResult> failCallback, bool sessionRequired = true)
	{
		parameters.Add ("sessionRequired", sessionRequired.ToString ());
		byte[] byteArray = Encoding.UTF8.GetBytes (ConstructQueryString (parameters));

		ObservableWWW.PostWWW (serverURL + protocol, byteArray)
			.Subscribe (
				x => ParseResponse(x.text, successCallback, failCallback),
				ex => Debug.LogException (ex));
	}

	private static void ParseResponse<T> (string responseText, Action<T> successCallback, Action<ErrorResult> failCallback)
	{
		LemmingNetworkResult<T> lemmingNetworkResult = JsonConvert.DeserializeObject<LemmingNetworkResult<T>>(responseText);
		if (lemmingNetworkResult.status == "ok") {
			successCallback(lemmingNetworkResult.result);
		} else if (lemmingNetworkResult.status == "fail") {
			var errorResult = new ErrorResult (lemmingNetworkResult.result as string);
			failCallback (errorResult);
		} else {
			Debug.LogError("Invalid network response");
		}
	}

	private static String ConstructQueryString (Dictionary<string, string> parameters)
	{
		List<string> items = new List<string> ();
		parameters.Keys.ToList ().ForEach (key => items.Add (string.Concat (key, "=", WWW.EscapeURL (parameters [key]))));
		return string.Join ("&", items.ToArray ());
	}

	public void Login(string facebookID, Action<EmptyResult> successCallback, Action<ErrorResult> failCallback)
	{

	}

	public static LemmingNetwork GetInstance {
		get {
			if (network == null)
				network = new LemmingNetwork ();
			return network;
		}
	}
}
