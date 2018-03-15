using UnityEngine;
using System.Collections;

public class PlaySound : MonoBehaviour {

	public AudioClip boing;

	private AudioSource boingSource;


	// Use this for initialization
	void Start () {
		boingSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		boingSource.PlayOneShot(boing);
	}
}
