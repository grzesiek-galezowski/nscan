namespace NScanSpecification.Lib.AutomationLayer;

public abstract class GenericReportedMessage<T>(string text) : ReportedMessage(text)
{
  public T Error()
  {
    return NewInstance(ToString() + ": [ERROR]");
  }

  public T Ok()
  {
    return NewInstance(ToString() + ": [OK]");
  }

  protected abstract T NewInstance(string str);
}
