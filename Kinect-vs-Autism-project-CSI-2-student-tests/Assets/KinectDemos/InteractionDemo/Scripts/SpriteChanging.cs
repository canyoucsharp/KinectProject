using UnityEngine;
using System.Collections;

public class SpriteChanging : MonoBehaviour {

    public SpriteRenderer sr;
    public Sprite[] textures;
    // Use this for initialization
    void Start () {

        sr = GetComponent<SpriteRenderer>();
		//Loads all from the Resources folder - images folder
        textures = Resources.LoadAll<Sprite>("images");


        Debug.Log(textures.Length);
        int choice = Random.Range(0, textures.Length);

        Debug.Log(choice.ToString());
        sr = GetComponent<SpriteRenderer>();
        gameObject.GetComponent<SpriteRenderer>().sprite = textures[choice] as Sprite;
    }

	public void changeBackground() {
		int choice = Random.Range (0, textures.Length);
		gameObject.GetComponent<SpriteRenderer>().sprite = textures[choice] as Sprite;
	}
	

}
