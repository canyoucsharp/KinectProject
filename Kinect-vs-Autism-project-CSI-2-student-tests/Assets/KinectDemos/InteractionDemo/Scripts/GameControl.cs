using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{
    public GameObject overLay;
    public GameObject picture;
    public GameObject eraser;
	private GameManager gameManager;

	int level;
    // Use this for initialization
    void Start()
    {
		gameManager=GameManager.getInstance();
		//Initialize the overlay(The box to copy)
        if (GameObject.Find("Overlay"))
            overLay = GameObject.Find("Overlay");
        else Debug.Log("no OverLay");
		//
        if (GameObject.Find("Cube"))
            picture = GameObject.Find("Cube");
        else Debug.Log("no cube");
		//Initialize the picture to the Eraser which is the parent of P1&P2 Erasers
        if (GameObject.Find("Eraser"))
            picture = GameObject.Find("Eraser");
        else Debug.Log("no eraser");

		//Sets the locations of the clone overlay objects
		//Vertical panels
        for (int i = 0; i < 5; i++)
        {
			//Horizontal panels
            for (int j = 0; j < 6; j++)
            {
                Instantiate(overLay, new Vector3(overLay.transform.position.x + j*2, 
				                                 overLay.transform.position.y + i*2, 
				                                 overLay.transform.position.z), 
				            Quaternion.identity);
            }
        }
        Destroy(overLay);
    }


// Update is called once per frame
void Update()
{
    if (!GameObject.Find("Overlay(Clone)")){
        Debug.Log("You Win!!!");
		//Play Animations of sparkle and victory
		//move to next level or back to results page of the level it was from
		SceneManager.LoadScene(gameManager.getCurrentSceneIndex());//changed 02/03/18
			//loads the next level set by peccard
		
			/*
			if (PlayerPrefs.GetInt("Level") == 0)
			Application.LoadLevel(0);
		else
			{
			level = PlayerPrefs.GetInt("Level") + 1;
			Application.LoadLevel(level);
			}
*/
		
		}

}







}