namespace Business.Models;


public class PictureResult<T> : BaseResult
{
    public T? Data { get; set; }
}

public class PictureResult : BaseResult
{
}
