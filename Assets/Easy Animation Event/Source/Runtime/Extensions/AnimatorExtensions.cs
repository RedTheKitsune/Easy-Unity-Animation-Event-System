/*
 * Script created by Ahmed Amine Laaraf
 * Date: June 12, 2024
 */

using System;
using UnityEngine;

namespace EasyAnimationEvent
{
    /// <summary>
    /// Extension methods for adding and removing events to animation clips within an Animator component.
    /// </summary>
    /// <author>Author: [Your Name]</author>
    public static class AnimatorEventExtensions
    {
        /// <summary>
        /// Adds a Event to be invoked at a specific time within the specified animation clip.
        /// </summary>
        /// <param name="animator">The Animator instance.</param>
        /// <param name="clipName">The name of the animation clip.</param>
        /// <param name="time">The time in seconds within the clip to invoke the Event.</param>
        /// <param name="methodName">The unique name of the callback.</param>
        /// <param name="onClipTimeReached">The Event to invoke at the specified time.</param>
        public static void AddEvent(this Animator animator, string clipName, float time, string methodName, Action onClipTimeReached)
        {
            var clip = animator.GetAnimationClipByName(clipName);
            if (clip == null)
            {
                Debug.LogWarning($"Failed to find animation clip '{clipName}' in animator {animator.name}.");
                return;
            }

            clip.BindEvent(animator.gameObject, time, methodName, onClipTimeReached);
        }

        /// <summary>
        /// Removes a Event from a specific time within the specified animation clip.
        /// </summary>
        /// <param name="animator">The Animator instance.</param>
        /// <param name="clipName">The name of the animation clip.</param>
        /// <param name="time">The time in seconds within the clip to remove the Event.</param>
        /// <param name="methodName">The unique name of the callback.</param> 
        /// <param name="onClipTimeReached">The Event to remove from the specified time.</param>
        public static void RemoveEvent(this Animator animator, string clipName, float time, string methodName, Action onClipTimeReached)
        {
            var clip = animator.GetAnimationClipByName(clipName);
            if (clip == null)
            {
                Debug.LogWarning($"Failed to find animation clip '{clipName}' in animator {animator.name}.");
                return;
            }

            clip.UnbindEvent(animator.gameObject, time, methodName, onClipTimeReached);
        }

        /// <summary>
        /// Retrieves the animation clip by name from a specified layer of the Animator.
        /// </summary>
        /// <param name="animator">The Animator instance.</param>
        /// <param name="clipName">The name of the animation clip.</param>
        /// <returns>The AnimationClip if found; otherwise, null.</returns>
        private static AnimationClip GetAnimationClipByName(this Animator animator, string clipName)
        {

            var animationClips = animator.runtimeAnimatorController.animationClips;

            var clipInfo = Array.Find(animationClips, clip =>
                 clip.name.Equals(clipName, StringComparison.OrdinalIgnoreCase)
            );

            if (clipInfo == null)
            {
                Debug.LogWarning($"Clip '{clipName}' not found in animator {animator.name}.");
            }
            return clipInfo;
        }
    }
}
