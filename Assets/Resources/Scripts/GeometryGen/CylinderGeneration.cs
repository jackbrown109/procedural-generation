using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CylinderGeneration : MonoBehaviour
{
    //===========================================================
    // Public floats declared (can be altered in Unity)
    //===========================================================
    public int m_RadialSegmentCount = 1; // How many segments around the cylinder
    public int m_HeightSegmentCount = 1; // How many segments the tube of the cylinder is split into
    public float m_Radius = 1.0f; // Radius half the diameter of cylinder (affects thickness)
    public float m_Height = 1.0f; // How tall the cylinder is

    // Create a new mesh
    MeshBuilder meshBuilder = new MeshBuilder();


    //===========================================================
    // Start function to initialise code
    //===========================================================
    private void Start ()
    {
        Generate();
    }

    //===========================================================
    // Generate function begins sphere generation
    //===========================================================
    void Generate()
    {
        //Create new mesh
        meshBuilder = new MeshBuilder();

        // Access BuildCap function to create the caps on the cylinder
        BuildCap(meshBuilder, Vector3.zero, true);
        BuildCap(meshBuilder, Vector3.up * m_Height, false);


        float heightInc = m_Height / m_HeightSegmentCount;
        // For loop used to build the tube in rings depending on HeightSegmentCount
        for (int i = 0; i <= m_HeightSegmentCount; i++)
        {
            Vector3 centrePos = Vector3.up * heightInc * i; // Centre position of the current ring
            float v = (float)i / m_HeightSegmentCount; // v coordinate is based on the height
            // BuildRing function called to create cylinder's tube
            BuildRing(meshBuilder, m_RadialSegmentCount, centrePos, m_Radius, v, i > 0);
        }
    }

    //===========================================================
    // Update function to check if dimensions have been altered
    // with sliders.
    //===========================================================
    void Update()
    {
        if (GameObject.Find("GeometryCanvas(Clone)"))
        {
            float tempRadius = GameObject.Find("CylinderSlider4").GetComponent<Slider>().value;

            int tempRadialSegments = (int)GameObject.Find("CylinderSlider1").GetComponent<Slider>().value;

            float tempHeight = GameObject.Find("CylinderSlider3").GetComponent<Slider>().value;

            int tempHeightSegments = (int)GameObject.Find("CylinderSlider2").GetComponent<Slider>().value;


            if (m_Radius != tempRadius)
            {
                m_Radius = tempRadius;
                Generate();
            }

            if (m_RadialSegmentCount != tempRadialSegments)
            {
                m_RadialSegmentCount = tempRadialSegments;
                Generate();
            }

            if (m_Height != tempHeight)
            {
                m_Height = tempHeight;
                Generate();
            }

            if (m_HeightSegmentCount != tempHeightSegments)
            {
                m_HeightSegmentCount = tempHeightSegments;
                Generate();
            }
        }
    }


    //===========================================================
    // BuildRing function builds the tube portion of the cylinder
    //===========================================================
    void BuildRing (MeshBuilder meshBuilder, int segmentCount, Vector3 centre, float radius, float v, bool buildTriangles)
    {
        // builds the vertices around the edge
        float angleInc = (Mathf.PI * 2.0f) / segmentCount;

        for (int i = 0; i <= segmentCount; i++)
        {
            float angle = angleInc * i;

            Vector3 unitPosition = Vector3.zero;
            unitPosition.x = Mathf.Cos(angle);
            unitPosition.z = Mathf.Sin(angle);

            meshBuilder.Vertices.Add(centre + unitPosition * radius);
            meshBuilder.Normals.Add(unitPosition);
            meshBuilder.UVs.Add(new Vector2((float)i / segmentCount, v));


            // Build triangles
            if (i > 0 && buildTriangles)
            {
                int baseIndex = meshBuilder.Vertices.Count - 1;

                int vertsPerRow = segmentCount + 1;

                int index0 = baseIndex;
                int index1 = baseIndex - 1;
                int index2 = baseIndex - vertsPerRow;
                int index3 = baseIndex - vertsPerRow - 1;

                meshBuilder.AddTriangle(index0, index2, index1);
                meshBuilder.AddTriangle(index2, index3, index1);
            }

            // Assigns the created mesh to the mesh filter
            MeshFilter filter = GetComponent<MeshFilter>();

            if (filter != null)
            {
                filter.sharedMesh = meshBuilder.CreateMesh();
            }
        }

    }

    //===========================================================
    // BuildCap function caps the tube created in BuildRing
    //===========================================================
    void BuildCap(MeshBuilder meshBuilder, Vector3 centre, bool reverseDirection)
    {
        // The normal will be either up or down
        Vector3 normal = reverseDirection ? Vector3.down : Vector3.up;

        // Adds a vertex to the centre of cap
        meshBuilder.Vertices.Add(centre);
        meshBuilder.Normals.Add(normal);
        meshBuilder.UVs.Add(new Vector2(0.5f, 0.5f));

        // stores the index of the vertex added for later reference
        int centreVertexIndex = meshBuilder.Vertices.Count - 1;

        // Adds vertices around the edge of the cap
        float angleInc = (Mathf.PI * 2.0f) / m_RadialSegmentCount;

        for (int i = 0; i <= m_RadialSegmentCount; i++)
        {
            float angle = angleInc * i;

            Vector3 unitPosition = Vector3.zero;
            unitPosition.x = Mathf.Cos(angle);
            unitPosition.z = Mathf.Sin(angle);

            meshBuilder.Vertices.Add(centre + unitPosition * m_Radius);
            meshBuilder.Normals.Add(normal);

            Vector2 uv = new Vector2(unitPosition.x + 1.0f, unitPosition.z + 1.0f) * 0.5f;
            meshBuilder.UVs.Add(uv);

            //Build triangles
            if (i > 0)
            {
                int baseIndex = meshBuilder.Vertices.Count - 1;

                if (reverseDirection)
                    meshBuilder.AddTriangle(centreVertexIndex, baseIndex - 1,
                        baseIndex);
                else
                    meshBuilder.AddTriangle(centreVertexIndex, baseIndex,
                        baseIndex - 1);
            }
        }

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
            Gizmos.color = Color.black;

            // set a gizmo for every vertex in the shape
            foreach (Vector3 vec3 in meshBuilder.Vertices)
            {
                Vector3 cylinderPos = GameObject.Find("CylinderGeneration(Clone)").transform.position; // finds the position of the generated cylinder and sets the gizmos to spawn on it's position 
                Vector3 actualGizmoPos = cylinderPos + vec3;
                Gizmos.DrawSphere(actualGizmoPos, 0.1f); // adds the gizmos to the shape
            }
        }
    }
}
