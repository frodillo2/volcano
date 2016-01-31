using UnityEngine;
using System.Collections;

public class TransitionController : MonoBehaviour {

	// Use this for initialization
	IEnumerator Start () {
		yield return new WaitForSeconds (0.5f);
		UnityEngine.SceneManagement.SceneManager.LoadScene (0);
	}

}
