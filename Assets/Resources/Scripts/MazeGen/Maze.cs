using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Maze : MonoBehaviour
{
    // Alters width/ length of grid generated (accessed in Unity editor) instead of using two integers
    public IntVector2 size;

    // Reference to the cell, wall and passage prefabs so instances can be created of them
    public MazeCell cellPrefab;
    public MazePassage passagePrefab;
    public MazeWall wallPrefab;

    private MazeCell[,] cells; // Stores the cells in a 2d array

    //public float generationStepDelay; // Alters the step delay for the generation process

    //=================================================================================
    // Retrieves the maze's cell at a coordinate to guard against revisiting the same
    // cell more than once.
    //=================================================================================
    public MazeCell GetCell (IntVector2 coordinates)
    {
        return cells[coordinates.x, coordinates.z];
    }

    //=================================================================================
    // Generate function takes care of constructing the maze contents.
    //=================================================================================
    public void Generate()
    {
        //Adds delay to see how the maze is generated
        //WaitForSeconds delay = new WaitForSeconds(generationStepDelay); 

        // Create new maze cell
        cells = new MazeCell[size.x, size.z];

        List<MazeCell> activeCells = new List<MazeCell>(); // Create a temporary list of cells active
        DoFirstGenerationStep(activeCells); // access DoFirstGenerationStep function in Maze.cs

        while (activeCells.Count > 0)
        {
            //yield return delay; // delay implemented each time "while" loop accessed
            DoNextGenerationStep(activeCells); // access DoNextGenerationStep function in Maze.cs
        }
    }

    //=================================================================================
    // DoFirstGenerationStep retrieves the first cell
    //=================================================================================
    private void DoFirstGenerationStep (List<MazeCell> activeCells)
    {
        activeCells.Add(CreateCell(RandomCoordinates));
    }

    //=================================================================================
    // DoNextGenerationStep Retrieves the current cell, checks whether it can move on
    // to the next cell and removes cells from the list if the move isn't possible
    //=================================================================================
    private void DoNextGenerationStep (List<MazeCell> activeCells)
    {
        int currentIndex = activeCells.Count - 1;
        MazeCell currentCell = activeCells[currentIndex];

        //if all of a cells neighbours have been visited, removes cell from the active list
        if (currentCell.IsFullyInitialised)
        {
            activeCells.RemoveAt(currentIndex);
            return;
        }

        MazeDirection direction = currentCell.RandomUninitialisedDirection;
        IntVector2 coordinates = currentCell.coordinates + direction.ToIntVector2();

        //if the coordinate found in Contains coordinate is within the bounds of the maze
        if (ContainsCoordinates(coordinates)) 
        {
            MazeCell neighbour = GetCell(coordinates);
            if (neighbour == null) //if the cell is empty, create a passage
            {
                neighbour = CreateCell(coordinates); //creates the cell
                CreatePassage(currentCell, neighbour, direction); //creates passage using the CreatePassage function
                activeCells.Add(neighbour); //sets the cell as active
            }
            else //if cell is occupied
            {
                CreateWall(currentCell, neighbour, direction); //creates wall using CreateWall function
            }
        }
        else
        {
            CreateWall(currentCell, null, direction); //creates wall using CreateWall function
        }
    }

    //=================================================================================
    // Function to create the passages in the maze
    //=================================================================================
    private void CreatePassage (MazeCell cell, MazeCell otherCell, MazeDirection direction)
    {
        MazePassage passage = Instantiate(passagePrefab) as MazePassage; // adds an instance of the passage prefab
        passage.Initialise(cell, otherCell, direction);
        passage = Instantiate(passagePrefab) as MazePassage;
        passage.Initialise(otherCell, cell, direction.GetOpposite());
    }

    //=================================================================================
    // Function to create the walls of the maze
    //=================================================================================
    private void CreateWall (MazeCell cell, MazeCell otherCell, MazeDirection direction)
    {
        MazeWall wall = Instantiate(wallPrefab) as MazeWall;  // adds an instance of the wall prefab
        wall.Initialise(cell, otherCell, direction);
        if (otherCell != null)
        {
            wall = Instantiate(wallPrefab) as MazeWall; 
            wall.Initialise(otherCell, cell, direction.GetOpposite());
        }

    }

    //=================================================================================
    // Function creates the cells that contain the passages and walls
    //=================================================================================
    private MazeCell CreateCell(IntVector2 coordinates)
    {
        MazeCell newCell = Instantiate(cellPrefab) as MazeCell;
        cells[coordinates.x, coordinates.z] = newCell;
        newCell.coordinates = coordinates;
        newCell.name = "Maze Cell " + coordinates.x + ", " + coordinates.z;
        newCell.transform.parent = transform;
        newCell.transform.localPosition = new Vector3(coordinates.x - size.x * 0.5f + 0.5f, 0f, coordinates.z - size.z * 0.5f + 0.5f);
        return newCell;
    }

    //=================================================================================
    // Produces random coordinates within the function
    //=================================================================================
    public IntVector2 RandomCoordinates
    {
        get
        {
            return new IntVector2(Random.Range(0, size.x), Random.Range(0, size.z));
        }
    }

    //=================================================================================
    // Checks whether the coordinates fall inside the bounds of the maze
    //=================================================================================
    public bool ContainsCoordinates (IntVector2 coordinate)
    {
        return coordinate.x >= 0 && coordinate.x < size.x && coordinate.z >= 0 && coordinate.z < size.z;
    }
}
