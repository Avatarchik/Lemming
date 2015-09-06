using System.Collections.Generic;

public class LemmingNetworkResult<T>
{
	public T GetFirstReulst() { return result[0]; }
	public string status;
	public List<T> result;
	public string errorResult;
}

public class ErrorResult
{
	public string text;

	public ErrorResult (string text)
	{
		this.text = text;
	}
}

public class LoginResult
{
	public string nickName;
	public float record;
}

public class EmptyResult
{}