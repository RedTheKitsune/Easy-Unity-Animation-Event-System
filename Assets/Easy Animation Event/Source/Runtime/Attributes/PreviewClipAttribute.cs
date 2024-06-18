/*
 * Script created by Ahmed Amine Laaraf
 * Date: June 12, 2024
 */

using System.Collections.Generic;
using UnityEngine;

namespace EasyAnimationEvent
{
    public class PreviewClipAttribute : PropertyAttribute
    {
        public string animatorFieldName;
        public string clipNameFieldName;
        // Dictionary to store preview active states by property hash
        public static readonly Dictionary<string, int> previewActiveStates = new Dictionary<string, int>();
        public PreviewClipAttribute(string animatorFieldName, string clipNameFieldName)
        {
            this.animatorFieldName = animatorFieldName;
            this.clipNameFieldName = clipNameFieldName;
        }
    }
}
