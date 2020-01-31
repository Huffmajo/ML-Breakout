using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class PaddleAcademy : Academy
{

    public float paddleLength { get; private set; }

    public float ballSpeed { get; private set; }
    public float ballStartSpeed { get; private set; }

    public override void InitializeAcademy()
    {
        paddleLength = 1f;
        //transform.scale.y(paddleLength);
    
        ballSpeed = 15f;
        ballStartSpeed = 15f;

        FloatProperties.RegisterCallback("paddle_length", f =>
        {
            paddleLength = f;
        });
        
        
        FloatProperties.RegisterCallback("ballSpeed", f =>
        {
            ballSpeed = f;
        });
        
        
        FloatProperties.RegisterCallback("ballStartSpeed", f =>
        {
            ballStartSpeed = f;
        });


    
    }






}
