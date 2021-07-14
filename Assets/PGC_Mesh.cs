/**
 * Ryan Anderson
 * PGC_Mesh.cs generates a perlin noise mesh and animates its movement with a sine function.
 * It also generates a primitive sphere as well as a cube and drops them onto the grid.
 * 
 **/



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PGC_Mesh : MonoBehaviour
{
/// ////////////////////
/// Cube Variables
    GameObject cube;
    Rigidbody cubeRb;
    MeshCollider cubeMeshCollider;
    Mesh cubeMesh;
    MeshRenderer cubeMeshRenderer;
    MeshFilter cubeFilter;

    Vector3[] cubeVertices;
    int[] cubeTriangles;

    public int cubeX;
    public int cubeY = 10;
    public int cubeZ;

/// ////////////////////
/// NoiseMesh Variables
    Mesh mesh;
    GameObject noiseMesh;
    MeshFilter meshFilter;
    MeshCollider meshCollider;
    MeshRenderer meshRend;
    Material meshMat;

    public int xSize = 60;
    public int zSize = 40;

    Vector3[] vertices;
    int[] triangles;
    Vector3[] waveVert;


    public float wavelength = 6f;
    public float amplitude = 2f;
    public float speed = 1.5f;

    float noise;

/// ////////////////////
/// Sphere Variables
    public int sphereX;
    public int sphereY = 10;
    public int sphereZ;

    void Awake()
    {
        //NoiseMesh commands are run on awake because createCube depends on meshMat variable
        noiseMesh = new GameObject();
        noiseMesh.name= "NoiseMesh";
        meshFilter = noiseMesh.AddComponent<MeshFilter>();
        meshCollider = noiseMesh.AddComponent<MeshCollider>();
        meshCollider.convex = true;

        meshRend = noiseMesh.AddComponent<MeshRenderer>();
        meshMat = new Material(Shader.Find("Diffuse"));
        meshRend.material = meshMat;
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateMesh();
        CreateTestSphere();
        CreateCube();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMesh();
    }

    //Updates NoiseMesh to move dynamically with a sine function
    void UpdateMesh()
    {
        noiseMesh.GetComponent<MeshFilter>().sharedMesh = mesh;
        noiseMesh.GetComponent<MeshCollider>().sharedMesh = meshFilter.mesh;

        waveVert = new Vector3[(xSize + 1) * (zSize + 1)];


        for (int z = 0, index = 0; z < zSize + 1; z++)
        {
            for (int x = 0; x < xSize + 1; x++)
            {
                waveVert[index].x = x;
                waveVert[index].z = z;
                waveVert[index].y = vertices[index].y //noise 
                                   + amplitude * Mathf.Sin((2 * 3.14f / wavelength) * (vertices[index].x - speed * Time.time)); //wave motion
                index++;
            }
        }

        mesh.vertices = waveVert;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }

    //Generates NoiseMesh triangles/vertices and renders the mesh
    void CreateMesh()
    {

        noiseMesh.GetComponent<MeshFilter>().sharedMesh = mesh = new Mesh();
        noiseMesh.GetComponent<MeshCollider>().sharedMesh = meshFilter.mesh;


        vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        triangles = new int[xSize * zSize * 6];//3 per triangle renders 1 square a time for 6 verts
        int vertice = 0;//offset
        int tris = 0;//offset

        //Fills vertices array
        for (int z = 0, index = 0; z < zSize + 1; z++)
        {
            for (int x = 0; x < xSize + 1; x++)
            {
                noise = Mathf.PerlinNoise(x*.4f, z*.4f) * 2f;
                vertices[index] = new Vector3(x, noise, z);

                index++;
            }
        }

        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = 0 + vertice;  //first triangle
                triangles[tris + 1] = xSize + 1 + vertice;
                triangles[tris + 2] = 1 + vertice;

                triangles[tris + 3] = 1 + vertice;  //second triangle
                triangles[tris + 4] = xSize + 1 + vertice;
                triangles[tris + 5] = xSize + 2 + vertice;
                
                vertice++;
                tris += 6;
            }
            vertice++;//makes sure endpoints on grid arnt connected
        }


        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }

    //Creates a unity sphere primitive, moves it to spawnpoint, and replaces the sphere collider with a mesh collider
    void CreateTestSphere()
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = new Vector3(sphereX, sphereY, sphereZ);//Updating spawn position
        MeshCollider meshSphereC = sphere.AddComponent<MeshCollider>();
        meshSphereC.convex = true;
        Rigidbody sphereRb = sphere.AddComponent<Rigidbody>();
        Destroy(sphere.GetComponent<SphereCollider>());//Removes sphere collider so only meshcollider exists
    }

    //Creates a cube object, moves it to spawnpoint, generates triangles/vertices, then renders
    void CreateCube()
    {
        cube = new GameObject();
        cube.name = "Cube";
        cubeFilter = cube.AddComponent<MeshFilter>();
        cubeMeshCollider = cube.AddComponent<MeshCollider>();
        cubeMeshCollider.convex = true;

        cubeMeshRenderer = cube.AddComponent<MeshRenderer>();
        cubeRb = cube.AddComponent<Rigidbody>();
        cubeMeshRenderer.material = meshMat;



        cube.GetComponent<MeshFilter>().sharedMesh = cubeMesh = new Mesh();
        cube.GetComponent<MeshCollider>().sharedMesh = cubeFilter.mesh;


        cube.transform.position = new Vector3(cubeX, cubeY, cubeZ); //Updates position

        cubeVertices = new Vector3[8]; //8 corners in a cube
        cubeTriangles = new int[36];//6 per side * 6 sides

        //Generating vertices for cube object
        cubeVertices = new Vector3[]
        {
            new Vector3(cubeX,cubeY,cubeZ), new Vector3(cubeX,cubeY+1,cubeZ), new Vector3(cubeX+1,cubeY+1,cubeZ), new Vector3(cubeX+1,cubeY,cubeZ),
            new Vector3(cubeX,cubeY,cubeZ+1), new Vector3(cubeX,cubeY+1,cubeZ+1), new Vector3(cubeX+1,cubeY+1,cubeZ+1), new Vector3(cubeX+1,cubeY,cubeZ+1)
        };
        //Generating triangles for cube object
        cubeTriangles = new int[]
        {
            //bottom
            0, 1, 2,
            2, 3, 0,
            //front
            1, 5, 6,
            6, 2, 1,
            //right
            2, 6, 3,
            3, 6, 7,
            //left
            1, 4, 5,
            0, 4, 1,
            //back
            4, 0, 3,
            3, 7, 4,
            //top
            7, 6, 5,
            5, 4, 7


        };

        cubeMesh.vertices = cubeVertices;
        cubeMesh.triangles = cubeTriangles;

        cubeMesh.RecalculateNormals();
    }
}
