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

       for (int i = 0; i < numberOfObjects; i++)
       {
           float angle = i * Mathf.PI * 2  / numberOfObjects;
           float x = Mathf.Cos(angle) * radius;
           float y = Mathf.Sin(angle) * radius;
           Vector3 pos = transform.position + new Vector3(x, y, 0);
           float angleDegrees = +angle*Mathf.Rad2Deg+90;
           Quaternion rot = Quaternion.Euler(0, 0, angleDegrees);
           Instantiate(prefab, pos, rot);
       }
   }

    // Update is called once per frame
    void Update()
    {
        
    }
}
