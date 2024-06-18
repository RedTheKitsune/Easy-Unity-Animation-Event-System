/*
 * Script created by Ahmed Amine Laaraf
 * Date: June 12, 2024
 */

using UnityEngine.Events;
using System;
using UnityEngine;

namespace EasyAnimationEvent
{
    [Serializable]
    public class AnimationEventData
    {
        public string clipName;
        public float eventTime;
        public UnityEvent onAnimationEvent;

        /// <summary>
        /// Checks whether all fields are properly initialized.
        /// </summary>
        /// <returns>True if all fields are valid; otherwise, false.</returns>
        public bool IsValid()
        {
            bool isValidClipName = !string.IsNullOrEmpty(clipName);
            bool isValidEventTime = eventTime >= 0f; // Define what constitutes a valid event time for your application
            bool isValidOnAnimationEvent = onAnimationEvent != null;

            if (!isValidClipName)
            {
                Debug.LogWarning("clipName is null or empty.");
            }
            if (!isValidEventTime)
            {
                Debug.LogWarning("eventTime is invalid.");
            }
            if (!isValidOnAnimationEvent)
            {
                Debug.LogWarning("onAnimationEvent is null.");
            }

            return isValidClipName && isValidEventTime && isValidOnAnimationEvent;
        }
    }
}