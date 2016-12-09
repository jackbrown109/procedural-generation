using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour
{
    public GameStateManager stateManager;
    public Updater UpdateController;

    
    void Awake()
    {
        stateManager = GameStateManager.Create();
        if (stateManager != null)
        {
            // Game states registered
            stateManager.RegisterState<GeometryGenState>("GeometryGen"); 
            stateManager.RegisterState<TerrainGenState>("TerrainGen");
            stateManager.RegisterState<MazeGenState>("MazeGen");
            stateManager.RegisterState<PathfindingState>("Pathfinding");

            
        }

        Instantiate(UpdateController); // Creates an instance of the Update controller

    }
    //=============================================================================================
    // Declares the different states to switch between
    //=============================================================================================
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