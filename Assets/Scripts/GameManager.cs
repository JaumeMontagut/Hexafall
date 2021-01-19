using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public List<GameObject> players;

    void Awake()
    {
        Managers.Game = this;
    }
}
