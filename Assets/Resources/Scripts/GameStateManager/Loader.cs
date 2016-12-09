using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour
{
    public GameStateManager stateManager;
    public Updater UpdateController;

    // Use this for initialization
    void Awake()
    {
        stateManager = GameStateManager.Create();
        if (stateManager != null)
        {
            //Register game states here.
            //stateManager.RegisterState<SimpleState>("SimpleState");
            stateManager.RegisterState<GeometryGenState>("GeometryGen");
            stateManager.RegisterState<TerrainGenState>("TerrainGen");
            stateManager.RegisterState<MazeGenState>("MazeGen");
            stateManager.RegisterState<PathfindingState>("Pathfinding");


            //stateManager.EnterState("SimpleState");
        }

        Instantiate(UpdateController);

    }

    public void StateChange(int stateName)
    {
        switch(stateName)
        {
            case 0:
                Debug.Log("Changing state");
                stateManager.EnterState("GeometryGen");
                break;

            case 1:
                Debug.Log("Changing state");
                stateManager.EnterState("TerrainGen");
                break;

            case 2:
                Debug.Log("Changing state");
                stateManager.EnterState("MazeGen");
                break;

            case 3:
                Debug.Log("Changing state");
                stateManager.EnterState("Pathfinding");
                break;

        }
    }
}