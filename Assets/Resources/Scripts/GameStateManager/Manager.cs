using UnityEngine;
using System;

//=============================================================================================
// Manager class is a singleton so there is only one instance of this class active. Takes
// a child object as part of it's instantiation process.
//=============================================================================================
public abstract class Manager<DerivedManager>
    where DerivedManager : Manager<DerivedManager>
{
    protected abstract void Terminate();

    private static DerivedManager s_Instance;

    //=============================================================================================
    // Used to fetch the data in the DerivedManager but doesn't allow the variables value to be
    // set.
    //=============================================================================================
    public static DerivedManager Instance
    {
        get { return s_Instance; }
    }

    //=============================================================================================
    // Used to create an instance of the Manager class
    //=============================================================================================
    public static DerivedManager Create ()
    {
        try
        {
            // if instance of Manager class doesn't exist
            if (null == s_Instance)
            {
                s_Instance = Activator.CreateInstance(typeof(DerivedManager), true) as DerivedManager; // Create instance
            }
            else
            {
                string exceptionMessage = System.String.Format("Instance of {0} already exists", typeof(DerivedManager).ToString());

                throw new Exception(exceptionMessage);
            }
        }

        catch( Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return s_Instance;
    }

    //=============================================================================================
    // Destroy instance of the manager class.
    //=============================================================================================
    public static void Destroy()
    {
        s_Instance.Terminate();
    }

    public override String ToString()
    {
        return "Base Manager Class";
    }
}
