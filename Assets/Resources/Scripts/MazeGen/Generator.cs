using UnityEngine;
using System.Collections;


public class Generator : MonoBehaviour
{
    public Maze mazePrefab;  // Creates a reference to "Maze" prefab so it can create instances of it
    private Maze mazeInstance; // Holds the instance of the prefab

	//=================================================================================
    // Start function begins maze generation process by accessing BeginGen() function,
    // where "Maze" script and all others are accessed.
    //=================================================================================
	private void Start ()
    {
        BeginGen(); // access BeginGen() function in Generator.cs
	}

    //=================================================================================
    // Check for input every frame. If space key is down, restart the maze generation.
    //=================================================================================
    private void Update ()
    {
        //Use the space key to restart the generation of the maze
	    if (Input.GetKeyDown (KeyCode.Space))
        {
            RestartGen(); //access RestartGen() function in Generator.cs
        }
	}

    //=================================================================================
    // BeginGen function creates an instance of the Maze prefab, which contains the
    // "Maze" script which accesses other scripts required.
    //=================================================================================
    private void BeginGen()
    {
        mazeInstance = Instantiate(mazePrefab) as Maze; // create instance of mazePrefab
        /*StartCoroutine(*/mazeInstance.Generate()/*)*/; //starts the generation of the maze found in "Maze" script
    }

    //=================================================================================
    // RestartGen function stops coroutines, destroys previously instantiated game 
    // objects and jumps back to the BeginGen() function.
    //=================================================================================
    private void RestartGen()
    {
        //StopAllCoroutines(); //stops coroutine of maze generation so it can be interrupted mid generation
        Destroy(mazeInstance.gameObject); // Destroy previous maze instance
        BeginGen(); // Jump back to begin function to rebuild maze
    }
}
