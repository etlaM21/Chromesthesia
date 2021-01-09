using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CicleFormation : MonoBehaviour
{
    // Start is called before the first frame update
    // Instantiates prefabs in a circle formation
   public GameObject prefab;
   public int numberOfObjects = 20;
   public float radius = 20f;

   public float radiusInner = 5f;
   void Start()
   {
       /*for (int i = 0; i < numberOfObjects; i++)
       {
           float angle = i * Mathf.PI * 2 / numberOfObjects;
           float x = Mathf.Cos(angle) * radius;
           float z = Mathf.Sin(angle) * radius;
           Vector3 pos = transform.position + new Vector3(x, 0, z);
           float angleDegrees = -angle*Mathf.Rad2Deg;
           Quaternion rot = Quaternion.Euler(0, angleDegrees, 0);
           Instantiate(prefab, pos, rot);
       } */

       	//CreateCube ();

       for (int i = 0; i < numberOfObjects; i++)
       {
            /*
           float angle = i * Mathf.PI * 2  / numberOfObjects;
           float x = Mathf.Cos(angle) * radius;
           float y = Mathf.Sin(angle) * radius;
           Vector3 pos = transform.position + new Vector3(x, y, 0);
           float angleDegrees = +angle*Mathf.Rad2Deg+90;
           Quaternion rot = Quaternion.Euler(0, 0, angleDegrees);
           GameObject instObject = Instantiate(prefab, pos, rot); */

           float outerVerticesAngle01 = (i) * Mathf.PI * 2  / (numberOfObjects);
           float xOuter01 = Mathf.Cos(outerVerticesAngle01) * radius;
           float yOuter01 = Mathf.Sin(outerVerticesAngle01) * radius;
           Vector3 posOuter01 = new Vector3(xOuter01, yOuter01, 0);
           float outerVerticesAngle02 = (i+1) * Mathf.PI * 2  / (numberOfObjects);
           float xOuter02 = Mathf.Cos(outerVerticesAngle02) * radius;
           float yOuter02 = Mathf.Sin(outerVerticesAngle02) * radius;
           Vector3 posOuter02 = new Vector3(xOuter02, yOuter02, 0);


           float innerVerticesAngle01 = (i) * Mathf.PI * 2  / (numberOfObjects);
           float xInner01 = Mathf.Cos(innerVerticesAngle01) * radiusInner;
           float yInner01 = Mathf.Sin(innerVerticesAngle01) * radiusInner;
           Vector3 posInner01 = new Vector3(xInner01, yInner01, 0);
           float innerVerticesAngle02 = (i+1) * Mathf.PI * 2  / (numberOfObjects);
           float xInner02 = Mathf.Cos(innerVerticesAngle02) * radiusInner;
           float yInner02 = Mathf.Sin(innerVerticesAngle02) * radiusInner;
           Vector3 posInner02 = new Vector3(xInner02, yInner02, 0);

           CreateCrazyCube(posOuter01, posOuter02, posInner01, posInner02);
           /*// Get instantiated mesh
            Mesh mesh = instObject.GetComponent<MeshFilter>().mesh;
            // Randomly change vertices
            Vector3[] vertices = mesh.vertices;
            int p = 0;
            while (p < vertices.Length)
            {
                vertices[p] += new Vector3(0, 10*p, 0);
                p++;
            }
            
            Debug.Log(vertices.Length);
            mesh.vertices = vertices;
            mesh.RecalculateNormals(); */
       }

   }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreateCube() 
    {

        GameObject cubeObject;
        cubeObject = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);

	    Vector3[] vertices;
        int[] triangles;

        vertices = new Vector3[8];

    	// BOTTOM VERTICES
        vertices[0] = new Vector3(0, 0, 0); //BBL
        vertices[1] = new Vector3(1, 0, 0); //BBR
        vertices[2] = new Vector3(0, 0, 1); //BTL
        vertices[3] = new Vector3(1, 0, 1); //BTR
        // TOP VERTICES
        vertices[4] = new Vector3(0, 1, 0); //TBL
        vertices[5] = new Vector3(1, 1, 0); //TBR
        vertices[6] = new Vector3(0, 1, 1); //TTL
        vertices[7] = new Vector3(1, 1, 1); //TTR

        triangles = new int[36];

        // BOTTOM TRIANGLES
        triangles[0] = 1; //BBR
        triangles[1] = 2; //BTL
        triangles[2] = 0; //BBL
        triangles[4] = 2; //BTL
        triangles[3] = 3; //BTR
        triangles[5] = 1; //BBR
        
        // TOP TRIANGLES
        triangles[6] = 4; //TBL
        triangles[7] = 6; //TTL
        triangles[8] = 7; //TTR
        triangles[9] = 5; //TBR
        triangles[10] = 4; //TBL
        triangles[11] = 7; //TTR

        // BACK TRIANGLES
        triangles[12] = 5; //TBR
        triangles[13] = 0; //BBL
        triangles[14] = 4; //TBL
        triangles[15] = 1; //BBR   
        triangles[16] = 0; //BBL
        triangles[17] = 5; //TBR

        // FRONT TRIANGLES
        triangles[18] = 6; //TTL
        triangles[19] = 2; //BTL
        triangles[20] = 3; //BTR
        triangles[21] = 6; //TTL
        triangles[22] = 3; //BTR 
        triangles[23] = 7; //TTR

        // LEFT TRIANGLES
        triangles[24] = 6; //TTL
        triangles[25] = 4; //TBL 
        triangles[26] = 2; //BTL
        triangles[27] = 0; //BBL 
        triangles[28] = 2; //BTL 
        triangles[29] = 4; //TBL 

        // RIGHT TRIANGLES
        triangles[32] = 7; //TTR
        triangles[31] = 5; //TBR
        triangles[30] = 1; //BBR  
        triangles[35] = 1; //BBR  
        triangles[34] = 3; //BTR 
        triangles[33] = 7; //TTR

        Mesh cubeMesh = new Mesh(); 

        cubeObject.GetComponent<MeshFilter>().sharedMesh = cubeMesh;

        cubeMesh.Clear();
        cubeMesh.vertices = vertices;
        cubeMesh.triangles = triangles;

        cubeMesh.RecalculateNormals();

        for(int i = 3; i < 8; i++){
            vertices[i] = vertices[i] + new Vector3(0, 10, 0);
        }
         cubeMesh.Clear();
        cubeMesh.vertices = vertices;
        cubeMesh.triangles = triangles;
        cubeMesh.Optimize();
        cubeMesh.RecalculateNormals();
        
	}

    public void CreateCrazyCube(Vector3 outerCoord01, Vector3 outerCoord02, Vector3 innerCoord01, Vector3 innerCoord02) 
    {

        GameObject cubeObject;
        cubeObject = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);

	    Vector3[] vertices;
        int[] triangles;

        vertices = new Vector3[8];

    	// BOTTOM VERTICES
        vertices[0] = new Vector3(0, 0, 0) + outerCoord01; //BBL
        vertices[1] = new Vector3(0, 0, 0) + outerCoord02; //BBR
        vertices[2] = new Vector3(0, 0, 1) + outerCoord01; //BTL
        vertices[3] = new Vector3(0, 0, 1) + outerCoord02; //BTR
        // TOP VERTICES
        vertices[4] = new Vector3(0, 1, 0) + innerCoord01; //TBL
        vertices[5] = new Vector3(0, 1, 0) + innerCoord02; //TBR
        vertices[6] = new Vector3(0, 1, 1) + innerCoord01; //TTL
        vertices[7] = new Vector3(0, 1, 1) + innerCoord02; //TTR

        triangles = new int[36];

        // BOTTOM TRIANGLES
        triangles[0] = 1; //BBR
        triangles[1] = 2; //BTL
        triangles[2] = 0; //BBL
        triangles[4] = 2; //BTL
        triangles[3] = 3; //BTR
        triangles[5] = 1; //BBR
        
        // TOP TRIANGLES
        triangles[6] = 4; //TBL
        triangles[7] = 6; //TTL
        triangles[8] = 7; //TTR
        triangles[9] = 5; //TBR
        triangles[10] = 4; //TBL
        triangles[11] = 7; //TTR

        // BACK TRIANGLES
        triangles[12] = 5; //TBR
        triangles[13] = 0; //BBL
        triangles[14] = 4; //TBL
        triangles[15] = 1; //BBR   
        triangles[16] = 0; //BBL
        triangles[17] = 5; //TBR

        // FRONT TRIANGLES
        triangles[18] = 6; //TTL
        triangles[19] = 2; //BTL
        triangles[20] = 3; //BTR
        triangles[21] = 6; //TTL
        triangles[22] = 3; //BTR 
        triangles[23] = 7; //TTR

        // LEFT TRIANGLES
        triangles[24] = 6; //TTL
        triangles[25] = 4; //TBL 
        triangles[26] = 2; //BTL
        triangles[27] = 0; //BBL 
        triangles[28] = 2; //BTL 
        triangles[29] = 4; //TBL 

        // RIGHT TRIANGLES
        triangles[32] = 7; //TTR
        triangles[31] = 5; //TBR
        triangles[30] = 1; //BBR  
        triangles[35] = 1; //BBR  
        triangles[34] = 3; //BTR 
        triangles[33] = 7; //TTR

        Mesh cubeMesh = new Mesh(); 

        cubeObject.GetComponent<MeshFilter>().sharedMesh = cubeMesh;

        cubeMesh.Clear();
        cubeMesh.vertices = vertices;
        cubeMesh.triangles = triangles;
        cubeMesh.Optimize();
        cubeMesh.RecalculateNormals();
        
	}
}
