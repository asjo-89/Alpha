namespace Business.Models;

public class RoleResult<T> : BaseResult
{
    public T? Data { get; set; }
}

public class RoleResult : BaseResult
{
}
