// COPIED FROM https://answers.unity.com/answers/1487948/view.html
using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
public class RenameAttribute : PropertyAttribute
{
  public string NewName { get; private set; }
  public RenameAttribute(string name)
  {
    NewName = name;
  }
}