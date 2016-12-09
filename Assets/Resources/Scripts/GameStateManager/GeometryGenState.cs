using UnityEngine;
using System.Collections;

public class GeometryGenState : State
{
    GameObject Cube;
    GameObject Cylinder;
    GameObject Sphere;
    GameObject Sliders;

    public GameStateManager stateManager;

    public GeometryGenState(string a_stringName) : base(a_stringName)
    {
        Debug.Log("Current state: GeometryGenState");

        m_fDuration = -1;
        Process = Initialise;
    }

    protected override void Initialise(float a_fTimeStep)
    {
        Debug.Log("GeometryGenState Initialise");

        Cube = Resources.Load("Prefabs/GeometryGen/CubeGeneration", typeof(GameObject)) as GameObject;
        Cylinder = Resources.Load("Prefabs/GeometryGen/CylinderGeneration", typeof(GameObject)) as GameObject;
        Sphere = Resources.Load("Prefabs/GeometryGen/SphereGeneration", typeof(GameObject)) as GameObject;

        Sliders = Resources.Load("Prefabs/GeometryGen/GeometryCanvas", typeof(GameObject)) as GameObject;

        Camera.main.transform.rotation = Quaternion.Euler(new Vector3(45, 0, 0));
        Camera.main.transform.position = new Vector3(0, 89, -16);

        GameObject.Instantiate(Cube);
        GameObject.Instantiate(Cylinder);
        GameObject.Instantiate(Sphere);

        GameObject.Instantiate(Sliders);

        Process = Update;
    }

    protected override void Update(float a_fTimeStep)
    {
        Debug.Log("GeometryGenState Update");

        if (this != GameStateManager.Instance.CurrentState)
        {
            Process = Leave;
        }
    }

    protected override void Leave(float a_fTimeStep)
    {
        Debug.Log("GeometryGenState Leave");

        if (GameObject.Find("CubeGeneration(Clone)"))
        {
            GameObject.Destroy(GameObject.Find("CubeGeneration(Clone)"));
        }

        if (GameObject.Find("CylinderGeneration(Clone)"))
        {
            GameObject.Destroy(GameObject.Find("CylinderGeneration(Clone)"));
        }

        if (GameObject.Find("SphereGeneration(Clone)"))
        {
            GameObject.Destroy(GameObject.Find("SphereGeneration(Clone)"));
        }

        if (GameObject.Find("GeometryCanvas(Clone)"))
        {
            GameObject.Destroy(GameObject.Find("GeometryCanvas(Clone)"));
        }

        if (this == GameStateManager.Instance.CurrentState)
        {
            Process = Initialise;
        }
    }
}

