using UnityEngine;
using System.Collections;

public class Trash : MonoBehaviour
{
    AudioSource ass;

    private void Awake()
    {
        Trash[] trashes = FindObjectsOfType<Trash>();

        if (trashes.Length == 1)
        {
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        ass = FindObjectOfType<AudioSource>();
        ass.volume = PlayerPrefs.GetFloat("volume");
        ass.Play();

    }

}
