//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
using MLAgents;

public class PaddleAcademy : Academy
{
    public float paddleXScale { get; private set; }
    public float ballSpeed { get; private set; }

    //Academy gets vars from .json file that can increase the difficulty for the paddle
    //training as the paddle gets better and getting rewards
    public override void InitializeAcademy()
    {   //default values
		paddleXScale = 1f;
		ballSpeed = 20f;

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
