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
using System.Linq;

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
                    case "pointTable":
                        m_GazedAtObject?.SendMessage("OnPointerExit");
                        m_GazedAtObject = hit.transform.gameObject;
                        /* if (Input.GetMouseButtonDown(0))
                        {
                            if (!GeneratePoints.instance.isInit)
                            {
                                GeneratePoints.instance.lineComplete = false;
                                GeneratePoints.instance.initPosX = (int)hit.transform.localPosition.x;
                                GeneratePoints.instance.initPosY = (int)hit.transform.localPosition.z;
                                hit.transform.gameObject.GetComponent<Renderer>().material.color = Color.red;
                                GeneratePoints.instance.isInit = true;
                                GeneratePoints.instance.lineR.SetPosition(GeneratePoints.instance.lineCount, new Vector3((int)hit.transform.position.x, 0, (int)hit.transform.position.z));
                            }
                            else
                            {
                                GeneratePoints.instance.lineR.positionCount++;
                                hit.transform.gameObject.GetComponent<Renderer>().material.color = Color.black;
                                GeneratePoints.instance.finalPosX = (int)hit.transform.localPosition.x;
                                GeneratePoints.instance.finalPosY = (int)hit.transform.localPosition.z;
                                GeneratePoints.instance.isInit = false;
                                GeneratePoints.instance.lineR.SetPosition(GeneratePoints.instance.lineCount + 1, new Vector3((int)hit.transform.position.x, 0, (int)hit.transform.position.z));
                                GeneratePoints.instance.lineComplete = true;
                                GeneratePoints.instance.lineCount++;
                            }
                        } */
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
                    case "pointTable":
                        //m_GazedAtObject?.SendMessage("OnPointerExit");
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
                case "pointTable":

                    break;
                default:
                    break;

            }
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
                case "pointTable":
                    if (!GeneratePoints.instance.isInit)
                    {
                        GeneratePoints.instance.initPosX = (int)m_GazedAtObject.transform.localPosition.x;
                        GeneratePoints.instance.initPosY = (int)m_GazedAtObject.transform.localPosition.z;
                        GeneratePoints.instance.listPointEneable.Add(new Vector3(GeneratePoints.instance.initPosX, 0, GeneratePoints.instance.initPosY));

                        //GeneratePoints.instance.cutrentX = Int32.Parse(m_GazedAtObject.transform.name.Split('-')[0]);
                        //GeneratePoints.instance.currentY = Int32.Parse(m_GazedAtObject.transform.name.Split('-')[1]);
                        foreach (Vector3 childEneable in GeneratePoints.instance.listPointEneable)
                        {
                            foreach (Transform child in GeneratePoints.instance.gameObject.transform)
                            {
                                string auxData = childEneable.x + "-" + childEneable.z;
                                if (child.transform.name == auxData)
                                {
                                    child.gameObject.GetComponent<SphereCollider>().enabled = false;
                                    child.gameObject.transform.gameObject.GetComponent<Renderer>().material.color = Color.yellow;
                                }
                            }
                        }

                        if (GeneratePoints.instance.lineCount < 1)
                        {
                            foreach (Transform child in GeneratePoints.instance.gameObject.transform)
                            {
                                if (GeneratePoints.instance.initPosX == Int32.Parse(child.transform.name.Split('-')[0]) || GeneratePoints.instance.initPosY == Int32.Parse(child.transform.name.Split('-')[1]))
                                {
                                    child.gameObject.GetComponent<SphereCollider>().enabled = true;
                                    child.gameObject.transform.gameObject.GetComponent<Renderer>().material.color = Color.white;
                                }
                                else
                                {
                                    child.gameObject.GetComponent<SphereCollider>().enabled = false;
                                    child.gameObject.transform.gameObject.GetComponent<Renderer>().material.color = Color.blue;
                                }
                            }
                        }
                        m_GazedAtObject.transform.gameObject.GetComponent<Renderer>().material.color = Color.red;
                        GeneratePoints.instance.isInit = true;
                        //GeneratePoints.instance.lineR.SetPosition(GeneratePoints.instance.lineCount, new Vector3((int)m_GazedAtObject.transform.position.x, 0, (int)m_GazedAtObject.transform.position.z));
                        if (GeneratePoints.instance.lineCount < 1) GeneratePoints.instance.lineR.SetPosition(GeneratePoints.instance.lineCount, new Vector3(m_GazedAtObject.transform.position.x, 0, m_GazedAtObject.transform.position.z));
                    }
                    else
                    {
                        GeneratePoints.instance.lineR.positionCount++;
                        m_GazedAtObject.transform.gameObject.GetComponent<Renderer>().material.color = Color.black;
                        GeneratePoints.instance.isInit = false;
                        GeneratePoints.instance.lineR.SetPosition(GeneratePoints.instance.lineCount + 1, new Vector3(m_GazedAtObject.transform.position.x, 0, m_GazedAtObject.transform.position.z));
                        GeneratePoints.instance.lineCount++;
                        GeneratePoints.instance.finalPosX = m_GazedAtObject.transform.localPosition.x;
                        GeneratePoints.instance.finalPosY = m_GazedAtObject.transform.localPosition.z;
                        /*/**/
                        GeneratePoints.instance.initPosX = (int)m_GazedAtObject.transform.localPosition.x;
                        GeneratePoints.instance.initPosY = (int)m_GazedAtObject.transform.localPosition.z;

                        GeneratePoints.instance.listPointEneable.Add(new Vector3(GeneratePoints.instance.initPosX, 0, GeneratePoints.instance.initPosY));

                        foreach (Transform child in GeneratePoints.instance.gameObject.transform)
                        {
                            if (GeneratePoints.instance.initPosX == Int32.Parse(child.transform.name.Split('-')[0]) || GeneratePoints.instance.initPosY == Int32.Parse(child.transform.name.Split('-')[1]))
                            {
                                child.gameObject.GetComponent<SphereCollider>().enabled = true;
                                child.gameObject.transform.gameObject.GetComponent<Renderer>().material.color = Color.white;
                            }
                            else
                            {
                                child.gameObject.GetComponent<SphereCollider>().enabled = false;
                                child.gameObject.transform.gameObject.GetComponent<Renderer>().material.color = Color.blue;
                            }
                        }

                        foreach (Vector3 childEneable in GeneratePoints.instance.listPointEneable)
                        {
                            foreach (Transform child in GeneratePoints.instance.gameObject.transform)
                            {
                                string auxData = childEneable.x + "-" + childEneable.z;
                                if (child.transform.name == auxData)
                                {
                                    child.gameObject.GetComponent<SphereCollider>().enabled = false;
                                    child.gameObject.transform.gameObject.GetComponent<Renderer>().material.color = Color.yellow;
                                }
                            }
                        }

                        string auxDataa = GeneratePoints.instance.listPointEneable[0].x + "-" + GeneratePoints.instance.listPointEneable[0].z;

                        foreach (Transform child in GeneratePoints.instance.gameObject.transform)
                        {
                            if (child.transform.name == auxDataa && GeneratePoints.instance.lineCount > 2)
                            {
                                child.gameObject.GetComponent<SphereCollider>().enabled = true;
                                child.gameObject.transform.gameObject.GetComponent<Renderer>().material.color = Color.cyan;
                            }
                        }

                        if (GeneratePoints.instance.listPointEneable[0] == GeneratePoints.instance.listPointEneable.Last())
                        {
                            GeneratePoints.instance.CreateQuad();
                            Debug.Log("Entr muñeco");
                        }

                        GeneratePoints.instance.isInit = true;
                    }
                    break;
                default:
                    break;

            }
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
