using UnityEngine;
using System.Collections;

public class Node : IHeapItem<Node>
{
    public bool walkable; //whether the state of the object defined is walkable
    public Vector3 worldPosition; //where the node is in the unity editor

    public int gridX; // Used to store the node's x location in the Node class
    public int gridY; // Used to store the node's y location in the Node class

    public int gCost; // holds the value for how far from the start node is the current node
    public int hCost; // holds the value for how far from the target node is the current node

    public Node parent; // Sets a parent variable for the Node class

    int heapIndex;

    //=================================================================================
    //  Assigns the values when a node is created.
    //=================================================================================
    public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY)
    {
        walkable = _walkable;
        worldPosition = _worldPos;
        gridX = _gridX;
        gridY = _gridY;

    }

    //=================================================================================
    // Calculates fCost to decide which node to check next
    //=================================================================================
    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    //=================================================================================
    // Specify the HeapIndex found in the interface in the Heap class
    //=================================================================================
    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }

    //=================================================================================
    // Specify the CompareTo from IComparable in the interface in the Heap class
    //=================================================================================
    public int CompareTo(Node nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);

        // If fCosts are equal, use the h cost to decide
        if (compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }
}
