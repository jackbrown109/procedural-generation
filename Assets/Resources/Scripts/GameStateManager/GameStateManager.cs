using UnityEngine;
using System;
using System.Collections.Generic;

//=============================================================================================
// GameStateManager gives access to the Create, Instance and Destroy functionality from the
// manager class.
//=============================================================================================
public class GameStateManager : Manager <GameStateManager>
{
    private State _currentState = null;

    // Keeps track of the game states the game might currently be in
    private Stack<State> m_pActiveStates;

    State temp;

    // Keeps track of registered states
    private Dictionary<string, Type>registeredStates;

    public State CurrentState
    {
        get { return _currentState; }
    }

    //=============================================================================================
    // Private constructor so that only the Manager Base Class can create an instance of this object
    //=============================================================================================
    private GameStateManager()
    {
        m_pActiveStates = new Stack<State>();
    }

    protected override void Terminate()
    {
        
    }

    //=============================================================================================
    // Update function so the GameStateManager is updated every frame.
    //=============================================================================================
    public void Update()
    {
        float deltaTime = Time.deltaTime;

        // For each state currently in m_pActiveStates
        foreach (State state in m_pActiveStates)
        {
            if (m_pActiveStates.Count > 0)
            {
                state.Process.Invoke(deltaTime); // invokes the process the state needs to execute
                if (state.IsBlocking)
                {
                    break;
                }
            }
            break;
        } 
    }

    public State StateExists(string a_stateName)
    {
        // For each state currently in m_pActiveStates
        foreach (State state in m_pActiveStates)
        {
            string pName = state.StateName;
            if (pName != null && pName == a_stateName)
            {
                return state;
            }
        }
        return null;
    }

    //=============================================================================================
    // EnterState looks through the dictionary of registered states to find the state that has 
    // been registered with the name or identifier looked for.
    //=============================================================================================
    public bool EnterState (string a_stateName)
    {
        State pState = StateExists(a_stateName);
        if (pState != null)
        {
            //Our state is already in the list of active States
            PopToState(pState);
            return true;
        }
        else
        {
            if (registeredStates.ContainsKey(a_stateName))
            {
                // Used to only allocate the memory needed at that point
                State nextState = Activator.CreateInstance(registeredStates[a_stateName], a_stateName) as State;

                PushState(nextState);
                return true;
            }
        }
        return false;
    }

    private void PopToState(State a_state)
    {
        temp = m_pActiveStates.Peek();

        while(m_pActiveStates.Count != 0 && m_pActiveStates.Peek() != a_state)
        {
            m_pActiveStates.Pop();
        }

        _currentState = m_pActiveStates.Peek();

        temp.Process.Invoke(0.0f);
        temp.Process.Invoke(0.0f);
        temp = null;


    }

    private void PushState (State a_state)
    {
        if (m_pActiveStates.Count != 0)
        {
            temp = m_pActiveStates.Peek();
        }

        m_pActiveStates.Push(a_state);
        _currentState = m_pActiveStates.Peek();

        if (temp != null)
        {
            temp.Process.Invoke(0.0f);
            temp.Process.Invoke(0.0f);
            temp = null;
        }
    }

    private void PopState()
    {
        m_pActiveStates.Pop();
        _currentState = m_pActiveStates.Peek();
    }

    //=============================================================================================
    // Holds type information rather than instances of states to save on memory space.
    //=============================================================================================
    public void RegisterState<T> (string a_stateName)
        where T : State
    {
        if (registeredStates == null)
        {
            registeredStates = new Dictionary<string, Type>();
        }
        registeredStates.Add(a_stateName, typeof(T));
    }
}
