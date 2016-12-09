using UnityEngine;

public enum MazeDirection{North,East,South,West} // Creates an enum of the 4 directions which can be chosen

public static class MazeDirections // enums cannot define methods or properties inside, so MazeDirections is used
{
    public const int Count = 4; // Const int 4 as there are only 4 directions to choose from

    //=================================================================================
    //  Retrieves a random direction for the generation of the next maze piece
    //=================================================================================
    public static MazeDirection RandomValue
    {
        get
        {
            return (MazeDirection)Random.Range(0, Count);
        }
    }

    //=================================================================================
    // Adjust the current coordinates based on the random direction retrieved.
    //=================================================================================
    private static IntVector2[] vectors = {
        new IntVector2 (0,1),
        new IntVector2 (1,0),
        new IntVector2 (0,-1),
        new IntVector2 (-1,0) };

    //=================================================================================
    // Converts a direction into an integer vector using a private static array
    // of vectors.
    //=================================================================================
    public static IntVector2 ToIntVector2 (this MazeDirection direction) // "this" so ToIntVector2 will behave as an instance method of MazeDirection
    {
        return vectors[(int)direction];
    }

   // Creates opposites of the previously declared North, East, South, West
    private static MazeDirection[] opposites = {
        MazeDirection.South,
        MazeDirection.West,
        MazeDirection.North,
        MazeDirection.East };

    //=================================================================================
    // GetOpposite finds the opposite of the maze direction
    //=================================================================================
    public static MazeDirection GetOpposite (this MazeDirection direction)
    {
        return opposites[(int)direction];
    }

    //=================================================================================
    // Fixes the rotation of the wall pieces so they are created in the right direction
    //=================================================================================
    private static Quaternion[] rotations = {
        Quaternion.identity,
        Quaternion.Euler(0f, 90f, 0f),
        Quaternion.Euler(0f, 180f, 0f),
        Quaternion.Euler(0f, 270f, 0f) };

    public static  Quaternion ToRotation (this MazeDirection direction)
    {
        return rotations[(int)direction];
    }

    
}
