using UnityEngine;
using System.Collections;

public class JukeBox : MonoBehaviour {
	
	public AudioClip[] songs;
	
	int index = 0;
	
	// Update is called once per frame
	public void NextSong () {
	
		index = (index + 1) % songs.Length;
		
		gameObject.GetComponent<AudioSource>().Stop();
        gameObject.GetComponent<AudioSource>().clip = songs[index];
        gameObject.GetComponent<AudioSource>().Play();
	}
}
