using UnityEngine;
using System.Collections;


public class MapGenerator : MonoBehaviour
{
    public enum DrawMode {NoiseMap, ColourMap, Mesh};
    public DrawMode drawMode;

    public int mapWidth;
    public int mapHeight;

   // public const int mapChunkSize = 241;
    [Range(0,6)]
    public int levelOfDetail;
    public float noiseScale;

    public int octaves;
    [Range(0,1)]
    public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    public float meshHeightMultiplier;
    public AnimationCurve meshHeightCurve;

    public TerrainType[] regions;

    public Texture2D textureHeightMap;

    /*
    public void DrawMapInEditor ()
    {
        MapData mapData = GenerateMapData();

        MapDisplay display = FindObjectOfType<MapDisplay>();
        if (drawMode == DrawMode.NoiseMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(mapData.heightMap));
        }
        else if (drawMode == DrawMode.ColourMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromColourMap(mapData.colourMap, mapChunkSize, mapChunkSize));
        }
        else if (drawMode == DrawMode.Mesh)
        {
            display.DrawMesh(MeshGenerator.GenerateTerrainMesh(mapData.heightMap, meshHeightMultiplier, meshHeightCurve, levelOfDetail), TextureGenerator.TextureFromColourMap(mapData.colourMap, mapChunkSize, mapChunkSize));
        }
    }
    */

    public void Generate()
    {
        float[,] noiseMap;
        MapDisplay display = FindObjectOfType<MapDisplay>();

        if (textureHeightMap)
        {
            float xRatio = textureHeightMap.width / mapWidth;
            float yRatio = textureHeightMap.height / mapHeight;
            noiseMap = new float[mapWidth, mapHeight];
            
            for (int x = 0; x < mapWidth; x++)
            {
                for (int y = 0; y < mapWidth; y++)
                {
                    noiseMap[x, y] = textureHeightMap.GetPixel(x * (int)xRatio, y * (int)yRatio).grayscale;
                }
            }
        }

        else
        {
            noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);
        }

        Color[] colourMap = new Color[mapWidth * mapHeight];

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++)
                {
                    if (currentHeight <= regions[i].height)
                    {
                        colourMap[y * mapWidth + x] = regions[i].colour;
                        break;
                    }
                }
            }
            switch(drawMode)
            {
                case DrawMode.NoiseMap:
                    display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
                    break;

                case DrawMode.ColourMap:
                    display.DrawTexture(TextureGenerator.TextureFromColourMap(colourMap, mapWidth, mapHeight));
                    break;

                case DrawMode.Mesh:
                    display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve, levelOfDetail), TextureGenerator.TextureFromColourMap(colourMap, mapWidth, mapHeight));
                    break;
            }
        }
    }

    public void Generate(int a_mapWidth, int a_mapHeight, int a_seed, float a_scale, int a_octaves, float a_persistance, float a_lacunarity, Vector2 a_offset, float a_meshHeightMultiplier, GameObject a_mesh)
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(a_mapWidth, a_mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);

        MapDisplay display = a_mesh.GetComponent<MapDisplay>();

        noiseMap = Noise.GenerateNoiseMap(a_mapWidth, a_mapHeight, seed, a_scale, a_octaves, a_persistance, a_lacunarity, a_offset);

        Color[] colourMap = new Color[a_mapWidth * a_mapHeight];

        regions = new TerrainType[5];
        regions[0] = new TerrainType("Water", 0.2f, new Color(0.2f, 0.2f, 1.0f));
        regions[1] = new TerrainType("Sand", 0.3f, new Color(0.6f, 0.6f, 0.4f));
        regions[2] = new TerrainType("Grass", 0.5f, new Color(0.2f, 0.5f, 0.2f));
        regions[3] = new TerrainType("Rock", 0.85f, new Color(0.3f, 0.15f, 0.1f));
        regions[4] = new TerrainType("Snpw", 1.0f, new Color(1.0f, 1.0f, 1.0f));

        for (int y = 0; y < a_mapHeight; y ++)
        {
            for (int x = 0; x < a_mapWidth; x++)
            {
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++)
                {
                    if (currentHeight <= regions [i].height)
                    {
                        colourMap[y * a_mapWidth + x] = regions[i].colour;
                        break;
                    }
                }
            }
        }

        Keyframe[] keyframes = { new Keyframe(0.0f, 0.0f), new Keyframe(0.4f, 0.0f), new Keyframe(1.0f, 1.0f) };
        AnimationCurve ac = new AnimationCurve(keyframes);

        Texture2D tex = TextureGenerator.TextureFromColourMap(colourMap, a_mapWidth, a_mapHeight);
        MeshData meshData = MeshGenerator.GenerateTerrainMesh(noiseMap, a_meshHeightMultiplier, ac, levelOfDetail);

        display.DrawMesh(meshData, tex);


    }

    void OnValidate()
    {
        if (lacunarity < 1)
        {
            lacunarity = 1;
        }
        if (octaves < 0)
        {
            octaves = 0;
        }
    }
}

[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color colour;

    public TerrainType(string name, float height, Color colour)
    {
        this.name = name;
        this.height = height;
        this.colour = colour;
    }
}
/*
public struct MapData
{
    public float[,] heightMap;
    public Color[] colourMap;

    public MapData (float[,] heightMap, Color[] colourMap)
    {
        this.heightMap = heightMap;
        this.colourMap = colourMap;
    }
}
*/