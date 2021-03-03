using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopesImagesArray : MonoBehaviour
{
    public static RopesImagesArray instance;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        instance = this;
    }

    public void StateRenderRopes(int countRopes)
    {
        foreach (Transform child in this.gameObject.transform)
        {
            if (Int32.Parse(child.name) < countRopes)
            {
                child.gameObject.SetActive(true);
            }
            else
            {
                child.gameObject.SetActive(false);
            }
        }
    }
}
