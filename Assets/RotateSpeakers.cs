﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSpeakers : MonoBehaviour
{
    public float rotateSpeed;

    void Update()
    {
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
    }
}
