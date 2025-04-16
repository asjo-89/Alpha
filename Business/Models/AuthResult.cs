namespace Business.Models;

public class AuthResult<T> : BaseResult
{
    public T? Data { get; set; }
}
public class AuthResult : BaseResult
{
}
