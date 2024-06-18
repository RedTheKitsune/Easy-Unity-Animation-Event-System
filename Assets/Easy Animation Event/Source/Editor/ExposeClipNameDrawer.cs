/*
 * Script created by Ahmed Amine Laaraf
 * Date: June 12, 2024
 */

using UnityEditor;
using UnityEngine;

namespace EasyAnimationEvent
{
    [CustomPropertyDrawer(typeof(ExposeClipNameAttribute))]
    public class ExposeClipNameDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Check if the property is a string
            if (property.propertyType == SerializedPropertyType.String)
            {
                // Get the attribute
                var exposeClipNameAttribute = (ExposeClipNameAttribute)attribute;
                var targetObject = property.serializedObject.targetObject;

                // Get the Animator field using reflection
                var animatorField = targetObject.GetType().GetField(exposeClipNameAttribute.animatorFieldName);
                if (animatorField != null)
                {
                    var animator = animatorField.GetValue(targetObject) as Animator;
                    if (animator != null && animator.runtimeAnimatorController != null)
                    {
                        var animationClips = animator.runtimeAnimatorController.animationClips;
                        if (animationClips.Length > 0)
                        {
                            string[] clipNames = new string[animationClips.Length];
                            for (int i = 0; i < animationClips.Length; i++)
                            {
                                clipNames[i] = animationClips[i].name;
                            }

                            int selectedIndex = System.Array.IndexOf(clipNames, property.stringValue);
                            if (selectedIndex == -1) selectedIndex = 0;

                            selectedIndex = EditorGUI.Popup(position, label.text, selectedIndex, clipNames);
                            property.stringValue = clipNames[selectedIndex];
                        }
                        else
                        {
                            EditorGUI.LabelField(position, label.text, "No animation clips found");
                        }
                    }
                    else
                    {
                        EditorGUI.LabelField(position, label.text, $"Animator not found in {animatorField.Name}");
                    }
                }
                else
                {
                    EditorGUI.LabelField(position, label.text, "Animator field not found");
                }
            }
            else
            {
                EditorGUI.LabelField(position, label.text, "Use ExposeClipName with string");
            }
        }
    }
}