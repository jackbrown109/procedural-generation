using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//===========================================================
// MeshBuilder class used for mesh initialisation for each
// shape generation script.
//===========================================================
public class MeshBuilder
{ 

    // Creates a list of vertex positions for the mesh
    private List<Vector3> m_Vertices = new List<Vector3>();
    public List<Vector3> Vertices {  get { return m_Vertices; } }

    // Creates a list of vertex normals for the mesh
    private List<Vector3> m_Normals = new List<Vector3>();
    public List<Vector3> Normals { get { return m_Normals; } }

    // Creates a list of UV coordinates for the mesh
    private List<Vector2> m_UVs = new List<Vector2>();
    public List<Vector2> UVs { get { return m_UVs; } }

    // Creates a list of indices for the triangles
    private List<int> m_Indices = new List<int>();

    //===========================================================
    // Adds a triangle to the mesh.
    //===========================================================
    public void AddTriangle (int index0, int index1, int index2)
    {
        m_Indices.Add(index0); //vertex index at corner 0 of triangle
        m_Indices.Add(index1); //vertex index at corner 1 of triangle
        m_Indices.Add(index2); //vertex index at corner 2 of triangle
    }

    //===========================================================
    // Initialises an instance of the Unity Mesh class, based on
    // the stored values, then returns the completed mesh.
    //===========================================================
    public Mesh CreateMesh()
    {
        // Creates an instance of the Unity Mesh class
        Mesh mesh = new Mesh();

        //adds the vertex and triangle values to the new mesh
        mesh.vertices = m_Vertices.ToArray();
        mesh.triangles = m_Indices.ToArray();

        if (m_Normals.Count == m_Vertices.Count)
            mesh.normals = m_Normals.ToArray();

        if (m_UVs.Count == m_Vertices.Count)
            mesh.uv = m_UVs.ToArray();

        // The mesh recalculates its bounding box
        mesh.RecalculateBounds();

        return mesh; // Returns the completed mesh
    }
}
