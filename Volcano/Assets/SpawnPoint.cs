using UnityEngine;
using System.Collections;

public class SpawnPoint : MonoBehaviour {

	void OnDrawGizmos(){
		Gizmos.DrawSphere ( transform.position, 0.2f );
	}
}
