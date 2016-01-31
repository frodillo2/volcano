using UnityEngine;
using System.Collections;

public class TitleSceneController : MonoBehaviour {

	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			UnityEngine.SceneManagement.SceneManager.LoadScene (1);
		}
	}
}
