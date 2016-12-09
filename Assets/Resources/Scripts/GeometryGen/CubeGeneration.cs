using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CubeGeneration : MonoBehaviour
{
    //===========================================================
    // Public floats declared (can be altered in Unity)
    //===========================================================
    public float m_Width = 1.0f; //alters the width of the cube
    public float m_Length = 1.0f; //alters the length of the cube
    public float m_Height = 1.0f; //alters the height of the cube

    // Create a new mesh
    MeshBuilder meshBuilder = new MeshBuilder();


    // Calculates directional vectors for all 3 dimensions of the cube
    //Vector3 upDir;
    //Vector3 rightDir;
    //Vector3 forwardDir;


    //Vector3 farCorner; // Divides locations of farCorner vertex by 2 to place the cube in the centre of Unity scene
    //Vector3 nearCorner; // -farCorner will find where the nearCorner vertex lies


    //===========================================================
    // Start function to initialise code
    //===========================================================
    private void Start()
    {
        Generate(); // Call the generate function
    }

    //===========================================================
    // Generate function begins cube generation
    //===========================================================
    void Generate()
    {
        // Create new mesh
        meshBuilder = new MeshBuilder();
        // Calculates directional vectors for all 3 dimensions of the cube
        Vector3 upDir = Vector3.up * m_Height;
        Vector3 rightDir = Vector3.right * m_Width;
        Vector3 forwardDir = Vector3.forward * m_Length;


        Vector3 farCorner = (upDir + rightDir + forwardDir) / 2; // Divides locations of farCorner vertex by 2 to place the cube in the centre of Unity scene
        Vector3 nearCorner = -farCorner; // -farCorner will find where the nearCorner vertex lies

        // Access the code within the BuildQuad function for vertices connected to nearCorner to build the 3 quads that originate from the near corner
        BuildQuad(meshBuilder, nearCorner, forwardDir, rightDir);
        BuildQuad(meshBuilder, nearCorner, rightDir, upDir);
        BuildQuad(meshBuilder, nearCorner, upDir, forwardDir);

        // Access the code within the BuildQuad function for vertices connected to farCorner to build the 3 quads that originate from the far corner
        BuildQuad(meshBuilder, farCorner, -rightDir, -forwardDir);
        BuildQuad(meshBuilder, farCorner, -upDir, -rightDir);
        BuildQuad(meshBuilder, farCorner, -forwardDir, -upDir);
    }

    //===========================================================
    // Update function to check if dimensions have been altered
    // with sliders.
    //===========================================================
    void Update()
    {
        if (GameObject.Find("GeometryCanvas(Clone)"))
        {
            float tempWidth = GameObject.Find("CubeSlider1").GetComponent<Slider>().value;

            float tempLength = GameObject.Find("CubeSlider2").GetComponent<Slider>().value;

            float tempHeight = GameObject.Find("CubeSlider3").GetComponent<Slider>().value;


            if (m_Width != tempWidth)
            {
                m_Width = tempWidth;            
                Generate();
            }

            if (m_Length != tempLength)
            {
                m_Length = tempLength;
                Generate();
            }

            if (m_Height != tempHeight)
            {
                m_Height = tempHeight;
                Generate();
            }
        }
    }

    //===========================================================
    // BuildQuad function builds the faces of the cube
    //===========================================================
    void BuildQuad(MeshBuilder meshBuilder, Vector3 offset, Vector3 widthDir, Vector3 lengthDir)
    {
        Vector3 normal = Vector3.Cross(lengthDir, widthDir).normalized;

        meshBuilder.Vertices.Add(offset);
        meshBuilder.UVs.Add(new Vector2(0.0f, 0.0f));
        meshBuilder.Normals.Add(normal);

        meshBuilder.Vertices.Add(offset + lengthDir);
        meshBuilder.UVs.Add(new Vector2(0.0f, 1.0f));
        meshBuilder.Normals.Add(normal);

        meshBuilder.Vertices.Add(offset + lengthDir + widthDir);
        meshBuilder.UVs.Add(new Vector2(1.0f, 1.0f));
        meshBuilder.Normals.Add(normal);

        meshBuilder.Vertices.Add(offset + widthDir);
        meshBuilder.UVs.Add(new Vector2(1.0f, 0.0f));
        meshBuilder.Normals.Add(normal);


        // accounts for the 4 vertices just created
        int baseIndex = meshBuilder.Vertices.Count - 4;

        // Builds triangles
        meshBuilder.AddTriangle(baseIndex, baseIndex + 1, baseIndex + 2);
        meshBuilder.AddTriangle(baseIndex, baseIndex + 2, baseIndex + 3);

        // Assigns the created mesh to the mesh filter
        MeshFilter filter = GetComponent<MeshFilter>();

        if (filter != null)
        {
            filter.sharedMesh = meshBuilder.CreateMesh();
        }
    }

    //===========================================================
    // Adds black gizmos to each of the vertices created
    //===========================================================
    private void OnDrawGizmos()
    {
        // if vertices exist
        if (meshBuilder.Vertices != null)
        {
            Gizmos.color = Color.black; //sets colour of the gizmos

            //set a gizmo for every vertex in the shape
            foreach (Vector3 vec3 in meshBuilder.Vertices)
            {
                Vector3 cubePos = GameObject.Find("CubeGeneration(Clone)").transform.position; // finds the position of the generated cube and sets the gizmos to spawn on it's position 
                Vector3 actualGizmoPos = cubePos + vec3;
                Gizmos.DrawSphere(actualGizmoPos, 0.1f); //adds the gizmos to the shape
            }
        }
    }
}
