public class LemmingNetworkResult<T>
{
	public string status;
	public T result;
}

public class ErrorResult
{
	public string text;

	public ErrorResult (string text)
	{
		this.text = text;
	}
}

public class EmptyResult
{}