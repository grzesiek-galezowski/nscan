namespace NScanSpecification.Lib.AutomationLayer
{
  public class HasNoUsingsMessage : GenericReportedMessage<HasNoUsingsMessage>
  {
    public static HasNoUsingsMessage HasNoUsings(string project, string from, string to)
    {
      return new HasNoUsingsMessage(TestRuleFormats.FormatNoUsingsRule(project, from, to));
    }

    public HasNoUsingsMessage(string returnValue) : base(returnValue)
    {
    }

    protected override HasNoUsingsMessage NewInstance(string str)
    {
      return new HasNoUsingsMessage(str);
    }
  }
}