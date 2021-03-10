using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Ajustes : MonoBehaviour
{
    private AudioSource audioSource;

    private float musicVolume = 1f;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void changeVolume(float newVolume)
    {
        PlayerPrefs.SetFloat("volume", newVolume);
        AudioListener.volume = PlayerPrefs.GetFloat("volume");
    }

    public void setVolume(float vol)
    {
        musicVolume = vol;
    }

    public void setUrl(string url)
    {
        GameObject.Find("globalData").GetComponent<GlobalData>().url = url;
    }

    // Update is called once per frame
    void Update()
    {
        audioSource.volume = musicVolume;
    }
}
