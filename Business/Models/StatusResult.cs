namespace Business.Models;

public class StatusResult<T> : BaseResult
{
    public T? Data { get; set; }
}

public class StatusResult : BaseResult
{
}
