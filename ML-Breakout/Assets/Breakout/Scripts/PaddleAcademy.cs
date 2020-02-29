using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class PaddleAcademy : Academy
{

    public float paddleXScale { get; private set; }
    public float ballSpeed { get; private set; }

    public override void InitializeAcademy()
    {
        FloatProperties.RegisterCallback("paddleXScale", f =>
        {
            paddleXScale = f;
        });
        
        
        FloatProperties.RegisterCallback("ballSpeed", f =>
        {
            ballSpeed = f;
        });
    }
}
