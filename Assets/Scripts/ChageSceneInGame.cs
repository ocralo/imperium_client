using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChageSceneInGame : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnPointerEnterDoubleClick(int i)
    {
        Debug.Log(i);
        if (i != 100)
        {
            this.gameObject.GetComponent<changeScene>().ViewLoadScene(i);
        }
        else
        {
            this.gameObject.GetComponent<changeScene>().ViewLoadScene(0);
        }
    }
}
