using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedFoveatedRendering : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        OVRManager.fixedFoveatedRenderingLevel = OVRManager.FixedFoveatedRenderingLevel.High; // it's the maximum foveation level
        OVRManager.useDynamicFixedFoveatedRendering = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
