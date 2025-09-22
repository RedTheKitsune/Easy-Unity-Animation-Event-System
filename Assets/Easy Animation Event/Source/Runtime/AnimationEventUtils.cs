using UnityEngine;

namespace EasyAnimationEvent
{
    public class AnimationEventUtils
    {
        public static string ConstructKey(AnimationClip clip, float time) => $"{clip.name}_{time}";
    }
}