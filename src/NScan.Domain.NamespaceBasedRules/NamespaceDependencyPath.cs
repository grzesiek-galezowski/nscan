using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using Value;

namespace NScan.NamespaceBasedRules;

public class NamespaceDependencyPath(Seq<NamespaceName> elements) : ValueType<NamespaceDependencyPath>
{
  private static readonly SimpleDependencyPathFormat DefaultFormat = new();

  public static NamespaceDependencyPath Empty() => new(Seq<NamespaceName>.Empty);
  public static NamespaceDependencyPath With(params NamespaceName[] args) => new(args.ToSeq());

  protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality()
  {
    yield return elements;
  }

  public override string ToString()
  {
    return ToStringFormatted(DefaultFormat);
  }

  public bool IsPathToItself()
  {
    return elements.Count == 2 && BeginsAndEndsWithTheSameElement();
  }

  public bool FormsACycleFromFirstElement()
  {
    return elements.Count > 2 && BeginsAndEndsWithTheSameElement();
  }

  public bool ContainsACycleButNotFromFirstElement()
  {
    return ContainsANamespaceTwice() && !BeginsAndEndsWithTheSameElement();
  }

  public NamespaceDependencyPath Plus(NamespaceName namespaceName) 
    => new(elements.Add(namespaceName));

  public bool HasElements()
  {
    return elements.Count > 0;
  }

  private bool IsEquivalentTo(NamespaceDependencyPath other)
  {
    return other
      .ElementsOrderedForEquivalencyComparison()
      .SequenceEqual(
        ElementsOrderedForEquivalencyComparison());
  }

  private IOrderedEnumerable<NamespaceName> ElementsOrderedForEquivalencyComparison()
  {
    return elements
      .Distinct()
      .OrderBy(s => s.ToString());
  }

  private bool BeginsAndEndsWithTheSameElement()
  {
    return elements[0] == elements[^1];
  }

  private bool ContainsANamespaceTwice()
  {
    return elements.GroupBy(n => n).Where(n => n.Count() > 1).Sum(n => n.Count()) > 0;
  }

  public bool IsEquivalentToAnyOf(IEnumerable<NamespaceDependencyPath> cycles)
  {
    return !cycles.Any(c => c.IsEquivalentTo(this));
  }

  public string ToStringFormatted(INamespaceDependencyPathFormat format)
  {
    string result = string.Empty;
    for (var cycleElementIndex = 0; cycleElementIndex < elements.Count; cycleElementIndex++)
    {
      var segment = elements[cycleElementIndex].ToString();
      result += format.ElementIndentation(cycleElementIndex) + segment + format.ElementTerminator();
    }

    return result;
  }

  private class SimpleDependencyPathFormat : INamespaceDependencyPathFormat
  {
    public string ElementTerminator()
    {
      return ", ";
    }

    public string ElementIndentation(int elementIndex)
    {
      return string.Empty;
    }
  }
}
