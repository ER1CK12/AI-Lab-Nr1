using System;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public enum States
    {
        idle = 0,
        patrol = 1,
        chase = 2,
        attack = 3,
        search = 4
    }

    public States currentState = States.idle;
}
