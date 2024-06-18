/*
 * Script created by Ahmed Amine Laaraf
 * Date: June 12, 2024
 */

using System.Linq;
using UnityEditor;
using UnityEngine;

namespace EasyAnimationEvent
{
    [CustomPropertyDrawer(typeof(PreviewClipAttribute))]
    public class PreviewClipDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 40;
        }
        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);
            var previewStates = PreviewClipAttribute.previewActiveStates;
            // Ensure the property is a string (clip name)
            if (property.propertyType == SerializedPropertyType.Float)
            {
                var targetObject = property.serializedObject.targetObject;
                var previewClipAttribute = (PreviewClipAttribute)attribute;

                // Use reflection to get the Animator
                var animatorField = targetObject.GetType().GetField(previewClipAttribute.animatorFieldName,
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                Animator animator = animatorField?.GetValue(targetObject) as Animator;

                // Use reflection to get the clip name
                var clipNameField = targetObject.GetType().GetField(previewClipAttribute.clipNameFieldName,
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                string clipName = clipNameField?.GetValue(targetObject) as string;

                // Find the AnimationClip by name
                AnimationClip clip = animator != null && !string.IsNullOrEmpty(clipName) ? FindAnimationClip(animator, clipName) : null;

                if (clip != null)
                {

                    string propertyKey = property.name; // Using property name as the key

                    // Ensure we have a state entry for this property
                    if (!previewStates.ContainsKey(propertyKey))
                    {
                        previewStates[propertyKey] = -1; // Initialize state if not present
                    }


                    float spacerHeight = EditorGUIUtility.singleLineHeight * 0.5f;
                    float height = EditorGUIUtility.singleLineHeight;

                   

                    var RectValue = new Rect(rect.x, rect.y + spacerHeight, rect.width  - 50, height);
                    property.floatValue = EditorGUI.Slider(RectValue,label, property.floatValue, 0, clip.length);

                    RectValue = new Rect(rect.width - 20, rect.y + spacerHeight - 1.9f, rect.width, height);
                    GUI.changed = false;

                    int prevIndex = previewStates[propertyKey];
                    previewStates[propertyKey] = GUI.Toolbar(RectValue, previewStates[propertyKey], new[] { EditorGUIUtility.IconContent("d_PlayButton") }, "AppCommand");

             
                    
                    if (GUI.changed)
                    {
                        // Toggle toolbarIndex off if the same button is clicked
                        if (previewStates[propertyKey] == prevIndex)
                        {
                            previewStates[propertyKey] = -1; // Deactivate if same button is clicked again

                        }
                        else
                        {
                            // Deactivate all others if a new button is clicked
                            foreach (var key in previewStates.Keys.ToList())
                            {
                                if (key != propertyKey)
                                {
                                    previewStates[key] = -1;
                                }
                            }

                        }
                    }

                    // Preview the animation at the given time
                    if (previewStates[propertyKey] > -1)
                    {
                        StartPreview(animator, clip, property.floatValue);
                    }

                }
                else
                {
                    EditorGUI.LabelField(rect, "No valid clip found");
                }
            }
            else
            {
                EditorGUI.LabelField(rect, label.text, "Use PreviewClipWithTime with string");
            }

            EditorGUI.EndProperty();
        }

        private AnimationClip FindAnimationClip(Animator animator, string clipName)
        {
            return animator.runtimeAnimatorController?.animationClips
            .FirstOrDefault(clip => clip.name == clipName);
        }

        private void StartPreview(Animator animator, AnimationClip clip, float time)
        {
            if (clip != null)
            {
                AnimationMode.StartAnimationMode();
                AnimationMode.BeginSampling();
                AnimationMode.SampleAnimationClip(animator.gameObject, clip, time);
                AnimationMode.EndSampling();
            }
        }
    }
}