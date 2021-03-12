using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundScaper : MonoBehaviour
{
    Mesh mesh;

    Vector3[] vertices;
    int[] triangles;

    public int rows;

    public int frequencies;

    public bool BlockShape;

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        CreateShape();
        UpdateMesh();
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void CreateShape(){

        if(BlockShape){
            vertices = new Vector3[(frequencies*2) * (rows*2)];

            List<float[]> heightList = new List<float[]>();
             for (int i = 0, z = 0; z < rows; z++){
                float[] heightArray = new float[frequencies];
                for (int x = 0; x < frequencies; x++){
                    heightArray[x] = Random.Range(5f, 20f);
                } 
                heightList.Add(heightArray);
            }

            int heightListRow = 0;
            int zOff = 0;
            for (int i = 0, z = 0; z < rows*2; z++){
                zOff = zOff + z % 2;
                for (int x = 0; x < frequencies; x++){
                    Vector2 circlePosition = calculateCirclePosition(x, frequencies, heightList[heightListRow][x]);
                    vertices[i] = new Vector3(circlePosition.x, circlePosition.y, zOff);
                    // vertices[i] = new Vector3(x, heightList[heightListRow][x], zOff);
                    i++;
                    circlePosition = calculateCirclePosition(x+1, frequencies, heightList[heightListRow][x]);
                    vertices[i] = new Vector3(circlePosition.x, circlePosition.y, zOff);
                    // vertices[i] = new Vector3(x+1, heightList[heightListRow][x], zOff);
                    i++;
                }
                if (z % 2 == 1){
                    heightListRow++;
                }
            }

            triangles = new int[6 * frequencies*4 * rows*2];

            int vert = 0;
            int tris = 0;
            for (int z = 0; z < rows+1; z++){
                for (int x = 1; x < frequencies*2; x++){
                    triangles[tris + 0] = vert + 0;
                    triangles[tris + 1] = vert + frequencies*2 + 0;
                    triangles[tris + 2] = vert + 1;
                    triangles[tris + 3] = vert + 1;
                    triangles[tris + 4] = vert + frequencies*2 + 0;
                    triangles[tris + 5] = vert + frequencies*2 + 1;

                    vert++;
                    tris += 6;
                }
                vert++;
            }
        }
        else {
            vertices = new Vector3[(frequencies + 1) * (rows + 1)];

            for (int i = 0, z = 0; z <= rows; z++){
                for (int x = 0; x <= frequencies; x++){
                    Vector2 circlePosition = calculateCirclePosition(x*2, frequencies*2, 20f);
                    vertices[i] = new Vector3(circlePosition.x, circlePosition.y, z);
                    i++;
                }
            }
            triangles = new int[6 * frequencies * rows];

            int vert = 0;
            int tris = 0;
            for (int z = 0; z < rows; z++){
                for (int x = 0; x < frequencies; x++){
                    triangles[tris + 0] = vert + 0;
                    triangles[tris + 1] = vert + frequencies + 1;
                    triangles[tris + 2] = vert + 1;
                    triangles[tris + 3] = vert + 1;
                    triangles[tris + 4] = vert + frequencies + 1;
                    triangles[tris + 5] = vert + frequencies + 2;

                    vert++;
                    tris += 6;
                }
                vert++;
            }
        }
        

        
        
        
    }

    void UpdateMesh(){
        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
    }

    public Vector2 calculateCirclePosition(int circlePosition, int numberofObjectsInCircle, float radius){
        float angle = circlePosition * Mathf.PI * 2  / (numberofObjectsInCircle);
        float x = Mathf.Cos(angle) * radius;
        float y = Mathf.Sin(angle) * radius;
        Vector2 position = new Vector2(x, y);
        return position;
    }
    
}
