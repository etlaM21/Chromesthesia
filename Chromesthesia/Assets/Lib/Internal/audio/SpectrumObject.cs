using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectrumObject {
    
    public float TimeInSong { get; set; }
    public float Amplitude { get; set; }
    public float SecondsPerFFTChunk { get; set; } // replace with call to main
    public GameObject Object { get; set; }

    // New for tunnel

    public int indexInRow { get; set; }

    public int numberOfObjects { get; set; } // replace with call to main

    public float radiusOuter { get; set; } // replace with call to main

    public float radiusInner { get; set; } // replace with call to main

    public SpectrumObject () {
        
	}

    public void updateZPosition(){
        this.Object.transform.position = new Vector3(this.Object.transform.position.x, this.Object.transform.position.y, this.songPositionToWorldPosition(this.TimeInSong));
    }
    public float songPositionToWorldPosition(float songTime){
        return (songTime/SecondsPerFFTChunk)/2;
    }

    public void updateScale(){
        this.Object.transform.localScale = new Vector3(this.Object.transform.localScale.x, Amplitude*100+1, this.Object.transform.localScale.z);
    }

    public void updateObject(){
        updateMesh(tunnelMeshPoints());
        updateZPosition();
    }

    public Vector3[] tunnelMeshPoints(){
        float outerVerticesAngle01 = (this.indexInRow) * Mathf.PI * 2  / (this.numberOfObjects);
        float xOuter01 = Mathf.Cos(outerVerticesAngle01) * radiusOuter;
        float yOuter01 = Mathf.Sin(outerVerticesAngle01) * this.radiusOuter;
        Vector3 posOuter01 = new Vector3(xOuter01, yOuter01, 0);
        float outerVerticesAngle02 = (this.indexInRow+1) * Mathf.PI * 2  / (this.numberOfObjects);
        float xOuter02 = Mathf.Cos(outerVerticesAngle02) * this.radiusOuter;
        float yOuter02 = Mathf.Sin(outerVerticesAngle02) * this.radiusOuter;
        Vector3 posOuter02 = new Vector3(xOuter02, yOuter02, 0);


        float innerVerticesAngle01 = (this.indexInRow) * Mathf.PI * 2  / (this.numberOfObjects);
        float xInner01 = Mathf.Cos(innerVerticesAngle01) * (this.radiusOuter-(this.radiusOuter - this.radiusInner)*this.Amplitude);
        float yInner01 = Mathf.Sin(innerVerticesAngle01) *  (this.radiusOuter-(this.radiusOuter - this.radiusInner)*this.Amplitude);
        Vector3 posInner01 = new Vector3(xInner01, yInner01, 0);
        float innerVerticesAngle02 = (this.indexInRow+1) * Mathf.PI * 2  / (this.numberOfObjects);
        float xInner02 = Mathf.Cos(innerVerticesAngle02) *  (this.radiusOuter-(this.radiusOuter - this.radiusInner)*this.Amplitude);
        float yInner02 = Mathf.Sin(innerVerticesAngle02) *  (this.radiusOuter-(this.radiusOuter - this.radiusInner)*this.Amplitude);
        Vector3 posInner02 = new Vector3(xInner02, yInner02, 0);

        Vector3[] result = new Vector3[]
        {
            posOuter01, posOuter02, posInner01, posInner02
        };

        return result;
    }

    public void updateMesh(Vector3[] meshPoints){
        Mesh mesh = new Mesh(); 
        this.Object.GetComponent<MeshFilter>().sharedMesh = mesh;
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
            vertices[indexesBottomLeftVertices[i]] += meshPoints[0];
        }
        for(int i = 0; i < indexesBottomRightVertices.Length; i++){
            vertices[indexesBottomRightVertices[i]] += meshPoints[1];
        }
        for(int i = 0; i < indexesTopLeftVertices.Length; i++){
            vertices[indexesTopLeftVertices[i]] += meshPoints[2];
        }
        for(int i = 0; i < indexesTopRightVertices.Length; i++){
            vertices[indexesTopRightVertices[i]] += meshPoints[3];
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
    }

}