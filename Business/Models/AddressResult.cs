namespace Business.Models
{
    public class AddressResult<T> : BaseResult
    {
        public T? Data { get; set; }
    }

    public class AddressResult : BaseResult
    {
    }
}
