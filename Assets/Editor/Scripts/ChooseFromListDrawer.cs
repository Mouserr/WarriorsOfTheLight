using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Core.ConstantsContainers;
using UnityEngine;
using UnityEditor;


[CustomPropertyDrawer(typeof(ChooseFromListAttribute), true)]
public class ChooseFromListDrawer : PropertyDrawer
{
	GUIContent[] variants = null;
	private List<string> values = null;

	// Draw the property inside the given rect
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		// Using BeginProperty / EndProperty on the parent property means that
		// prefab override logic works on the entire property.
		EditorGUI.BeginProperty(position, label, property);


		if (values == null)
		{
			// First get the attribute since it contains the range for the slider
			ChooseFromListAttribute chooseFromList = attribute as ChooseFromListAttribute;
			IConstantsContainer container;

			if (chooseFromList != null && typeof(IConstantsContainer).IsAssignableFrom(chooseFromList.ContainerType))
			{
				container = Activator.CreateInstance(chooseFromList.ContainerType) as IConstantsContainer;
				if (container == null)
				{
					Debug.LogError("Failed to get container");
					return;
				}


			}
			else
			{
				Debug.LogError("Failed to get interface");
				return;
			}

			values = container.GetConstants();
		}
		if (variants == null)
		{
			variants = values.Select(x => new GUIContent(x)).ToArray();
		}

		if (property.type.ToLower() != "string")
		{
			for (int i = 0; i < property.arraySize; i++)
			{
				ShowSelectControl(position, property.GetArrayElementAtIndex(i), label);
			}
		}
		else
		{
			ShowSelectControl(position, property, label);
		}

		EditorGUI.EndProperty();
	}

	private void ShowSelectControl(Rect position, SerializedProperty property, GUIContent label)
	{
		int curIndex = values.IndexOf(property.stringValue);
		int newIndex = EditorGUI.Popup(position,
			label,
			curIndex >= 0 ? curIndex : 0,
			variants);

		if (newIndex >= 0 && newIndex != curIndex)
		{
			property.stringValue = values[newIndex];
		}
	}
}

