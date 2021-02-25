//-----------------------------------------------------------------------
// <copyright file="CameraPointer.cs" company="Google LLC">
// Copyright 2020 Google LLC. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.XR.Management;

/// <summary>
/// Sends messages to gazed GameObject.
/// </summary>
public class CameraPointer : MonoBehaviour
{
    private const float k_MaxDistance = 10;
    public float timeToSelect = 3.0f;
    public float timeToSelectFinal = 3.0f;
    public bool selecObj = true;
    private GameObject m_GazedAtObject = null;

    private WaitForSeconds doubleClickTreashHold = new WaitForSeconds(1);
    private int clickCount;
    void Start()
    {
        //StartXR();
        //StartCoroutine(StartXRIe());
    }

    /// <summary>
    /// Update is called once per frame.
    /// </summary>
    public void Update()
    {
        // Casts ray towards camera's forward direction, to detect if a GameObject is being gazed
        // at.
        RaycastHit hit;

#if UNITY_EDITOR

        // Bit shift the index of the layer (8) to get a bit mask
        Vector3 mouse = Input.mousePosition;
        Ray castPoint = Camera.main.ScreenPointToRay(mouse);

        if (Physics.Raycast(castPoint, out hit, Mathf.Infinity))
        {
            Debug.Log(hit.transform.tag);

            // GameObject detected in front of the camera.
            if (m_GazedAtObject != hit.transform.gameObject)
            {
                switch (hit.transform.tag)
                {
                    case "interactive":
                        m_GazedAtObject?.SendMessage("OnPointerExit");
                        m_GazedAtObject = hit.transform.gameObject;
                        break;
                    case "areaObject":
                        m_GazedAtObject?.SendMessage("OnPointerExit");
                        m_GazedAtObject = hit.transform.gameObject;

                        break;
                    default:
                        break;
                }
            }
            else
            {
                if (!selecObj)
                {
                    onPointerObserver();
                }
                switch (hit.transform.tag)
                {
                    case "interactive":
                        m_GazedAtObject?.SendMessage("OnPointerEnter");
                        break;
                    case "areaObject":
                        if (selecObj)
                        {
                            m_GazedAtObject?.SendMessage("OnPointerEnter", this.transform);
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        else
        {
            m_GazedAtObject?.SendMessage("OnPointerExit");
            m_GazedAtObject = null;
        }
        if (Input.GetMouseButtonDown(0))
        {
            OnPointerClick(m_GazedAtObject);
            m_GazedAtObject = null;
        }

#elif UNITY_ANDROID

        if (Physics.Raycast(transform.position, transform.forward, out hit, k_MaxDistance))
        {

            // GameObject detected in front of the camera.
            if (m_GazedAtObject != hit.transform.gameObject)
            {
                onPointerObserver();
                switch (hit.transform.tag)
                {
                    case "interactive":
                        m_GazedAtObject?.SendMessage("OnPointerExit");
                        m_GazedAtObject = hit.transform.gameObject;
                        m_GazedAtObject?.SendMessage("OnPointerEnter");
                        break;
                    case "areaObject":
                        m_GazedAtObject?.SendMessage("OnPointerExit");
                        m_GazedAtObject = hit.transform.gameObject;
                        break;
                    default:
                        break;

                }
            }else{

                if (!selecObj)
                {
                    onPointerObserver();
                }
                switch (hit.transform.tag)
                {
                    case "interactive":
                        m_GazedAtObject?.SendMessage("OnPointerEnter");
                        break;
                    case "areaObject":
                        if (selecObj)
                        {
                            m_GazedAtObject?.SendMessage("OnPointerEnter", this.transform);
                        }
                        break;
                    default:
                        break;
                }
            
            }
        }else
        {
            // No GameObject detected in front of the camera.
            m_GazedAtObject?.SendMessage("OnPointerExit");
            m_GazedAtObject = null;
        }
            // Checks for screen touches.
        if (Google.XR.Cardboard.Api.IsTriggerPressed)
        {
            OnPointerClick(m_GazedAtObject);
            m_GazedAtObject = null;
        }
#endif
    }
    //metodo para detectar el tiempo que se mira un objeto
    private void onPointerObserver()
    {
        Debug.Log(timeToSelect);
        if (timeToSelect < 0)
        {
            selecObj = true;
            timeToSelect = timeToSelectFinal;
        }
        else
        {
            timeToSelect -= Time.deltaTime;
        }

    }

    //metodo para detectar el doble click o doble toque
    private void OnPointerClick([Optional] GameObject hit)
    {
        clickCount++;
        if (clickCount == 2)
        {
            switch (m_GazedAtObject.transform.tag)
            {
                case "interactive":
                    m_GazedAtObject?.SendMessage("teleportPlayer");
                    break;
                case "areaObject":
                    m_GazedAtObject?.SendMessage("OnPointerEnterDoubleClick", hit.transform);
                    selecObj = false;
                    timeToSelect = timeToSelectFinal;
                    break;
                default:
                    break;

            }
            print("double click!");
            clickCount = 0;
        }
        else
        {
            StartCoroutine(TickDown());
            switch (m_GazedAtObject.transform.tag)
            {
                case "interactive":
                    //m_GazedAtObject?.SendMessage("OnPointerEnterClik", hit.transform);
                    break;
                case "areaObject":
                    m_GazedAtObject?.SendMessage("OnPointerEnterClik", m_GazedAtObject.transform);
                    break;
                default:
                    break;

            }
            print("click!");
        }
    }

    private IEnumerator TickDown()
    {
        yield return doubleClickTreashHold;
        if (clickCount > 0)
        {
            clickCount--;
        }
    }

}
