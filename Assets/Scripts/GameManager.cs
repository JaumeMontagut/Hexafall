using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyEvents;
public class GameManager : MonoBehaviour
{
    [HideInInspector] public List<GameObject> players;

    void Awake()
    {
        Managers.Game = this;
    }

    private void Update()
    {
        EventManager.TriggerEvent(MyEventType.PlayerReachGoal, null);
    }
}
