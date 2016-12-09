using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SphereGeneration : MonoBehaviour
{
    //===========================================================
    // Public floats declared (can be altered in Unity)
    //===========================================================
    public float m_Radius = 1.0f; //radius of the sphere
    public int m_RadialSegmentCount = 1; // the number of radial segments of the sphere

    // Create a new mesh
    MeshBuilder meshBuilder = new MeshBuilder();

    //===========================================================
    // Start function to initialise code
    //===========================================================
    void Start ()
    {
        Generate();
    }

    //===========================================================
    // Generate function begins sphere generation
    //===========================================================
    void Generate()
    {
        // Create new mesh
        meshBuilder = new MeshBuilder();

        // Height segments half of Radial segment count so the sphere is even horizontally and vertically
        int heightSegmentCount = m_RadialSegmentCount / 2;

        float angleInc = Mathf.PI / heightSegmentCount; // the angle increment per height segment

        for (int i = 0; i <= heightSegmentCount; i++)
        {
            Vector3 centrePos = Vector3.zero;

            // calculates a height offset and radius based on a vertical circle calculation
            centrePos.y = -Mathf.Cos(angleInc * i) * m_Radius;
            float radius = Mathf.Sin(angleInc * i) * m_Radius;

            float v = (float)i / heightSegmentCount;

            // Access the BuildRingForSphere function for each height segment to build up sphere
            BuildRingForSphere(meshBuilder, m_RadialSegmentCount, centrePos, radius, v, i > 0);
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
            float tempRadius = GameObject.Find("SphereSlider2").GetComponent<Slider>().value;

            int tempRadialSegments = (int)GameObject.Find("SphereSlider1").GetComponent<Slider>().value;


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
        }
    }

    //===========================================================
    // BuildRingForSphere builds the rings to create the sphere
    //===========================================================
    void BuildRingForSphere(MeshBuilder meshBuilder, int segmentCount, Vector3 centre, float radius,
    float v, bool buildTriangles)
    {
        // Builds the vertices around the edge
        float angleInc = (Mathf.PI * 2.0f) / segmentCount;

        for (int i = 0; i <= segmentCount; i++)
        {
            float angle = angleInc * i;

            Vector3 unitPosition = Vector3.zero;
            unitPosition.x = Mathf.Cos(angle);
            unitPosition.z = Mathf.Sin(angle);

            Vector3 vertexPosition = centre + unitPosition * radius;

            meshBuilder.Vertices.Add(vertexPosition);
            meshBuilder.Normals.Add(vertexPosition.normalized);
            meshBuilder.UVs.Add(new Vector2((float)i / segmentCount, v));

            // Builds triangles
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

                // Assigns the created mesh to the mesh filter
                MeshFilter filter = GetComponent<MeshFilter>();

                if (filter != null)
                {
                    filter.sharedMesh = meshBuilder.CreateMesh();
                }
            }
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
            Gizmos.color = Color.black; // sets the colour of the gizmos

            //set a gizmo for every vertex in the shape
            foreach (Vector3 vec3 in meshBuilder.Vertices)
            {
                Vector3 spherePos = GameObject.Find("SphereGeneration(Clone)").transform.position; // finds the position of the generated sphere and sets the gizmos to spawn on it's position 
                Vector3 actualGizmoPos = spherePos + vec3;
                Gizmos.DrawSphere(actualGizmoPos, 0.1f); //adds the gizmos to the shape
            }
        }
    }

}
