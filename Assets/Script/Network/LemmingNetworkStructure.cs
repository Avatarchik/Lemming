using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public class LemmingNetworkResult<T>
{
	public T GetFirstResult() { return result[0]; }
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

public class UserRecord
{
	public int rank;
	public string nickName;
	public float record;
	public string userID;
	[JsonConverter(typeof(StringEnumConverter))]
	public User.LoginType userType;
}

public class GetWorldRecordsResult
{
	public List<UserRecord> userRecords;
}
