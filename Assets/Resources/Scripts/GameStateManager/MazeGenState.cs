using UnityEngine;
using System.Collections;

public class MazeGenState : State
{
    //Sets reference to the game object
    GameObject MazeGenerator;

    public GameStateManager stateManager;

    public MazeGenState(string a_stringName) : base(a_stringName)
    {
        Debug.Log("Current state: MazeGenState");

        m_fDuration = -1;
        Process = Initialise;
    }

    protected override void Initialise(float a_fTimeStep)
    {
        Debug.Log("MazeGenState Initialise");

        // Load the prefab for each game object
        MazeGenerator = Resources.Load("Prefabs/MazeGen/MazeGenerator", typeof(GameObject)) as GameObject;

        Camera.main.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0)); // Alters main camera rotation
        Camera.main.transform.position = new Vector3(0, 25, 0); // Alters main camera position

        // Create an instance of the game object
        GameObject.Instantiate(MazeGenerator);
        
        Process = Update;
    }

    protected override void Update(float a_fTimeStep)
    {
        Debug.Log("MazeGenState Update");

        if (this != GameStateManager.Instance.CurrentState)
        {
            Process = Leave;
        }
    }

    //=============================================================================================
    // When a state is left, destroy all game object instances
    //=============================================================================================
    protected override void Leave(float a_fTimeStep)
    {
        Debug.Log("MazeGenState Leave");

        if (GameObject.Find("MazeGenerator(Clone)"))
        {
            GameObject.Destroy(GameObject.Find("MazeGenerator(Clone)"));
        }

        if (GameObject.Find("Maze(Clone)"))
        {
            GameObject.Destroy(GameObject.Find("Maze(Clone)"));
        }

        if (this == GameStateManager.Instance.CurrentState)
        {
            Process = Initialise;
        }
    }
}


