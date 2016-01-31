using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour {

	private AudioSource[] audio;

	public static SoundManager _instance;

	// Use this for initialization
	void Start () {
		audio = this.gameObject.GetComponents<AudioSource>();
		playMainTheme();
		if(_instance==null){
			_instance=this;
		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void playMainTheme(){
		audio[0].Play();
	}

	public void playVolcanoAngry(){
		audio[1].Play();
	}

	public void playPeopleGrount(){
		audio[2].Play();
	}

	public void playHappyPeople(){
		audio[3].Play();
	}

	public SoundManager getSoundManager(){
		return _instance;
	}

}
