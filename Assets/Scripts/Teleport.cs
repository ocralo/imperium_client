using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{

    /// <summary>
    /// This method is called by the Main Camera when it is gazing at this GameObject and the screen
    /// is touched.
    /// </summary>
    public void OnPointerEnterDoubleClick()
    {
        teleportPlayer();
    }
    public void teleportPlayer()
    {
        GameObject player = GameObject.Find("Player");
        Transform playerT = player.transform;
        playerT.position = new Vector3(this.transform.position.x, playerT.position.y, this.transform.position.z);
    }
}
