using UnityEngine;

public abstract class MazeCellEdge : MonoBehaviour
{
    public MazeCell cell, otherCell; // A reference to the cell it belongs to and other connecting cells

    public MazeDirection direction; // A direction to remember the cells orientation

    //=================================================================================
    // Function to make the edges children of their cells and place them in the same
    // location.  
    //=================================================================================
    public void Initialise (MazeCell cell, MazeCell otherCell, MazeDirection direction)
    {
        this.cell = cell;
        this.otherCell = otherCell;
        this.direction = direction;
        cell.SetEdge(direction, this);
        transform.parent = cell.transform;
        transform.localPosition = Vector3.zero;
        // Accesses ToRotation function in MazeDirections so walls spawn on side where the maze was interrupted by a previously activated cell
        transform.localRotation = direction.ToRotation();
    }
}
