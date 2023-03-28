namespace DevEventsApi.ViewModels;

public class SuccessResponseViewModel<T>
{
  public SuccessResponseViewModel(T? data, List<string> errors)
  {
    Data = data;
    Errors = errors;
  }

  public SuccessResponseViewModel(T? data)
  {
    Data = data;
  }

  public SuccessResponseViewModel(List<string> errors)
  {
    Errors = errors;
  }

  public T? Data { get; private set; }
  public List<string> Errors { get; private set; } = new();
}