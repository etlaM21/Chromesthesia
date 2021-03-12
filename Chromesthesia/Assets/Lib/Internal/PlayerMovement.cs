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
        // Forward / Backward
        float fwdSpeed = 1f;
        float bckwdSpeed = 2f; // Backward should be a little higher, because Audio is playing back. So if we go back 1 sec every sec, we actually stay in place
        if (Input.GetKey(KeyCode.Q)) {
            Main.Instance.AudioSource.time = Main.Instance.AudioSource.time + fwdSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.E)) {
            Main.Instance.AudioSource.time = Main.Instance.AudioSource.time - bckwdSpeed * Time.deltaTime;
        }
    }

    public void rotateLeft(){
        transform.eulerAngles = new Vector3(0,0, transform.eulerAngles.z - rotationSpeed * Time.deltaTime);
    }

    public void rotateRight(){
        transform.eulerAngles = new Vector3(0,0, transform.eulerAngles.z + rotationSpeed * Time.deltaTime);
    }

    public void rotate(float amount){
        transform.eulerAngles = new Vector3(0,0, transform.eulerAngles.z + (rotationSpeed * amount) * Time.deltaTime);
    }

    public void moveDown(){
        PlayerPosition.transform.localPosition =  PlayerPosition.transform.localPosition  + new Vector3(0, -movementSpeed * Time.deltaTime, 0);
    }

    public void moveUp(){
        PlayerPosition.transform.localPosition =  PlayerPosition.transform.localPosition + new Vector3(0, movementSpeed * Time.deltaTime, 0);
    }

    public void moveY(float amount){
         PlayerPosition.transform.localPosition =  PlayerPosition.transform.localPosition + new Vector3(0, (movementSpeed * amount) * Time.deltaTime, 0);
    }

    public void moveFwd(float speed){
        Main.Instance.AudioSource.time = Main.Instance.AudioSource.time + speed * Time.deltaTime;

    }

    public void moveBckwd(float speed){
        Main.Instance.AudioSource.time = Main.Instance.AudioSource.time - speed * Time.deltaTime;
    }
}
