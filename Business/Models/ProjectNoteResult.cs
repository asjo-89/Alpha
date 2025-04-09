namespace Business.Models;

public class ProjectNoteResult<T> : BaseResult
{
    public T? Data { get; set; }
}

public class ProjectNoteResult : BaseResult
{
}
