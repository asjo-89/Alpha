namespace Business.Models;

public class MemberUserResult<T> : BaseResult
{
    public T? Data { get; set; }
}

public class MemberUserResult : BaseResult
{
}
