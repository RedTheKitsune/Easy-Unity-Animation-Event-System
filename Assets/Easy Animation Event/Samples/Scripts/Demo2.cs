/*
 * Script created by Ahmed Amine Laaraf
 * Date: June 12, 2024
 */

using UnityEngine;
using UnityEngine.UI;

namespace EasyAnimationEvent
{
    public class Demo2 : MonoBehaviour
    {
        /// The Animator component to manage animation events for.
        public Animator MyAnimator;

        [ExposeClipName("MyAnimator")]
        public string clipName;

        [PreviewClip("MyAnimator", "clipName")]
        [SerializeField] private float firstEventTime;

        [PreviewClip("MyAnimator", "clipName")]
        [SerializeField] private float secondEventTime;

        [PreviewClip("MyAnimator", "clipName")]
        [SerializeField] private float thirdEventTime;

        [ExposeClipName("MyAnimator")]
        public string clipName2;
        [PreviewClip("MyAnimator", "clipName2")]
        [SerializeField] private float EventTimeClip2;

        [ExposeClipName("MyAnimator")]
        public string clipName3;
        [PreviewClip("MyAnimator", "clipName3")]
        [SerializeField] private float EventTimeClip3;

        [SerializeField] public Text display;

        private void Start()
        {
            FireWorkEvent();
        }
        private void FireWorkEvent()
        {
            if (MyAnimator == null)
            {
                Debug.LogWarning("Animator component is not assigned.");
                return;
            }


            MyAnimator.AddEvent(clipName, firstEventTime, "FirstFirework", () => DisplayMessage(clipName, firstEventTime));
            MyAnimator.AddEvent(clipName, secondEventTime, "SecondFirework", () => DisplayMessage(clipName, secondEventTime));
            MyAnimator.AddEvent(clipName, thirdEventTime, "ThirdFirework", () => DisplayMessage(clipName, thirdEventTime));


            MyAnimator.AddEvent(clipName2, EventTimeClip2, "EventTimeClip2", () => DisplayMessage(clipName2, EventTimeClip2));

            MyAnimator.AddEvent(clipName3, EventTimeClip3, "EventTimeClip3", () => DisplayMessage(clipName3, EventTimeClip3));
        }
        private void DisplayMessage(string clipName, float time)
        {
            if (display != null)
            {
                display.text = $"{clipName} event triggered at: {time}";
            }
        }
        [ContextMenu("RemoveFirework")]
        public void RemoveFirework()
        {
            MyAnimator.RemoveEvent(clipName, firstEventTime, "FirstFirework", () => DisplayMessage(clipName, firstEventTime));
            MyAnimator.RemoveEvent(clipName, secondEventTime, "SecondFirework", () => DisplayMessage(clipName, secondEventTime));
            MyAnimator.RemoveEvent(clipName, thirdEventTime, "ThirdFirework", () => DisplayMessage(clipName, thirdEventTime));


            MyAnimator.RemoveEvent(clipName2, EventTimeClip2, "EventTimeClip2", () => DisplayMessage(clipName2, EventTimeClip2));

            MyAnimator.RemoveEvent(clipName3, EventTimeClip3, "EventTimeClip3", () => DisplayMessage(clipName3, EventTimeClip3));
        }
    }
}