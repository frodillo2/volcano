using UnityEngine;
using System.Collections;

public class TestController : MonoBehaviour {

	public GameObject monoPrefab;

	void Start(){
		monoPrefab = Resources.Load ("baila_0") as GameObject;
	}

	void OnMouseDown() {
		Debug.Log ("hola pincho");
		GameObject instancioMono = Instantiate<GameObject>(monoPrefab);
		instancioMono.transform.position = this.transform.position;
		this.transform.position += new Vector3 (0, 1f);
	}
}
