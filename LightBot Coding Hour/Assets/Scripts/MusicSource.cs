using UnityEngine;
using System.Collections;

public class MusicSource : MonoBehaviour 
{
	void Awake() 
	{
		DontDestroyOnLoad(this.gameObject);
	}
}
