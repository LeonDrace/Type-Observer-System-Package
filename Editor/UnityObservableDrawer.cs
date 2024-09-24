using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace LeonDrace.TypeObserverEventSystem.Editor
{
	[CustomPropertyDrawer(typeof(UnityObservable<>))]
	public class UnityObservableDrawer : PropertyDrawer
	{
		private float m_SingleLineHeight = EditorGUIUtility.singleLineHeight;
		private float m_Padding = 2f;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginChangeCheck();
			Rect rect = position;
			//Override the position because it has the full height applied from the GetPropertyHeight method.
			rect.height = m_SingleLineHeight + m_Padding;
			//Create a foldout at the label position.
			property.isExpanded = EditorGUI.Foldout(GetLabelRect(rect), property.isExpanded, label);
			//Create a property field for the value on without a label and on the same height as the foldout but at the field position.
			EditorGUI.PropertyField(GetFieldRect(rect), property.FindPropertyRelative("value"), GUIContent.none, true);

			//When expanded draw the on value changed UnityEvent.
			if (property.isExpanded)
			{
				//Set height to onValueChanged property height.
				rect.height = EditorGUI.GetPropertyHeight(property.FindPropertyRelative("onValueChanged"));
				//Adjust position by the defaul margin.
				rect.y += m_SingleLineHeight + m_Padding;
				//Draw UnityEvent onValueChanged field.
				EditorGUI.PropertyField(rect, property.FindPropertyRelative("onValueChanged"), label, true);
			}

			if (EditorGUI.EndChangeCheck())
			{
				//Apply properties before invoking.
				property.serializedObject.ApplyModifiedProperties();
				//Find the object through the field info.
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

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			//Additional height required for the UnityEvent onValueChanged.
			float additionalHeight = 0;
			if (property.isExpanded)
			{
				additionalHeight = EditorGUI.GetPropertyHeight(property.FindPropertyRelative("onValueChanged"));
			}

			return base.GetPropertyHeight(property, label) + additionalHeight;
		}

		/// <summary>
		/// Given a position rect, get its field portion
		/// </summary>
		public static Rect GetFieldRect(Rect position)
		{
			position.width -= EditorGUIUtility.labelWidth;
			position.x += EditorGUIUtility.labelWidth;
			return position;
		}
		/// <summary>
		/// Given a position rect, get its label portion
		/// </summary>
		public static Rect GetLabelRect(Rect position)
		{
			position.width = EditorGUIUtility.labelWidth - 2f;
			return position;
		}
	}
}