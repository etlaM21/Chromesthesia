using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CanvasPointer : MonoBehaviour
{
    public float defaultLength = 3.0f;

    public EventSystem eventSystem = null;
    public StandaloneInputModule inputModule = null;

    private LineRenderer lineRenderer = null;

    public GameObject pointTarget = null;

    public GameObject debugCanvas;

    public GameObject selectCanvas;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        UpdateLength();
        pointTarget = UpdateTarget();

        GameObject[] buttons = GameObject.FindGameObjectsWithTag("SongSelectButton");
        foreach (GameObject btn in buttons)
        {
            btn.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1);
        }
        pointTarget.GetComponent<Image>().color = new Color(0,250,255,1);
        // Debug
        debugCanvas.transform.GetChild(0).GetComponent<Text>().text = "RayTarget: " + pointTarget.name;

    }

    public GameObject UpdateTarget(){
        // Get data
        PointerEventData eventData = new PointerEventData(eventSystem);
        eventData.position = inputModule.input.mousePosition;
        // Raycast w/ data
        List<RaycastResult> results = new List<RaycastResult>();
        eventSystem.RaycastAll(eventData, results);

        // Get closest
        RaycastResult closestResult = FindFirstRaycast(results);
        return closestResult.gameObject;
    }

    private void UpdateLength()
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, GetEnd());
    }

    private Vector3 GetEnd()
    {
        float distance = GetCanvasDistance();
        Vector3 endPosition = CalcEnd(defaultLength);

        if(distance != 0.0f){
            endPosition = CalcEnd(distance);
        }

        return endPosition;
    }
    private float GetCanvasDistance()
    {
        // Get data
        PointerEventData eventData = new PointerEventData(eventSystem);
        eventData.position = inputModule.input.mousePosition;
        // Raycast w/ data
        List<RaycastResult> results = new List<RaycastResult>();
        eventSystem.RaycastAll(eventData, results);

        // Get closest
        RaycastResult closestResult = FindFirstRaycast(results);
        float distance = closestResult.distance;

        return distance;
    }

    private RaycastResult FindFirstRaycast(List<RaycastResult> results)
    { 
        foreach(RaycastResult result in results){
            if(!result.gameObject){
                continue;
            }
            return result;
        }
        return new RaycastResult();
    }

    private Vector3 CalcEnd(float length)
    {
        return transform.position + (transform.forward * length);
    }
}
