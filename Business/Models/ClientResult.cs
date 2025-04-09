namespace Business.Models;

public class ClientResult<T> : BaseResult
{
    public T? Data { get; set; }
}

public class ClientResult : BaseResult
{
}
