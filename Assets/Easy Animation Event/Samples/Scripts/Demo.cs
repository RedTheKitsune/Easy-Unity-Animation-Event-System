/*
 * Script created by Ahmed Amine Laaraf
 * Date: June 12, 2024
 */

using UnityEngine;
using UnityEngine.UI;

namespace EasyAnimationEvent
{
    public class Demo : MonoBehaviour
    {
        [SerializeField] public Text text;
        public void FireSomethig(string eventName)
        {
            text.text = eventName;
        }
    }
}