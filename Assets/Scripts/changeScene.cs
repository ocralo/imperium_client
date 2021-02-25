using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class changeScene : MonoBehaviour
{
    public Scrollbar slider;
    public GameObject canvasCharge;
    public GameObject canvasInit;

    public void ViewLoadScene(int i)
    {
        canvasInit.SetActive(false);
        canvasCharge.SetActive(true);
        Debug.Log(i);
        StartCoroutine(LoadYourAsyncScene(i));
    }

    public void DebugP()
    {
        Debug.Log("hola guapisimos");
    }
    IEnumerator LoadYourAsyncScene(int i)
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(i);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            slider.size = asyncLoad.progress;
            yield return null;
        }
    }
}
