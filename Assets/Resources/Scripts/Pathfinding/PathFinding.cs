using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

public class PathFinding : MonoBehaviour
{
    private GameObject start;
    private GameObject end;

    //private Transform spawn, finish;

    Grid grid; // Creates a reference to the grid

    void Start()
    {
        start = GameObject.FindWithTag("Start");
        end = GameObject.FindWithTag("End");        
    }

    //=================================================================================
    // Awake function called after all objects are initialised and gets the grid 
    // created in the Grid script
    //=================================================================================
    void Awake()
    {
        grid = GetComponent<Grid>();
    }

    //=================================================================================
    // Sets the startPos for the FindPath function to be controlled by the seeker's 
    // position and the targetPos by the target's position and checks these every frame.
    //=================================================================================
    void Update()
    {
        //Go to FindPath function and connect startPos and targetPos to spawn and finish
        FindPath(start.transform.position, end.transform.position); 
    }

    //=================================================================================
    // FindPath takes in the start location and end location to find a path between 
    // the two.
    //=================================================================================
    void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        // Uses the NodeFromWorldPoint function to set a start and end node
        Node startNode = grid.NodeFromWorldPoint(startPos); 
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        // openSet- the set of nodes to be evaluated
        Heap<Node> openSet = new Heap<Node>(grid.Maxsize);
        // closedSet- the set of nodes already evaluated
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet.RemoveFirst();
            
            closedSet.Add(currentNode);

            // If current node is equal to target node, the path has been found
            if (currentNode == targetNode) 
            {

                RetracePath(startNode, targetNode); // Jump to RetracePath function
                return;
            }

            // Evaluates each neighbour of the current node
            foreach (Node neighbour in grid.GetNeighbours(currentNode))
            {
                // If the neighbour node is obstructed or nighbour is in closedSet, skip to next neighbour
                if (!neighbour.walkable || closedSet.Contains(neighbour))
                {
                    continue; 
                }

                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);

                // If the new path to neighbour is shorter or neighbour is not in openSet
                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode; // Sets current node to be a parent of neighbour

                    // if neighbour is not in openSet, add neighbour to openset
                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                    else
                        openSet.UpdateItem(neighbour);
                }
            }
        }
    }

    //=================================================================================
    // RetracePath retraces steps from the end node to the start node to decide on
    // the path taken
    //=================================================================================
    void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse(); // Reverses the path so it goes from start node to the end node

        grid.path = path;

    }

    //=================================================================================
    // GetDistance function used to get the distance between two nodes
    //=================================================================================
    int GetDistance(Node nodeA, Node nodeB)
    {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (distX > distY)
            return 14 * distY + 10 * (distX - distY); // return if distance to node is closer on the y axis
        return 14 * distX + 10 * (distY - distX); // return if distance to node is closer on the x axis
    }
}
