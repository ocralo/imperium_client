using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class changeAreaInputButton : MonoBehaviour
{
    public TextMeshProUGUI textInputArea;
    public GameObject generatePoint;
    public GameObject pointPlayer;
    public GameObject canvasInput;
    public GameObject areaRopes, perimeterRopes;
    public TextMeshProUGUI textInputState;
    public void OnPointerEnterClick(int i)
    {
        if (i == 1)
        {
            if (generatePoint.GetComponent<GeneratePoints>().areaInputnumber > 0)
            {
                generatePoint.GetComponent<GeneratePoints>().areaInputnumber--;
                textInputArea.text = generatePoint.GetComponent<GeneratePoints>().areaInputnumber.ToString();
            }
        }
        else if (i == 2)
        {
            generatePoint.GetComponent<GeneratePoints>().areaInputnumber++;
            textInputArea.text = generatePoint.GetComponent<GeneratePoints>().areaInputnumber.ToString();
        }
        else
        {
            if (generatePoint.GetComponent<GeneratePoints>().areaInputnumber == generatePoint.GetComponent<GeneratePoints>().areaFignumber)
            {
                pointPlayer.GetComponent<PointPlayer>().AddPointsUser(100);
                textInputState.text = "Correcto";
                textInputState.color = new Color32(0, 255, 0, 255);
            }
            else
            {
                pointPlayer.GetComponent<PointPlayer>().AddPointsUser(-10);
                textInputState.text = "Falso";
                textInputState.color = new Color32(255, 0, 0, 255);
            }
            StartCoroutine("WaitCahngecolorTextInput");
            this.gameObject.GetComponent<MeshCollider>().enabled = false;

        }
    }


    IEnumerator WaitCahngecolorTextInput()
    {
        yield return new WaitForSeconds(1.5f);
        textInputState.text = "";
        canvasInput.SetActive(false);
        areaRopes.SetActive(true);
        perimeterRopes.SetActive(true);
    }
}


