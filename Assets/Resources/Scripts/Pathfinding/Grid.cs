using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour
{
    public LayerMask unwalkableMask; // Creates a reference to the layer which will block the path
    public Vector2 gridWorldSize; // Defines the area in world coordinates that the grid will cover
    public float nodeRadius; // Defines how much space each node covers
    Node[,] grid; // 2d array of nodes to represent the grid

    float nodeDiameter; // Diameter of each node created
    int gridSizeX, gridSizeY;

    //=================================================================================
    // Initialise the grid used for the pathfinding process
    //=================================================================================
    void Start()
    {
        nodeDiameter = nodeRadius *2;
        gridSizeX = Mathf.RoundToInt (gridWorldSize.x / nodeDiameter); // How many nodes can fit into the area in x direction
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);// How many nodes can fit into the area in y direction
        CreateGrid(); // Access the CreateGrid() function to begin grid creation.
    }

    void Update()
    {
        foreach (Node n in grid) // for every node in the grid array, we want to check if its walkable
        {
            bool walkable = !(Physics.CheckSphere(n.worldPosition, nodeRadius, unwalkableMask));
            n.walkable = walkable;
        }
    }

    //=================================================================================
    // Used to find the max heap size
    //=================================================================================
    public int Maxsize
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }

    //=================================================================================
    // CreateGrid function used to create the array of nodes for the grid and check 
    // for collisions in the grid
    //=================================================================================
    void CreateGrid()
    {
        // Create new 2d array for grid
        grid = new Node[gridSizeX, gridSizeY];

        // Finds world position for each collision check
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

        // loops through all the positions the grid will be in
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius); // finds each point a node will occupy 
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask)); // collision check for each node to see if the path is obstructed

                // Populates the grid with nodes
                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    //=================================================================================
    // GetNeighbours function finds the nodes surrounding the current node
    //=================================================================================
    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>(); //access node location

        // searchs through a 3 by 3 grid around current node location
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                // ignores current node
                if (x == 0 && y == 0) 
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                // if node is inside the grid, add it to neighbours
                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]); // adds node to neighbours
                }                
            }
        }

        return neighbours; //returns list
    }

    //=================================================================================
    // Converts a world position into grid coordinates
    //=================================================================================
    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x; // How far along the x direction (percentage between 0 and 1) is the node
        float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y; // How far along the y direction (percentage between 0 and 1) is the node

        // Stops a value outside of the grid being produced
        percentX = Mathf.Clamp01(percentX); // Clamps value between 0 and 1 for x
        percentY = Mathf.Clamp01(percentY); // Clamps value between 0 and 1 for y

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX); // Finds the x indicies of the grid array
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY); // Finds the y indicies of the grid array

        // Returns the node from the grid
        return grid[x, y]; 
    }

    public List<Node> path;

    //=================================================================================
    // Adds the gizmos to show the open maze areas, the walled areas and the path
    // drawn from the seeker to the target
    //=================================================================================
    void OnDrawGizmos()
    {
        // if grid array is not empty
        if (grid != null) 
        {
            foreach (Node n in grid) // Foreach node in the grid created
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red; // Sets colour of open nodes and obstructed nodes

                // If path has been set by RetracePath function in Pathfinding script
                if (path != null)
                    // If path contains current node set it's colour to black
                    if (path.Contains(n))
                            Gizmos.color = Color.black; // sets colour of the gizmos in the path
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter/*- .1f*/)); // Draws cube gizmos for each node
            }
        }
        
    }
}
