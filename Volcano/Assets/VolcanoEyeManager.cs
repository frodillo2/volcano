using UnityEngine;
using System.Collections;

public class VolcanoEyeManager : MonoBehaviour {

	public static VolcanoEyeManager _instance;

	public GameObject normalEye;
	public GameObject happyEye;
	public GameObject angryEye;

	private float eyeTimer=0;
	private float eyeTimerMax=1.5f;
	private bool changeStateFlag=false;
	private bool normalEyeFlag=true;
	private bool happyEyeFlag=false;
	private bool angryEyeFlag=false;

	// Use this for initialization
	void Awake () {
		_instance=this;
	}

	public void chageEyeState(){
		changeStateFlag=true;
		normalEye.SetActive(normalEyeFlag);
		happyEye.SetActive(happyEyeFlag);
		angryEye.SetActive(angryEyeFlag);
	}

	// Update is called once per frame
	void Update () {
		if(changeStateFlag){
			eyeTimer+=1*Time.deltaTime;
			if(eyeTimer>eyeTimerMax){
				setnormalEyeFlagTrue();
				eyeTimer=0;
				changeStateFlag=false;
			}
		}
	}

	public void setnormalEyeFlagTrue(){
		normalEyeFlag=true;
		happyEyeFlag=false;
		angryEyeFlag=false;
		chageEyeState();
	}

	public void sethappyEyeFlagTrue(){
		normalEyeFlag=false;
		happyEyeFlag=true;
		angryEyeFlag=false;
		chageEyeState();
	}

	public void setAngryEyeFlagTrue(){
		normalEyeFlag=false;
		happyEyeFlag=false;
		angryEyeFlag=true;
		chageEyeState();
	}

	public static VolcanoEyeManager getInstance(){
		return _instance;
	}
}
