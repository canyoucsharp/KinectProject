using UnityEngine;
using System.Collections;

public class wire : MonoBehaviour {
	void OnDrawGizmosSelected() {
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireCube(transform.position, new Vector3(1, 1, 1));
	}
}