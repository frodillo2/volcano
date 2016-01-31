using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	public GameObject personPrefab;
	public GameObject volcan;
	public List<Person> persons  =  new List<Person>();
	private int remainingGoodAnswers;
	public int lives = 3;
	private int currentLives;
	public GameObject spawnPointsParent;
	List<Transform> spawnPoints = new List<Transform>();
	float lavaHeight = 1f;

	private int level;
	public bool gameFinished;
	public bool gameStarted;

	//numero de personas que aparecen 
	int[] minFollowers={3,4,5,5,6,8,12,5,5,6,11,10,5,5,8,8,10,12,10};
	int[] maxFollowers = { 5, 8, 10, 9, 11, 10, 15, 6, 6, 14, 18, 16, 7, 7, 12, 12, 20, 20, 20 };
	//numero de condiciones
	int[] minCondition={1,1,1,2,2,1,2,1,1,1,1,2,3,3,2,2,2,2,3};						
	int[] maxCondition = { 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 3 };						
	//Numero de personajes validos
	float[] minValidPercentage={30,25,20,20,20,25,30,20,20,20,20,20,30,30,30,20,20,20,10};
	float[] maxValidPercentage = { 60, 50, 40, 30, 30, 30, 40, 30, 30, 40, 30, 25, 40, 40, 40, 30, 30, 30, 20 };
	//Numero de preguntas negativas
	int[] minNegation={0,0,0,0,0,0,0,1,1,0,0,1,0,1,0,1,0,1,1};
	int[] maxNegation = { 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2 };

	IEnumerator Start(){

		yield return new WaitForSeconds (0.5f);

		while( true ){
			if (Input.GetMouseButtonDown (0)) {
				print("preiosno");
				break;
			}
			yield return 0;
		}

		instance = this;
		for( int i = 0; i < spawnPointsParent.transform.childCount; i++){
			spawnPoints.Add( spawnPointsParent.transform.GetChild(i));
		}
		currentLives = lives;
		personPrefab = Resources.Load ("PersonPrefab") as GameObject;
		loadGame();

	}
		
	void loadGame(){

		QuestionController.instance.defineConditions (2);

		int n = UnityEngine.Random.Range (1, 10);
		print ("validos: " + n);
		AparecerMonoCoroutine(n,10-n);

	}

	void AparecerMonoCoroutine(int valido, int invalido){

		List<Transform> remainingPos = new List<Transform> (spawnPoints);

		remainingGoodAnswers = 0;
		for(int i = 0; i <valido; i++){
			int r = UnityEngine.Random.Range (0,remainingPos.Count-1);
			Person newPerson = monoValido(remainingPos[r].position);
			remainingPos.RemoveAt (r);

			persons.Add (newPerson);
			if (QuestionController.instance.IsValid (newPerson)) {
				remainingGoodAnswers++;
			}
		}

		for(int i = 0; i <invalido; i++){
			int r = UnityEngine.Random.Range (0,remainingPos.Count-1);
			Person newPerson = monoInvalido (remainingPos [r].position);
			remainingPos.RemoveAt (r);

			persons.Add (newPerson);
			if (QuestionController.instance.IsValid (newPerson)) {
				remainingGoodAnswers++;
			}

		}

	}
	/*
	 * Va imprimieno monos en la pantalla
	 */
	Person apereceMono( List<Condition.Hair> hairs, List<Condition.Pant>  pants, List<Condition.Skin> skins, Vector3 pos) {

		GameObject instanciaPerson = Instantiate<GameObject>(personPrefab);
		Person person = instanciaPerson.GetComponent<Person> ();

		int nHair = hairs.Count;
		int nPant = pants.Count;
		int nSkin = skins.Count;

		int sHair = UnityEngine.Random.Range (0, nHair);
		int sPant = UnityEngine.Random.Range (0, nPant);
		int sSkin = UnityEngine.Random.Range (0, nSkin);

		person.Configure(hairs[sHair], pants[sPant], skins[sSkin] );

		instanciaPerson.transform.position = pos;
		return person;

	}

	Person monoValido(Vector3 pos) {
		return apereceMono( QuestionController.instance.validHairs, QuestionController.instance.validPants, QuestionController.instance.validSkins, pos );
	}

	Person monoInvalido(Vector3 pos){

		List<Condition.Hair> selectedHairs;
		List<Condition.Pant> selectedPants;
		List<Condition.Skin> selectedSkins;

		float r = UnityEngine.Random.value;
		if( r < 0.3333f ){
			selectedHairs = QuestionController.instance.invalidHairs;
			selectedPants = QuestionController.instance.allPants;
			selectedSkins = QuestionController.instance.allSkins;
		}
		else if( r < 0.6666f ){
			selectedHairs = QuestionController.instance.allHairs;
			selectedPants = QuestionController.instance.invalidPants;
			selectedSkins = QuestionController.instance.allSkins;
		}
		else{
			selectedHairs = QuestionController.instance.allHairs;
			selectedPants = QuestionController.instance.allPants;
			selectedSkins = QuestionController.instance.invalidSkins;
		}

		return apereceMono( selectedHairs, selectedPants, selectedSkins, pos );
	}


	public void OnAnswer( bool isGoodAnswer ){
		if (isGoodAnswer) {
			remainingGoodAnswers--;
			if (remainingGoodAnswers <= 0) {
				destroyGame();
				level++;
				loadGame();
			}
		} else {
			currentLives--;

			GameObject volcan = GameObject.FindGameObjectWithTag ("volcan");
			Vector3 v = volcan.transform.position;

			volcan.transform.localPosition += new Vector3 (0, lavaHeight / ((float)lives));


			//subir lava
			if (currentLives <= 0) {
				gameFinished = true;
				QuestionController.instance.killIcons ();
				StartCoroutine ( EndCoroutine());
			}
		}
	}


	private void destroyGame(){

		foreach (Person person in persons) {

			if (person != null) {
				Destroy (person.gameObject);
			}
		}
		persons.Clear ();

	}

	IEnumerator EndCoroutine(){
		yield return new WaitForSeconds (0.5f);
		while(true) {
			if( Input.GetMouseButtonDown(0) ){
				UnityEngine.SceneManagement.SceneManager.LoadScene (1);
				yield break;
			}
			yield return 0;
		}
	}

}
