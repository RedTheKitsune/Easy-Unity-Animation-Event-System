/*
 * Script created by Ahmed Amine Laaraf
 * Date: June 12, 2024
 */

using UnityEngine;

namespace EasyAnimationEvent
{
    public class ExposeClipNameAttribute : PropertyAttribute
    {
        public string animatorFieldName;

        public ExposeClipNameAttribute(string animatorFieldName)
        {
            this.animatorFieldName = animatorFieldName;
        }
    }
}
