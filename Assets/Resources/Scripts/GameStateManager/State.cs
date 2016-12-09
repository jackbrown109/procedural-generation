using UnityEngine;
using System.Collections;

//==========================================================
// State class represents a State the game may currently be in
//==========================================================

public class State
{
    // StateName property used to retrieve this data
    protected string m_StateName;
    public string StateName
    {
        get { return m_StateName; }
    }

    // m_fDuration- the length of time this state is active for
    // Duration is the property for this data
    protected float m_fDuration;
    public float Duration
    {
        get { return m_fDuration; }
    }

    // _isBlocking boolean value to represent whether this state allows other states
    //below it on the state stack to process or if they are blocked.
    private bool _isBlocking = false;
    public bool IsBlocking
    {
        get { return _isBlocking; }
        set { _isBlocking = value; }
    }

    // Constructor to create a state with a given name
    public State(string a_stateName)
    {
        m_StateName = a_stateName;
    }

    //Virtual functions that are to be used for processing the current process of the state
    protected virtual void Initialise(float a_fTimeStep) { }
    protected virtual void Update(float a_fTimeStep) { }
    protected virtual void Leave(float a_fTimeStep) { }

    //StateProcess is a function descriptor that allows us to have a function pointer that
    // can point to a function which has an identical argument list
    public delegate void StateProcess(float a_fDeltaTime);

    //m_stateProcess is the function pointer variable that the property Process will both
    //set and retrieve
    private StateProcess m_stateProcess;
    public StateProcess Process
    {
        get { return m_stateProcess; }
        set { m_stateProcess = value; }
    }
}
