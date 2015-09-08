using System.Collections.Generic;

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
}

public class GetWorldRecordsResult
{
	public List<UserRecord> userRecords;
}
