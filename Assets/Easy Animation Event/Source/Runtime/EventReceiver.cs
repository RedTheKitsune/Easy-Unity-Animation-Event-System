/*
 * Script created by Ahmed Amine Laaraf
 * Date: June 12, 2024
 */

using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyAnimationEvent
{
    /// <summary>
    /// Receives animation events and manages timeline callbacks for specific positions.
    /// </summary>
    /// <author>Author: Your Name</author>
    public class EventReceiver : MonoBehaviour
    {
        // Dictionary storing callbacks and their names for each timeline position.
        private readonly Dictionary<float, List<(Action callback, string name)>> CallbacksPoll = new Dictionary<float, List<(Action callback, string name)>>();

        /// <summary>
        /// Registers a callback to be invoked at a specific timeline position.
        /// </summary>
        /// <param name="position">The timeline position at which the callback should be invoked.</param>
        /// <param name="callback">The callback to register.</param>
        /// <param name="name">The name of the callback.</param>
        public void RegisterEvent(float position, Action callback, string name)
        {
            if (callback == null)
            {
                Debug.LogWarning("Attempted to register a null callback.");
                return;
            }

            if (string.IsNullOrEmpty(name))
            {
                Debug.LogWarning("Attempted to register a callback with a null or empty name.");
                return;
            }

            if (!CallbacksPoll.ContainsKey(position))
            {
                CallbacksPoll[position] = new List<(Action callback, string name)>();
            }

            CallbacksPoll[position].Add((callback, name));
        }

        /// <summary>
        /// Unregisters a callback by its name from a specific timeline position.
        /// </summary>
        /// <param name="position">The timeline position from which the callback should be unregistered.</param>
        /// <param name="name">The name of the callback to unregister.</param>
        /// <returns>True if it was the last callback for the position and the position was removed; otherwise, false.</returns>
        public bool UnregisterEvent(float position, string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                Debug.LogWarning("Attempted to unregister a callback with a null or empty name.");
                return false;
            }

            if (!CallbacksPoll.TryGetValue(position, out var callbacks))
            {
                Debug.LogWarning($"Attempted to unregister a callback not registered at position {position}.");
                return false;
            }

            // Find the index of the callback by name
            var callbackIndex = callbacks.FindIndex(c => c.name == name);
            if (callbackIndex < 0)
            {
                Debug.LogWarning($"Failed to unregister the callback '{name}' because it was not found.");
                return false;
            }

            callbacks.RemoveAt(callbackIndex);

            if (callbacks.Count == 0)
            {
                CallbacksPoll.Remove(position);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Invoked by Unity's animation system to trigger callbacks at a specific timeline position.
        /// </summary>
        /// <param name="position">The timeline position at which to trigger callbacks.</param>
        private void OnEvent(float position)
        {
            if (!CallbacksPoll.TryGetValue(position, out var callbacks))
            {
                Debug.LogWarning($"No callbacks registered for timeline position {position}.");
                return;
            }

            ExecuteEvents(callbacks);
        }

        /// <summary>
        /// Executes the provided list of callbacks.
        /// </summary>
        /// <param name="callbacks">The list of callbacks to execute.</param>
        private void ExecuteEvents(List<(Action callback, string name)> callbacks)
        {
            // Iterate over the original count to prevent new callbacks added during execution from being invoked in this cycle.
            var initialCount = callbacks.Count;
            for (var i = 0; i < initialCount; i++)
            {
                try
                {
                    callbacks[i].callback?.Invoke();
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Error executing callback '{callbacks[i].name}': {ex}");
                }
            }
        }
    }
}
