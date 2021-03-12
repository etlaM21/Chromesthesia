using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VRInput : BaseInput
{
    public Camera eventCamera = null;

    public OVRInput.Button clickButton = OVRInput.Button.PrimaryIndexTrigger;
    public OVRInput.Controller controller = OVRInput.Controller.All;

    public GameObject debugCanvas;

    public GameObject canvasPointer = null;

    public GameObject player = null;

    public GameObject SelectLogic;

    protected override void Awake()
    {
        GetComponent<BaseInputModule>().inputOverride = this;
    }

    public override bool GetMouseButton(int button)
    {
        debugCanvas.transform.GetChild(0).GetComponent<Text>().text = "GetMouseButton";
        return OVRInput.Get(clickButton, controller);
    }

    public override bool GetMouseButtonDown(int button)
    {
        debugCanvas.transform.GetChild(0).GetComponent<Text>().text = "GetMouseButtonDown";
        return OVRInput.GetDown(clickButton, controller);
    }

    public override bool GetMouseButtonUp(int button)
    {
        debugCanvas.transform.GetChild(0).GetComponent<Text>().text = "GetMouseButtonUp";
        return OVRInput.GetUp(clickButton, controller);
    }

    public override Vector2 mousePosition
    {
        get
        {
            return new Vector2(eventCamera.pixelWidth / 2, eventCamera.pixelHeight / 2);
        }
    }
    
    void Update(){
        /*
        if(Input.GetKey(KeyCode.S)) {
            debugCanvas.transform.GetChild(0).GetComponent<Text>().text = "KeyCode.S";//Changing text
            Debug.Log("LOL");
        }
        // returns true if the primary button (typically “A”) is currently pressed.
        if(OVRInput.Get(OVRInput.Button.One)) debugCanvas.transform.GetChild(0).GetComponent<Text>().text = "OVRInput.Button.One";

        // returns true if the primary button (typically “A”) was pressed this frame.
        if(OVRInput.GetDown(OVRInput.Button.One)) debugCanvas.transform.GetChild(0).GetComponent<Text>().text = "OVRInput.Button.One";

        // returns true if the “X” button was released this frame.
        if(OVRInput.GetUp(OVRInput.RawButton.X)) debugCanvas.transform.GetChild(0).GetComponent<Text>().text = "OVRInput.RawButton.X";


        // returns a Vector2 of the primary (typically the Left) thumbstick’s current state.
        // (X/Y range of -1.0f to 1.0f)
        // if(OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick)) debugCanvas.transform.GetChild(0).GetComponent<Text>().text = "OVRInput.Axis2D.PrimaryThumbstick";

        // returns true if the primary thumbstick is currently pressed (clicked as a button)
        if(OVRInput.Get(OVRInput.Button.PrimaryThumbstick)) debugCanvas.transform.GetChild(0).GetComponent<Text>().text = "OVRInput.Button.PrimaryThumbstick";

        // returns true if the primary thumbstick has been moved upwards more than halfway.
        // (Up/Down/Left/Right - Interpret the thumbstick as a D-pad).
        if(OVRInput.Get(OVRInput.Button.PrimaryThumbstickUp)) debugCanvas.transform.GetChild(0).GetComponent<Text>().text = "OVRInput.Button.PrimaryThumbstickUp";

        // returns a float of the secondary (typically the Right) index finger trigger’s current state.
        // (range of 0.0f to 1.0f)
        // OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger);

        // returns a float of the left index finger trigger’s current state.
        // (range of 0.0f to 1.0f)
        // OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger);

        // returns true if the left index finger trigger has been pressed more than halfway.
        // (Interpret the trigger as a button).
        if (OVRInput.Get(OVRInput.RawButton.LIndexTrigger)) debugCanvas.transform.GetChild(0).GetComponent<Text>().text = "OVRInput.RawButton.LIndexTrigger";

        // returns true if the secondary gamepad button, typically “B”, is currently touched by the user.
        if(OVRInput.Get(OVRInput.Touch.Two)) debugCanvas.transform.GetChild(0).GetComponent<Text>().text = "OVRInput.Touch.Two";
        */
        if(Main.Instance.inChromesthestia){
            player.GetComponent<PlayerMovement>().moveFwd(OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, controller)*5);
            player.GetComponent<PlayerMovement>().moveBckwd(OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, controller)*5);
            Vector2 thumbStickLeft = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
            player.GetComponent<PlayerMovement>().rotate(thumbStickLeft.x);
            player.GetComponent<PlayerMovement>().moveY(thumbStickLeft.y);
            Vector2 thumbStickRight = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
            player.GetComponent<PlayerMovement>().rotate(thumbStickRight.x);
            player.GetComponent<PlayerMovement>().moveY(thumbStickRight.y);
        }
        else{
            Vector2 thumbStickLeft = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
            Vector2 thumbStickRight = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
            if(thumbStickLeft.y > 0.5f || thumbStickRight.y > 0.5f){
                SelectLogic.GetComponent<SelectSceneLogic>().moveCanvasUp(0.2f);
            }
            if(thumbStickLeft.y < -0.5f || thumbStickRight.y < -0.5f){
                SelectLogic.GetComponent<SelectSceneLogic>().moveCanvasDown(0.2f);
            }
            if(OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, controller) > 0.5f){
                if(canvasPointer != null){
                    canvasPointer.GetComponent<CanvasPointer>().pointTarget.GetComponent<Button>().onClick.Invoke();
                }
            }
            if(OVRInput.GetDown(OVRInput.Button.One)){
                if(canvasPointer != null){
                    canvasPointer.GetComponent<CanvasPointer>().pointTarget.GetComponent<Button>().onClick.Invoke();
                }
            }
            if(OVRInput.GetDown(OVRInput.Button.Two)){
                if(canvasPointer != null){
                    canvasPointer.GetComponent<CanvasPointer>().pointTarget.GetComponent<Button>().onClick.Invoke();
                }
            }
        }
    }   

    public void buttonPressed(){
        debugCanvas.transform.GetChild(0).GetComponent<Text>().text = "Button 01 pressed";
    }

    public void buttonPressed2(){
        debugCanvas.transform.GetChild(0).GetComponent<Text>().text = "Button 02 pressed";
    }


}
