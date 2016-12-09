using UnityEngine;

public class MazeCell : MonoBehaviour
{
    public IntVector2 coordinates; // Uses the integer vector type to add coordinates to MazeCell

    // The cells store their edges in an array
    private MazeCellEdge[] edges = new MazeCellEdge[MazeDirections.Count];

    private int initialisedEdgeCount;

    //=================================================================================
    // IsFullyInitialised checks whether a cell is fully initialised
    //=================================================================================
    public bool IsFullyInitialised
    {
        get
        {
            return initialisedEdgeCount == MazeDirections.Count;
        }
    }

    //=================================================================================
    // GetEdge and SetEdge checks how often an edge has been set
    //=================================================================================
    public MazeCellEdge GetEdge (MazeDirection direction)
    {
        return edges[(int)direction];
    }

    public void SetEdge (MazeDirection direction, MazeCellEdge edge)
    {
        edges[(int)direction] = edge;
        initialisedEdgeCount += 1; 
    }

    //=================================================================================
    // Function which randomly decideds how many uninitialised directions should be
    // skipped. 
    //=================================================================================
    public MazeDirection RandomUninitialisedDirection
    {
        get
        {
            int skips = Random.Range(0, MazeDirections.Count - initialisedEdgeCount);
            for (int i = 0; i < MazeDirections.Count; i++)
            {
                if (edges[i] == null) //loop through the edges array
                {
                    if (skips == 0) //if skips limit has been reached, that direction is chosen
                    {
                        return (MazeDirection)i;
                    }
                    skips -= 1; //if not, decrease skips remaining by 1.
                }
            }
            throw new System.InvalidOperationException("MazeCell has no uninitialised directions left."); //throws out error message if no uninitialised directions remain
        }
    }
}
