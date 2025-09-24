/*
 * Script created by Ahmed Amine Laaraf
 * Date: June 12, 2024
 */

using System.Collections.Generic;
using UnityEngine;

namespace EasyAnimationEvent
{
    public class AnimationEventHandler : MonoBehaviour
    {
        /// The Animator component to manage animation events for.
        public Animator animator;
        /// List of AnimationEventData instances representing animation events to manage.
        public List<AnimationEventData> animationEvents = new List<AnimationEventData>();

        public bool initializeOnAwake = true;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            
            if(!initializeOnAwake) return;
            
            AddEvents();
        }

        /// <summary>
        /// Adds all animation events from animationEvents to the Animator component
        /// </summary>
        public void AddEvents()
        {
            if (animator == null)
            {
                Debug.LogWarning("Animator component is not assigned.");
                return;
            }

            foreach (var animationEvent in animationEvents)
            {
                if (!animationEvent.IsValid())
                    return;

                var eventTime = animationEvent.eventTime;
                var clipName = animationEvent.clipName.ToString();
                var onAnimationEvent = animationEvent.onAnimationEvent;


                animator.AddEvent(clipName, eventTime, "AnimUnityEvent", () => onAnimationEvent?.Invoke());
            }
        }
        /// <summary>
        /// Removes all animation events from animationEvents from the Animator component.
        /// </summary> 
        public void RemoveEvents()
        {
            if (animator == null)
            {
                Debug.LogWarning("Animator component is not assigned.");
                return;
            }

            foreach (var animationEvent in animationEvents)
            {
                if (!animationEvent.IsValid())
                    return;

                var eventTime = animationEvent.eventTime;
                var clipName = animationEvent.clipName.ToString();
                var onAnimationEvent = animationEvent.onAnimationEvent;


                animator.RemoveEvent(clipName, eventTime, "AnimUnityEvent", () => onAnimationEvent?.Invoke());
            }
        }
    }
}
