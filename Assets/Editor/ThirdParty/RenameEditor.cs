// COPIED FROM https://answers.unity.com/answers/1487948/view.html
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(RenameAttribute))]
public class RenameEditor : PropertyDrawer
{
  public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
  {
    EditorGUI.PropertyField(position, property, new GUIContent((attribute as RenameAttribute).NewName));
  }
}
#endif
