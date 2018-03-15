using UnityEngine;
using System.Collections;

public class CoverBlocks : MonoBehaviour {
    public bool shrinking = false;
	public int playerDestroyed;

	public Color color;
    void Start () {

		//Randomizes the number
		playerDestroyed = (int)(float)(Random.value*10)/5;
		//new Color(8,.5f,.5f, .5f);

		//If it belongs to player 1 then color it Green/ RED
        if(playerDestroyed==0)
			GetComponent<Renderer>().material.color = new Color(5,0,0, .5f);
		
		//Else it belongs to player 2 then color it Blue new Color(1, .3f, 1, .5f);
		else
			GetComponent<Renderer>().material.color = new Color(0, 0, 1, .5f);


    }

   
    void Update () {
		//Shrinking is set true when collided with the eraser
        if (shrinking)
        {
			//shrinks the object
            transform.localScale -= new Vector3(0.1F, 0.1f, 0);
			//If the object is small enough then destroy it
            if (transform.localScale.x < .1)
                Destroy(gameObject);
        }

    }
}
