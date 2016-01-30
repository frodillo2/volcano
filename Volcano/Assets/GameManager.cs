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

	void Start(){
		instance = this;
		personPrefab = Resources.Load ("PersonPrefab") as GameObject;
		loadGame();
	}
		
	void loadGame(){

		QuestionController.instance.defineConditions ();

		//StartCoroutine (AparecerMonoCoroutine());

		int n = UnityEngine.Random.Range (1, 10);
		print ("valido: " + n);
		AparecerMonoCoroutine(n,10-n);
	}

	void AparecerMonoCoroutine(int valido, int invalido){
		
		remainingGoodAnswers = 0;
		for(int i = 0; i <valido; i++){
			Person newPerson = monoValido();	

			persons.Add (newPerson);
			if (QuestionController.instance.IsValid (newPerson)) {
				remainingGoodAnswers++;
			}
			//yield return new WaitForSeconds (0.8f);

		}

		for(int i = 0; i <invalido; i++){
			Person newPerson = monoInvalido();	

			persons.Add (newPerson);
			if (QuestionController.instance.IsValid (newPerson)) {
				remainingGoodAnswers++;
			}

		}

	}
	/*
	 * Va imprimieno monos en la pantalla
	 */
	Person apereceMono( List<Condition.Hair> hairs, List<Condition.Pant>  pants, List<Condition.Skin> skins) {

		GameObject instanciaPerson = Instantiate<GameObject>(personPrefab);
		Person person = instanciaPerson.GetComponent<Person> ();

		int nHair = hairs.Count;
		int nPant = pants.Count;
		int nSkin = skins.Count;

		int sHair = UnityEngine.Random.Range (0, nHair);
		int sPant = UnityEngine.Random.Range (0, nPant);
		int sSkin = UnityEngine.Random.Range (0, nSkin);

		person.Configure(hairs[sHair], pants[sPant], skins[sSkin] );

		float x = UnityEngine.Random.Range (-5f, 5f);
		float y = UnityEngine.Random.Range(-8f,-3f);
		instanciaPerson.transform.position = new Vector3 (x, y);
		return person;

	}

	Person monoValido() {
		return apereceMono( QuestionController.instance.validHairs, QuestionController.instance.validPants, QuestionController.instance.validSkins );
	}

	Person monoInvalido(){

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

		return apereceMono( selectedHairs, selectedPants, selectedSkins );
	}


	public void OnAnswer( bool isGoodAnswer ){
		if (isGoodAnswer) {
			remainingGoodAnswers--;
			if (remainingGoodAnswers <= 0) {
				destroyGame();
				loadGame();
			}
		} else {
			//erorr
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

}
