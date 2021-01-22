using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyEvents;

public class AudioManager : MonoBehaviour
{
    private void Awake()
    {
        Managers.Audio = this;
    }
}
