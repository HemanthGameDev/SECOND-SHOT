using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ReplayFrame
{
    public float time;
    public Vector2 moveInput;
    public Vector3 aimDirection;
    public bool shoot;
}

[Serializable]
public class ReplayData
{
    public Vector3 startPosition;
    public float duration;
    public List<ReplayFrame> frames = new List<ReplayFrame>();
    
}