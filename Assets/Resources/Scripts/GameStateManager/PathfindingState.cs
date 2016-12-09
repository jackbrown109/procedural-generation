using UnityEngine;
using System.Collections;

public class PathfindingState : State
{
    GameObject Pathfinding;
    GameObject MazeGenerator;
    GameObject start;
    GameObject end;

    public GameStateManager stateManager;

    public PathfindingState(string a_stringName) : base(a_stringName)
    {
        Debug.Log("Current state: PathfindingState");

        m_fDuration = -1;
        Process = Initialise;
    }

    protected override void Initialise(float a_fTimeStep)
    {
        Debug.Log("PathfindingState Initialise");

        MazeGenerator = Resources.Load("Prefabs/MazeGen/MazeGenerator", typeof(GameObject)) as GameObject;
        Pathfinding = Resources.Load("Prefabs/Pathfinding/AStar", typeof(GameObject)) as GameObject;
        start = Resources.Load("Prefabs/Pathfinding/Spawn", typeof(GameObject)) as GameObject;
        end = Resources.Load("Prefabs/Pathfinding/Finish", typeof(GameObject)) as GameObject;

        Camera.main.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
        Camera.main.transform.position = new Vector3(0, 25, 0);

        GameObject.Instantiate(Pathfinding);
        GameObject.Instantiate(MazeGenerator);
        GameObject.Instantiate(start);
        GameObject.Instantiate(end);

        Process = Update;
    }

    protected override void Update(float a_fTimeStep)
    {
        Debug.Log("PathfindingState Update");

        if (this != GameStateManager.Instance.CurrentState)
        {
            Process = Leave;
        }
    }

    protected override void Leave(float a_fTimeStep)
    {
        Debug.Log("PathfindingState Leave");

        if (GameObject.Find("AStar(Clone)"))
        {
            GameObject.Destroy(GameObject.Find("AStar(Clone)"));
        }

        if (GameObject.Find("MazeGenerator(Clone)"))
        {
            GameObject.Destroy(GameObject.Find("MazeGenerator(Clone)"));
        }

        if (GameObject.Find("Maze(Clone)"))
        {
            GameObject.Destroy(GameObject.Find("Maze(Clone)"));
        }

        if (GameObject.Find("Spawn(Clone)"))
        {
            GameObject.Destroy(GameObject.Find("Spawn(Clone)"));
        }

        if (GameObject.Find("Finish(Clone)"))
        {
            GameObject.Destroy(GameObject.Find("Finish(Clone)"));
        }


        if (this == GameStateManager.Instance.CurrentState)
        {
            Process = Initialise;
        }
    }
}

