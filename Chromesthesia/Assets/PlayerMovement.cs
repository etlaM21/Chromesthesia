using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float rotationSpeed = 45f;

    public float movementSpeed = 5f;
    public GameObject PlayerPosition;

    void Update()
    {
        // Rotation
        if (Input.GetKey(KeyCode.A)) {
            transform.eulerAngles = new Vector3(0,0, transform.eulerAngles.z - rotationSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.D)) {
            transform.eulerAngles = new Vector3(0,0, transform.eulerAngles.z + rotationSpeed * Time.deltaTime);
        }
        // Height
        if (Input.GetKey(KeyCode.W)) {
            PlayerPosition.transform.localPosition =  PlayerPosition.transform.localPosition + new Vector3(0, movementSpeed * Time.deltaTime, 0);
        }

        if (Input.GetKey(KeyCode.S)) {
            PlayerPosition.transform.localPosition =  PlayerPosition.transform.localPosition  + new Vector3(0, -movementSpeed * Time.deltaTime, 0);
        }

    }
}
