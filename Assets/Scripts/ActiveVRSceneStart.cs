using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveVRSceneStart : MonoBehaviour
{
    //Init Variables
    public VrModeController vrModeController;

    // Start is called before the first frame update
    void Start()
    {
        vrModeController.EnterVR();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
