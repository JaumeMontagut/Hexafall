using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyEvents
{
    public class EventManager
    {
        private static Dictionary<MyEventType, Action<object>> eventListeners;
        
        public static void StartListening(MyEventType eventType, Action<object> listener)
        {
            if (eventListeners == null)
            {
                eventListeners = new Dictionary<MyEventType, Action<object>>();
            }

            if (!eventListeners.ContainsKey(eventType))
            {
                eventListeners.Add(eventType, listener);
            }
            else
            {
                eventListeners[eventType] += listener;
            }
        }

        public static void StopListening(MyEventType eventType, Action<object> listener)
        {
            if (eventListeners.ContainsKey(eventType))
            {
                eventListeners[eventType] -= listener;

                if (eventListeners[eventType] == null)
                {
                    eventListeners.Remove(eventType);
                }
            }
        }

        public static void TriggerEvent(MyEventType eventType, object info, float delay = 0f )
        {
            if (delay != 0f)
            {
                TriggerEventDelay(eventType, info, delay);
            }

            if (eventListeners == null || !eventListeners.ContainsKey(eventType))
            {
                return;
            }

            eventListeners[eventType]?.Invoke(info);

        }

        private static IEnumerator TriggerEventDelay(MyEventType eventType, object info, float delay)
        {
            yield return new WaitForSeconds(delay);
            TriggerEvent(eventType, info, delay);
        }
    }
}
