namespace API.ViewModels;

public class ResponseErrorVM<TEntity>
{
    public int Code { get; set; }
    public string Status { get; set; }
    public TEntity Errors { get; set; }
}
