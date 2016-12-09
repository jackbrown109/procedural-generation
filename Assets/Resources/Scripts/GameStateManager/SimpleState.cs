using UnityEngine;
using System;
using System.Collections;

public class SimpleState : State
{
    public SimpleState(string a_stringName) : base(a_stringName)
    {
        m_fDuration = -1;
        Process = Initialise;
    }

    protected override void Initialise(float a_fTimeStep)
    {
        Debug.Log("SimpleState Initialise");
        Process = Update;
    }

    protected override void Update (float a_fTimeStep)
    {
        Debug.Log("SimpleState Update");
        Process = Leave;
    }

    protected override void Leave(float a_fTimeStep)
    {
        Debug.Log("SimpleState Leave");
        Process = Initialise;
    }
}
