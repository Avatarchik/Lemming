using UnityEngine;
using UniRx;
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

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
				x => ParseResponse(x.text, successCallback, failCallback),
				ex => Debug.LogException (ex));
	}

	private void ParseResponse<T> (string responseText, Action<LemmingNetworkResult<T>> successCallback, Action<ErrorResult> failCallback)
	{
		if (true) {

		} else {
			failCallback(new ErrorResult());
		}
	}

	private static String ConstructQueryString (Dictionary<string, string> parameters)
	{
		List<string> items = new List<string> ();
		parameters.Keys.ToList ().ForEach (key => items.Add (string.Concat (key, "=", WWW.EscapeURL (parameters [key]))));
		return string.Join ("&", items.ToArray ());
	}

	public static LemmingNetwork GetInstance {
		get {
			if (network == null)
				network = new LemmingNetwork ();
			return network;
		}
	}
}
