namespace NScan.Lib.Union7
{
  public interface IUnionVisitor<in T1, in T2, in T3, in T4, in T5, in T6, in T7>
  {
    void Visit(T1 arg);
    void Visit(T2 dto);
    void Visit(T3 dto);
    void Visit(T4 dto);
    void Visit(T5 dto);
    void Visit(T6 dto);
    void Visit(T7 dto);
  }
}
