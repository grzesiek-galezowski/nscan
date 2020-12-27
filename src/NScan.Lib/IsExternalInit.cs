using System.ComponentModel;

// ReSharper disable once CheckNamespace
namespace System.Runtime.CompilerServices //bug running nscan should pick this is an error
{
  /// <summary>
  /// Reserved to be used by the compiler for tracking metadata.
  /// This class should not be used by developers in source code.
  /// </summary>
  [EditorBrowsable(EditorBrowsableState.Never)]
  public static class IsExternalInit {
  }
}
