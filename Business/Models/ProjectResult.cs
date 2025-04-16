namespace Business.Models;

public class ProjectResult<T> : BaseResult
{
    public T? Data { get; set; }
}

public class ProjectResult : BaseResult
{
}
