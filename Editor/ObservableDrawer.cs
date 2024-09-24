using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace LeonDrace.TypeObserverEventSystem.Editor
{
	[CustomPropertyDrawer(typeof(Observable<>))]
	public class ObservableDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginChangeCheck();

			//Draw field as a normal field without foldout.
			EditorGUI.PropertyField(position, property.FindPropertyRelative("value"), label, true);

			if (EditorGUI.EndChangeCheck())
			{
				//Apply properties before invoking.
				property.serializedObject.ApplyModifiedProperties();
				//Find the object through field info.
				object info = fieldInfo.GetValue(property.serializedObject.targetObject);
				//Get the method through object infos type.
				MethodInfo methodInfo = info.GetType().GetMethod("Invoke");
				//If found execute invoke to update all listeners when changing the value in the inspector too.
				if (methodInfo != null)
				{
					methodInfo.Invoke(info, null);
				}
			}
		}
	}
}