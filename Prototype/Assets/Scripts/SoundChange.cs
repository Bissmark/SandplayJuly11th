using UnityEngine;
using System.Collections;

public class SoundChange : MonoBehaviour 
{
	public GameObject soundEnabledIcon;
	public GameObject soundDisabledIcon;
	public AudioSource music;

	[SerializeField]
	public float muteRatio = 0.01f;

	private float volume = 0;
	private float initialVolume = 0;
	private bool mute = false;

	private void Awake()
	{
		initialVolume = music.volume;
	}
	
	public void SoundMute() 
	{
		if(soundEnabledIcon.activeSelf == false)
		{
			soundEnabledIcon.SetActive(true);
			soundDisabledIcon.SetActive(false);
			mute = false;
		}
		else
		{
			soundEnabledIcon.SetActive(false);
			soundDisabledIcon.SetActive(true);
			mute = true;
		}
	}

	private void Update()
	{
		if ( mute )
		{
			music.volume = Mathf.Clamp(Mathf.Lerp( music.volume, 0f, muteRatio), 0, 1 );
		}
		else
		{
			music.volume = Mathf.Clamp(Mathf.Lerp( music.volume, 1.0f, muteRatio ), 0, 1);
		}
	}
}
