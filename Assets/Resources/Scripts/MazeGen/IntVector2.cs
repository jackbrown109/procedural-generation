
[System.Serializable]
public struct IntVector2 // Struct allows us to use Vector2 with integer values
{
    public int x, z; // Bundles together the two integers as a single value

    //=================================================================================
    //  IntVector2 allows us to define values with "new IntVector2(x, z)"
    //=================================================================================
    public IntVector2 (int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    //=================================================================================
    // Adds support for the + operator, so we can add two vectors together
    //=================================================================================
    public static IntVector2 operator + (IntVector2 a, IntVector2 b)
    {
        a.x += b.x;
        a.z += b.z;
        return a;
    }
}
