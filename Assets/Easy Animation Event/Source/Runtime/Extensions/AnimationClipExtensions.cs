/*
 * Script created by Ahmed Amine Laaraf
 * Date: June 12, 2024
 */

using System;
using System.Linq;
using UnityEngine;

namespace EasyAnimationEvent
{
    /// <summary>
    /// Extension methods for binding and unbinding events on AnimationClip objects.
    /// </summary>
    public static class AnimationClipExtensions
    {
        private const string OnEvent = "OnEvent"; // Method name from EventReceiver

        /// <summary>
        /// Binds an event to the specified position in the animation clip.
        /// </summary>
        /// <param name="clip">The AnimationClip to bind the event to.</param>
        /// <param name="animEvtReceiverObject">The GameObject that will receive the event.</param>
        /// <param name="position">The position in the animation clip timeline to bind the event to.</param>
        /// <param name="methodName">The unique name of the callback.</param> 
        /// <param name="callback">The callback action to invoke when the event is triggered.</param>
        public static void BindEvent(this AnimationClip clip, GameObject animEvtReceiverObject, float position, string methodName, Action callback)
        {
            clip.BindOrUnbindEvent(animEvtReceiverObject, position, callback, methodName, true);
        }

        /// <summary>
        /// Unbinds an event from the specified position in the animation clip.
        /// </summary>
        /// <param name="clip">The AnimationClip to unbind the event from.</param>
        /// <param name="animEvtReceiverObject">The GameObject that should stop receiving the event.</param>
        /// <param name="position">The position in the animation clip timeline to unbind the event from.</param>
        /// <param name="methodName">The unique name of the callback.</param> 
        /// <param name="callback">The callback action to stop invoking when the event is triggered.</param>
        public static void UnbindEvent(this AnimationClip clip, GameObject animEvtReceiverObject, float position, string methodName, Action callback)
        {
            clip.BindOrUnbindEvent(animEvtReceiverObject, position, callback, methodName, false);
        }

        /// <summary>
        /// Internal method for binding or unbinding an event to/from the animation clip.
        /// </summary>
        /// <param name="clip">The AnimationClip to bind or unbind the event to/from.</param>
        /// <param name="animEvtReceiverObject">The GameObject that will receive the event.</param>
        /// <param name="position">The position in the animation clip timeline to bind/unbind the event to/from.</param>
        /// <param name="callback">The callback action to invoke/stop invoking when the event is triggered.</param>
        /// <param name="methodName">The unique name of the callback.</param> 
        /// <param name="bind">If true, binds the event; if false, unbinds the event.</param>
        private static void BindOrUnbindEvent(this AnimationClip clip, GameObject animEvtReceiverObject, float position, Action callback, string methodName, bool bind)
        {
            var actionWord = bind ? "register" : "unregister";

            // Validation checks
            if (animEvtReceiverObject == null)
            {
                Debug.LogWarningFormat("Trying to {0} callback for null animation event receiver game object", actionWord);
                return;
            }

            if (callback == null)
            {
                Debug.LogWarningFormat("Trying to {0} null callback for animation clip", actionWord);
                return;
            }

            if (position < 0.0f || position > clip.length)
            {
                Debug.LogWarningFormat("Trying to {0} callback for position outside of clip timeline", actionWord);
                return;
            }

            // Retrieve or create EventReceiver component on the GameObject
            var eventReceiver = animEvtReceiverObject.GetComponent<EventReceiver>();
            if (bind)
            {
                if (eventReceiver == null)
                {
                    eventReceiver = animEvtReceiverObject.AddComponent<EventReceiver>();
                }

                // Register the callback at the specified timeline position
                eventReceiver.RegisterEvent(position, callback, methodName);

                // Add an AnimationEvent to the AnimationClip if it doesn't already exist
                clip.AddEventIfNotExists(OnEvent, position, position);
            }
            else
            {
                if (eventReceiver == null)
                {
                    Debug.LogWarningFormat("Trying to unregister callback for game object without EventReceiver component");
                    return;
                }

                // Unregister the callback from the EventReceiver
                var lastEventForPositionRemoved = eventReceiver.UnregisterEvent(position, methodName);
                if (lastEventForPositionRemoved)
                {
                    // Remove the AnimationEvent from the AnimationClip if it was the last event at that position
                    clip.RemoveEvent(OnEvent, position, position);
                }
            }
        }

        /// <summary>
        /// Adds an AnimationEvent to the AnimationClip if it does not already exist.
        /// </summary>
        /// <param name="clip">The AnimationClip to add the event to.</param>
        /// <param name="methodName">The method name associated with the event.</param>
        /// <param name="floatParameter">The float parameter associated with the event.</param>
        /// <param name="time">The time in the animation clip timeline associated with the event.</param>
        private static void AddEventIfNotExists(this AnimationClip clip, string methodName, float floatParameter, float time)
        {
            var clipAnimationEvents = clip.events;
            var animationEvent = Array.Find(clipAnimationEvents,
                e => e.functionName == methodName && e.floatParameter == floatParameter && e.time == time);

            if (animationEvent == null)
            {
                // Create a new AnimationEvent and add it to the AnimationClip
                animationEvent = new AnimationEvent();
                animationEvent.functionName = methodName;
                animationEvent.floatParameter = floatParameter;
                animationEvent.time = time;
                clip.AddEvent(animationEvent);
            }
        }

        /// <summary>
        /// Removes an AnimationEvent from the AnimationClip.
        /// </summary>
        /// <param name="clip">The AnimationClip to remove the event from.</param>
        /// <param name="methodName">The method name associated with the event.</param>
        /// <param name="floatParameter">The float parameter associated with the event.</param>
        /// <param name="time">The time in the animation clip timeline associated with the event.</param>
        private static void RemoveEvent(this AnimationClip clip, string methodName, float floatParameter, float time)
        {
            var clipAnimationEvents = clip.events;
            var animationEventIndex = Array.FindIndex(clipAnimationEvents,
                e => e.functionName == methodName && e.floatParameter == floatParameter && e.time == time);

            if (animationEventIndex != -1)
            {
                // Remove the AnimationEvent from the array of AnimationEvents
                clipAnimationEvents = clipAnimationEvents.Where((val, idx) => idx != animationEventIndex).ToArray();
                clip.events = clipAnimationEvents;
            }
            else
            {
                Debug.LogWarningFormat("Failed to remove animation event for clip {0}", clip.name);
            }
        }
    }
}
