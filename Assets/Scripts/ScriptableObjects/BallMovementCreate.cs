using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BallMovementCreate",menuName ="ScriptableObjects/BallMovement",order =1)]
public class BallMovementCreate : ScriptableObject
{
    [SerializeField] float groundMult;
    public float GroundMult
    {
        get { return groundMult; }
    }

    [SerializeField] float swipeMult;

    public float SwipeMult
    {
        get { return swipeMult; }
    }


    [SerializeField] float angle;
    public float Angle
    {
        get { return angle; }
    }
}
