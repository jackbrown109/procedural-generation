using UnityEngine;
using System.Collections;


public class TerrainGenState : State
{
    GameObject TerrainGen;
    GameObject mesh;

    MapGenerator mapGen;
    MapDisplay mapDisplay;

    public GameStateManager stateManager;

    public TerrainGenState(string a_stringName) : base(a_stringName)
    {
        Debug.Log("Current state: TerrainGenState");

        m_fDuration = -1;
        Process = Initialise;
    }

    protected override void Initialise(float a_fTimeStep)
    {
        Debug.Log("TerrainGenState Initialise");

        TerrainGen = Resources.Load("Prefabs/TerrainGen/MapGenerator", typeof(GameObject)) as GameObject;
        mesh = Resources.Load("Prefabs/TerrainGen/Mesh", typeof(GameObject)) as GameObject;
        Camera.main.transform.rotation = Quaternion.Euler(new Vector3(43.5f, 0, 0));
        Camera.main.transform.position = new Vector3(-8, 123, -260);
        
        if (mesh.GetComponent<MapDisplay>() == null)
        {
            mapDisplay = mesh.AddComponent<MapDisplay>();
            mapDisplay.meshFilter = mesh.AddComponent<MeshFilter>();
            mapDisplay.meshRenderer = mesh.AddComponent<MeshRenderer>();
            //mapDisplay.meshRenderer.material = new Material(Shader.Find("Diffuse"));
            mapDisplay.meshRenderer.material = mesh.GetComponent<Material>();
        }

        mapGen = TerrainGen.GetComponent<MapGenerator>();
        mapGen.Generate(128, 128, 0, 30, 4, 0.5f, 2, Vector2.zero, 10.0f, mesh);
        

        GameObject.Instantiate(TerrainGen);
        GameObject.Instantiate(mesh);

        Process = Update;
    }

    protected override void Update(float a_fTimeStep)
    {
        Debug.Log("TerrainGenState Update");

        if (this != GameStateManager.Instance.CurrentState)
        {
            Process = Leave;
        }
    }

    protected override void Leave(float a_fTimeStep)
    {
        Debug.Log("TerrainGenState Leave");

        if (GameObject.Find("MapGenerator(Clone)"))
        {
            GameObject.Destroy(GameObject.Find("MapGenerator(Clone)"));
        }

        if (GameObject.Find("Mesh(Clone)"))
        {
            GameObject.Destroy(GameObject.Find("Mesh(Clone)"));
        }

        if (this == GameStateManager.Instance.CurrentState)
        {
            Process = Initialise;
        }
    }
}

