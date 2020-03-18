using System.Collections.Generic;

public class MegaViewModel<T>
{
    public MegaViewModel(T data)
    {
        Status = MegaStatus.Successfull;
        Messages = new List<string>();
        Data = data;
    }
    public MegaViewModel(T data, MegaStatus megaStatus)
    {
        Status = megaStatus;
        Messages = new List<string>();
        Data = data;
    }
    public MegaViewModel(params string[] messages)
    {
        Status = MegaStatus.Failed;
        Messages = new List<string>(messages);
    }

    public MegaStatus Status { get; set; }
    public List<string> Messages { get; set; }
    public T Data { get; set; }
}
public enum MegaStatus
{
    Successfull = 200,
    Failed = 500,
    Unauthorized = 400
}