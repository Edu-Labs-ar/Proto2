using System.Collections.Generic;

namespace EduLabs
{
  public static class UtilExtensions
  {
    //COPIED FROM https://stackoverflow.com/a/1211626/6844034
    public static IEnumerable<T> ReverseIterator<T>(this IList<T> items)
    {
      for (int i = items.Count - 1; i >= 0; i--)
        yield return items[i];
    }
  }
}