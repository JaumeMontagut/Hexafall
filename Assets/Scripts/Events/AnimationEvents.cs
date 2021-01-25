using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyEvents;

public class AnimationEvents : MonoBehaviour
{
    public void AnimationEvent(MyEventType evt)
    {
        EventManager.TriggerEvent(evt, gameObject);
    }
}
