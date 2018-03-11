using UnityEngine;
using System.Collections;

public class Trash : MonoBehaviour 
{
	AudioSource ass;


	void Start () 
	{
		ass = FindObjectOfType<AudioSource> ();
		ass.Play ();
	}

}
