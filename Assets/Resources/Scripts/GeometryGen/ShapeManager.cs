using UnityEngine;
using System.Collections;

public class ShapeManager : MonoBehaviour
{
    public enum DrawMode { Plane, Cube, Cylinder, Sphere};
    public DrawMode drawMode;

    public GameObject PlaneGeneration;
    public GameObject CubeGeneration;
    public GameObject CylinderGeneration;
    public GameObject SphereGeneration;

    private bool Plane;
    private bool Cube;
    private bool Cylinder;
    private bool Sphere;

    // Use this for initialization
    void Start ()
    {
        Plane = true;
        Cube = true;
        Cylinder = true;
        Sphere = true;

        Update();
        //Instantiate(PlaneGeneration);
        //Instantiate(CubeGeneration);
        //Instantiate(CylinderGeneration);
        //Instantiate(SphereGeneration);

    }

    public void DrawShapeInEditor()
    {
        if (drawMode == DrawMode.Plane && Plane == true)
        {
            Instantiate(PlaneGeneration);
            Plane = false;
        }
        else if (drawMode == DrawMode.Cube && Cube == true)
        {
            Instantiate(CubeGeneration);
            Cube = false;
        }
        else if (drawMode == DrawMode.Cylinder && Cylinder == true)
        {
            Instantiate(CylinderGeneration);
            Cylinder = false;
        }
        else if (drawMode == DrawMode.Sphere && Sphere == true)
        {
            Instantiate(SphereGeneration);
            Sphere = false;
        }
    }

    // Update is called once per frame
    void Update ()
    {
        DrawShapeInEditor();
    }
}
