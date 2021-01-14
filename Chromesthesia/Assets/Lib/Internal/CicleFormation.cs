using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CicleFormation : MonoBehaviour
{
    // Start is called before the first frame update
    // Instantiates prefabs in a circle formation
   public GameObject prefab;

   public List<GameObject> objectList;

   public List<List<GameObject>> tunnelList;
   public int numberOfObjects = 32;

   public int tunnelRows = 6;
   public float radius = 20f;

   public float radiusInner = 5f;
   void Start()
   {
    
        objectList = new List<GameObject>();
        tunnelList = new List<List<GameObject>>();
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
        for (int p = 0; p < tunnelRows; p++) 
        {
            objectList = new List<GameObject>();
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

                    /*
                    * Script to draw meshes of objects in cicrcle -> problem with normalmaps
                    */
                    /*
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

                createNiceCrazyMazyCube(posOuter01, posOuter02, posInner01, posInner02);
                */

                    Vector2 outerCirclePosLeft = calculateCirclePosition(numberOfObjects/4*3, numberOfObjects, radius);
                    Vector2 outerCirclePosRight = calculateCirclePosition(numberOfObjects/4*3+1, numberOfObjects, radius);
                    Vector3 outerPosLeft = new Vector3(outerCirclePosLeft.x, outerCirclePosLeft.y, 0);
                    Vector3 outerPosRight = new Vector3(outerCirclePosRight.x, outerCirclePosRight.y, 0);

                    float randomInnerRadius = Random.Range(5f, 20f);
                    Vector2 innerCirclePosLeft = calculateCirclePosition(numberOfObjects/4*3, numberOfObjects, randomInnerRadius);
                    Vector2 innerCirclePosRight = calculateCirclePosition(numberOfObjects/4*3+1, numberOfObjects, randomInnerRadius);
                    Vector3 innerPosLeft = new Vector3(innerCirclePosLeft.x, innerCirclePosLeft.y, 0);
                    Vector3 innerPosRight = new Vector3(innerCirclePosRight.x, innerCirclePosRight.y, 0);

                    objectList.Add(createNiceCrazyMazyCube(outerPosLeft, outerPosRight, innerPosLeft, innerPosRight));
                    
                //createNiceCrazyCube();
            }
            for (int o = 0; o < objectList.Count; o++)
            {
                float angle = o * Mathf.PI * 2  / objectList.Count;
                float x = Mathf.Cos(angle) * 0;
                float y = Mathf.Sin(angle) * 0;
                Vector3 pos = transform.position + new Vector3(x, y, 0);
                float angleDegrees = +angle*Mathf.Rad2Deg+90;
                Quaternion rot = Quaternion.Euler(0, 0, angleDegrees);
                objectList[o].transform.position = pos;
                objectList[o].transform.rotation = rot;
                
            }
            tunnelList.Add(objectList);
            Debug.Log("tunnelList.Count = " + tunnelList.Count);
        }
        
        for (int i = 0; i < tunnelList.Count; i++){
            for (int o = 0; o < tunnelList[i].Count; o++)
            {
                Debug.Log("tunnelList[" + i + "][" + o + "].transform.position = " + tunnelList[i][o].transform.position);
                tunnelList[i][o].transform.position = new Vector3(tunnelList[i][o].transform.position.x, tunnelList[i][o].transform.position.y, (float)(i-tunnelRows/2));
            }
        }
   }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector2 calculateCirclePosition(int circlePosition, int numberofObjectsInCircle, float radius){
        float angle = circlePosition * Mathf.PI * 2  / (numberofObjectsInCircle);
        float x = Mathf.Cos(angle) * radius;
        float y = Mathf.Sin(angle) * radius;
        Vector2 position = new Vector2(x, y);
        return position;
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

    public void createNiceCrazyCube(){
        // You can change that line to provide another MeshFilter
        /* MeshFilter filter = gameObject.AddComponent< MeshFilter >();
        Mesh mesh = filter.mesh; */
        GameObject cubeObject;
        cubeObject = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
        Mesh mesh = new Mesh(); 
        cubeObject.GetComponent<MeshFilter>().sharedMesh = mesh;
        mesh.Clear();
        
        float length = 1f;
        float width = 1f;
        float height = 1f;
        
        #region Vertices
        Vector3 p0 = new Vector3( -length * .5f,	-width * .5f, height * .5f );
        Vector3 p1 = new Vector3( length * .5f, 	-width * .5f, height * .5f );
        Vector3 p2 = new Vector3( length * .5f, 	-width * .5f, -height * .5f );
        Vector3 p3 = new Vector3( -length * .5f,	-width * .5f, -height * .5f );	
        
        Vector3 p4 = new Vector3( -length * .5f,	width * .5f,  height * .5f );
        Vector3 p5 = new Vector3( length * .5f, 	width * .5f,  height * .5f );
        Vector3 p6 = new Vector3( length * .5f, 	width * .5f,  -height * .5f );
        Vector3 p7 = new Vector3( -length * .5f,	width * .5f,  -height * .5f );
        
        Vector3[] vertices = new Vector3[]
        {
            // Bottom
            p0, p1, p2, p3,
        
            // Left
            p7, p4, p0, p3,
        
            // Front
            p4, p5, p1, p0,
        
            // Back
            p6, p7, p3, p2,
        
            // Right
            p5, p6, p2, p1,
        
            // Top
            p7, p6, p5, p4
        };

        int[] indexesBottomVertices = new int[] {
             // Bottom
            0, 1, 2, 3,
        
            // Left
            6, 7,
        
            // Front
            10, 11,
        
            // Back
            14, 15,
        
            // Right
            18, 19
        
            // Top
        };

        int[] indexesTopVertices = new int[] {
            // Bottom
        
            // Left
            4, 5,
    
            // Front
            8, 9,
        
            // Back
            12, 13,
        
            // Right
            16, 17,
        
            // Top
            20, 21, 22, 23
        };
        
        int[] indexesBottomLeftVertices = new int[] {
            // Bottom
            0,  3,
        
            // Left
            6, 7,
        
            // Front
            11,
        
            // Back
           14
        };

        int[] indexesBottomRightVertices = new int[] {
            // Bottom
            1, 2,
        
            // Front
            10,
        
            // Back
            15,
        
            // Right
            18, 19
        };

        int[] indexesTopLeftVertices = new int[] {                
            // Left
            4, 5, 
        
            // Front
            8,
        
            // Back
            13,
        
            // Top
            20, 23
        };

        int[] indexesTopRightVertices = new int[] {                  
            // Front
            9,
        
            // Back
            12,
        
            // Right
            16, 17,
        
            // Top
            21, 22,
        };

        #endregion
        
        #region Normales
        Vector3 up 	= Vector3.up;
        Vector3 down 	= Vector3.down;
        Vector3 front 	= Vector3.forward;
        Vector3 back 	= Vector3.back;
        Vector3 left 	= Vector3.left;
        Vector3 right 	= Vector3.right;
        
        Vector3[] normales = new Vector3[]
        {
            // Bottom
            down, down, down, down,
        
            // Left
            left, left, left, left,
        
            // Front
            front, front, front, front,
        
            // Back
            back, back, back, back,
        
            // Right
            right, right, right, right,
        
            // Top
            up, up, up, up
        };
        #endregion	
        
        #region UVs
        Vector2 _00 = new Vector2( 0f, 0f );
        Vector2 _10 = new Vector2( 1f, 0f );
        Vector2 _01 = new Vector2( 0f, 1f );
        Vector2 _11 = new Vector2( 1f, 1f );
        
        Vector2[] uvs = new Vector2[]
        {
            // Bottom
            _11, _01, _00, _10,
        
            // Left
            _11, _01, _00, _10,
        
            // Front
            _11, _01, _00, _10,
        
            // Back
            _11, _01, _00, _10,
        
            // Right
            _11, _01, _00, _10,
        
            // Top
            _11, _01, _00, _10,
        };
        #endregion
        
        #region Triangles
        int[] triangles = new int[]
        {
            // Bottom
            3, 1, 0,
            3, 2, 1,			
        
            // Left
            3 + 4 * 1, 1 + 4 * 1, 0 + 4 * 1,
            3 + 4 * 1, 2 + 4 * 1, 1 + 4 * 1,
        
            // Front
            3 + 4 * 2, 1 + 4 * 2, 0 + 4 * 2,
            3 + 4 * 2, 2 + 4 * 2, 1 + 4 * 2,
        
            // Back
            3 + 4 * 3, 1 + 4 * 3, 0 + 4 * 3,
            3 + 4 * 3, 2 + 4 * 3, 1 + 4 * 3,
        
            // Right
            3 + 4 * 4, 1 + 4 * 4, 0 + 4 * 4,
            3 + 4 * 4, 2 + 4 * 4, 1 + 4 * 4,
        
            // Top
            3 + 4 * 5, 1 + 4 * 5, 0 + 4 * 5,
            3 + 4 * 5, 2 + 4 * 5, 1 + 4 * 5,
        
        };
        #endregion
        
        mesh.vertices = vertices;
        mesh.normals = normales;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        
        mesh.RecalculateBounds();
        mesh.Optimize();
    }

    public GameObject createNiceCrazyMazyCube(Vector3 outerCoord01, Vector3 outerCoord02, Vector3 innerCoord01, Vector3 innerCoord02){
        // You can change that line to provide another MeshFilter
        /* MeshFilter filter = gameObject.AddComponent< MeshFilter >();
        Mesh mesh = filter.mesh; */
        GameObject cubeObject;
        cubeObject = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
        Mesh mesh = new Mesh(); 
        cubeObject.GetComponent<MeshFilter>().sharedMesh = mesh;
        mesh.Clear();
        
        /*float length = 1f;
        float width = 1f; */
        float height = 1f;
        float length = 0f;
        float width = 0f;
        
        #region Vertices
        Vector3 p0 = new Vector3( -length * .5f,	-width * .5f, height * .5f );
        Vector3 p1 = new Vector3( length * .5f, 	-width * .5f, height * .5f );
        Vector3 p2 = new Vector3( length * .5f, 	-width * .5f, -height * .5f );
        Vector3 p3 = new Vector3( -length * .5f,	-width * .5f, -height * .5f );	
        
        Vector3 p4 = new Vector3( -length * .5f,	width * .5f,  height * .5f );
        Vector3 p5 = new Vector3( length * .5f, 	width * .5f,  height * .5f );
        Vector3 p6 = new Vector3( length * .5f, 	width * .5f,  -height * .5f );
        Vector3 p7 = new Vector3( -length * .5f,	width * .5f,  -height * .5f );
        
        Vector3[] vertices = new Vector3[]
        {
            // Bottom
            p0, p1, p2, p3,
        
            // Left
            p7, p4, p0, p3,
        
            // Front
            p4, p5, p1, p0,
        
            // Back
            p6, p7, p3, p2,
        
            // Right
            p5, p6, p2, p1,
        
            // Top
            p7, p6, p5, p4
        };

        int[] indexesBottomVertices = new int[] {
             // Bottom
            0, 1, 2, 3,
        
            // Left
            6, 7,
        
            // Front
            10, 11,
        
            // Back
            14, 15,
        
            // Right
            18, 19
        
            // Top
        };

        int[] indexesTopVertices = new int[] {
            // Bottom
        
            // Left
            4, 5,
    
            // Front
            8, 9,
        
            // Back
            12, 13,
        
            // Right
            16, 17,
        
            // Top
            20, 21, 22, 23
        };
        
        int[] indexesBottomLeftVertices = new int[] {
            // Bottom
            0,  3,
        
            // Left
            6, 7,
        
            // Front
            11,
        
            // Back
           14
        };

        int[] indexesBottomRightVertices = new int[] {
            // Bottom
            1, 2,
        
            // Front
            10,
        
            // Back
            15,
        
            // Right
            18, 19
        };

        int[] indexesTopLeftVertices = new int[] {                
            // Left
            4, 5, 
        
            // Front
            8,
        
            // Back
            13,
        
            // Top
            20, 23
        };

        int[] indexesTopRightVertices = new int[] {                  
            // Front
            9,
        
            // Back
            12,
        
            // Right
            16, 17,
        
            // Top
            21, 22,
        };

        #endregion
        
        #region VerticesManipulation
        //Vector3 outerCoord01, Vector3 outerCoord02, Vector3 innerCoord01, Vector3 innerCoord02;
        for(int i = 0; i < indexesBottomLeftVertices.Length; i++){
            vertices[indexesBottomLeftVertices[i]] += outerCoord01;
        }
        for(int i = 0; i < indexesBottomRightVertices.Length; i++){
            vertices[indexesBottomRightVertices[i]] += outerCoord02;
        }
        for(int i = 0; i < indexesTopLeftVertices.Length; i++){
            vertices[indexesTopLeftVertices[i]] += innerCoord01;
        }
        for(int i = 0; i < indexesTopRightVertices.Length; i++){
            vertices[indexesTopRightVertices[i]] += innerCoord02;
        }
        #endregion

        #region Normales
        Vector3 up 	= Vector3.up;
        Vector3 down 	= Vector3.down;
        Vector3 front 	= Vector3.forward;
        Vector3 back 	= Vector3.back;
        Vector3 left 	= Vector3.left;
        Vector3 right 	= Vector3.right;
        
        Vector3[] normales = new Vector3[]
        {
            // Bottom
            down, down, down, down,
        
            // Left
            left, left, left, left,
        
            // Front
            front, front, front, front,
        
            // Back
            back, back, back, back,
        
            // Right
            right, right, right, right,
        
            // Top
            up, up, up, up
        };
        #endregion	
        
        #region UVs
        Vector2 _00 = new Vector2( 0f, 0f );
        Vector2 _10 = new Vector2( 1f, 0f );
        Vector2 _01 = new Vector2( 0f, 1f );
        Vector2 _11 = new Vector2( 1f, 1f );
        
        Vector2[] uvs = new Vector2[]
        {
            // Bottom
            _11, _01, _00, _10,
        
            // Left
            _11, _01, _00, _10,
        
            // Front
            _11, _01, _00, _10,
        
            // Back
            _11, _01, _00, _10,
        
            // Right
            _11, _01, _00, _10,
        
            // Top
            _11, _01, _00, _10,
        };
        #endregion
        
        #region Triangles
        int[] triangles = new int[]
        {
            // Bottom
            3, 1, 0,
            3, 2, 1,			
        
            // Left
            3 + 4 * 1, 1 + 4 * 1, 0 + 4 * 1,
            3 + 4 * 1, 2 + 4 * 1, 1 + 4 * 1,
        
            // Front
            3 + 4 * 2, 1 + 4 * 2, 0 + 4 * 2,
            3 + 4 * 2, 2 + 4 * 2, 1 + 4 * 2,
        
            // Back
            3 + 4 * 3, 1 + 4 * 3, 0 + 4 * 3,
            3 + 4 * 3, 2 + 4 * 3, 1 + 4 * 3,
        
            // Right
            3 + 4 * 4, 1 + 4 * 4, 0 + 4 * 4,
            3 + 4 * 4, 2 + 4 * 4, 1 + 4 * 4,
        
            // Top
            3 + 4 * 5, 1 + 4 * 5, 0 + 4 * 5,
            3 + 4 * 5, 2 + 4 * 5, 1 + 4 * 5,
        
        };
        #endregion
        
        mesh.vertices = vertices;
        mesh.normals = normales;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        
        mesh.RecalculateBounds();
        mesh.Optimize();

        return cubeObject;
    }
}
