/*
 * Script created by Ahmed Amine Laaraf
 * Date: June 12, 2024
 */

using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace EasyAnimationEvent
{
    [CustomEditor(typeof(AnimationEventHandler))]
    public class AnimationEventHandlerEditor : Editor
    {
        private ReorderableList reorderableList;
        private AnimationClip[] animationClips;
        private string[] ClipNames;
        private float fieldHeight;
        private float fieldSpacing;
        private List<int> toolbarStates; // List to track toolbar states
        private Animator animator;
        private int activeIndex = -1; // Track active index for preview

        private void OnEnable()
        {
            fieldHeight = EditorGUIUtility.singleLineHeight;
            fieldSpacing = fieldHeight + 10;
            toolbarStates = new List<int>();

            reorderableList = new ReorderableList(serializedObject,
                serializedObject.FindProperty("animationEvents"),
                true, true, true, true);

            reorderableList.drawHeaderCallback = (Rect rect) =>
            {
                EditorGUI.LabelField(rect, "");
            };

            reorderableList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                if (toolbarStates.Count <= index) { toolbarStates.Add(-1); }

                SerializedProperty element = reorderableList.serializedProperty.GetArrayElementAtIndex(index);


                SerializedProperty clipNameProperty = element.FindPropertyRelative("clipName");
                SerializedProperty eventTime = element.FindPropertyRelative("eventTime");
                SerializedProperty onAnimationEvent = element.FindPropertyRelative("onAnimationEvent");

                EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width, fieldHeight), "Animation Event");



                // Clip Name Popup
                int selectedClipIndex = System.Array.IndexOf(ClipNames, clipNameProperty.stringValue);
                var RectValue = new Rect(rect.x, rect.y + fieldSpacing, rect.width, fieldHeight);

                selectedClipIndex = EditorGUI.Popup(RectValue, clipNameProperty.displayName, selectedClipIndex, ClipNames, EditorStyles.popup);
                // save value 
                if (selectedClipIndex >= 0)
                {
                    clipNameProperty.stringValue = ClipNames[selectedClipIndex];
                }

                EditorGUI.BeginChangeCheck();
                float AnimationDuration = animationClips[selectedClipIndex].length;
                // Event Time Field
                RectValue = new Rect(rect.x, rect.y + fieldSpacing * 2, rect.width - fieldSpacing - 20, fieldHeight);
                eventTime.floatValue = EditorGUI.Slider(RectValue, "Event Time", eventTime.floatValue, 0, AnimationDuration);
                if (EditorGUI.EndChangeCheck())
                {
                    if (activeIndex == index)
                    {
                        StartPreview(animationClips[selectedClipIndex], eventTime.floatValue);
                    }
                }

                // Preview Toggle
                RectValue = new Rect(rect.width, rect.y + fieldSpacing * 1.9f, rect.width, fieldHeight);
                GUI.changed = false; // Reset GUI.changed to detect toolbar changes only
                int prevIndex = toolbarStates[index];
                toolbarStates[index] = GUI.Toolbar(RectValue, toolbarStates[index], new[] { EditorGUIUtility.IconContent("d_PlayButton") }, "AppCommand");

                if (GUI.changed)
                {
                    // Toggle toolbarIndex off if the same button is clicked
                    if (toolbarStates[index] == prevIndex)
                    {
                        toolbarStates[index] = -1; // Deactivate if same button is clicked again
                        activeIndex = -1;
                    }
                    else
                    {
                        // Deactivate all others if a new button is clicked
                        for (int i = 0; i < toolbarStates.Count; i++)
                        {
                            if (i != index)
                            {
                                toolbarStates[i] = -1;
                            }
                        }
                        activeIndex = index;
                    }
                }

                // On Animation Event Field
                RectValue = new Rect(rect.x, rect.y + fieldSpacing * 3, rect.width, fieldHeight);
                EditorGUI.PropertyField(RectValue, onAnimationEvent);
            };

            reorderableList.elementHeightCallback = (int index) =>
             {
                 SerializedProperty element = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
                 SerializedProperty endProperty = element.GetEndProperty();
                 float totalHeight = 0f;

                 // Initialize the property for iteration
                 SerializedProperty currentProperty = element.Copy();
                 currentProperty.NextVisible(true); // Move to the first child

                 // Iterate through all child properties until we reach the end property
                 while (!SerializedProperty.EqualContents(currentProperty, endProperty))
                 {
                     totalHeight += EditorGUI.GetPropertyHeight(currentProperty, true);
                     currentProperty.NextVisible(false); // Move to the next sibling property
                 }

                 // Add some padding if needed
                 totalHeight += EditorGUIUtility.singleLineHeight * 3; // not sure why

                 return totalHeight;


             };

            reorderableList.onAddCallback = (ReorderableList list) =>
            {
                list.serializedProperty.arraySize++;
                list.index = list.serializedProperty.arraySize - 1;
            };
        }
        private void StartPreview(AnimationClip clip, float time)
        {
            if (clip != null)
            {
                AnimationMode.StartAnimationMode();
                AnimationMode.BeginSampling();
                AnimationMode.SampleAnimationClip(animator.gameObject, clip, time);
                AnimationMode.EndSampling();
            }
        }
        private void StopPreview()
        {
            AnimationMode.StopAnimationMode();
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var script = (AnimationEventHandler)target;
            animator = script.GetComponent<Animator>();
            // Center and show the text in big white font
            GUIStyle style = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 24,
                fontStyle = FontStyle.Bold,
                normal = new GUIStyleState { textColor = Color.white }
            };
            GUIContent iconContent = EditorGUIUtility.IconContent("Animation.AddEvent");

            GUILayout.Space(20); // Add some spacing
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace(); // Add flexible space to center the content


            // Draw the text
            GUILayout.Label("Animation Event", style, GUILayout.Height(32));
            // Draw the icon
            GUILayout.Label(iconContent, GUILayout.Width(32), GUILayout.Height(32));

            GUILayout.FlexibleSpace(); // Add flexible space to center the content
            GUILayout.EndHorizontal();

            GUILayout.Space(10); // Add some more spacing

            if (animator != null && animator.runtimeAnimatorController != null)
            {
                animationClips = animator.runtimeAnimatorController.animationClips;
                if (animationClips.Length > 0)
                {
                    ClipNames = animationClips.Select(x => x.name).ToArray();

                    reorderableList.DoLayoutList();

                }
                else
                {
                    EditorGUILayout.HelpBox("Animator doesn't have clips!.", MessageType.Warning);
                }

            }
            else
            {
                EditorGUILayout.HelpBox("Animator component or runtime controller is missing.", MessageType.Warning);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}