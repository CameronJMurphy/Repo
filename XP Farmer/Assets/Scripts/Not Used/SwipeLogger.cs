﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This is not used in this project
/// </summary>
public class SwipeLogger : MonoBehaviour
{
    void Awake()
    {
        SwipeDetector.OnSwipe += SwipeDetector_OnSwipe;
    }
    private void SwipeDetector_OnSwipe(SwipeDetector.SwipeData data)
    {
        Debug.Log("Swipe in direction: " + data.Direction);
    }
}

//reference
//https://www.youtube.com/watch?v=jbFYYbu5bdc