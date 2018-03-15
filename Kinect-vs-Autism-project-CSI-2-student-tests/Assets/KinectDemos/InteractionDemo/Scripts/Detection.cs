using UnityEngine;
using System.Collections;

public class Detection : MonoBehaviour
{
    public int HiddenFor;
    public bool Dissapear=false;
    public AudioClip reveal;
    bool played = false;
    // Use this for initialization
    void Start()
    {
        HiddenFor = Random.Range(1, 3);

        gameObject.GetComponent<Renderer>().enabled = false;

    }

    // Update is called once per frame
    void Update()
    {



        if (Dissapear)
        {
            GoAway();
            if (played == false)
            {
                AudioSource.PlayClipAtPoint(reveal, this.transform.position);
                played = true;
            }
        } 
    }


    void OnCollisionEnter(Collision collision)
    {
        if (gameObject.GetComponent<Renderer>().enabled == false)
        {
            
            if (collision.gameObject.GetComponent<Lantern>().player == HiddenFor)
            {
                gameObject.GetComponent<Renderer>().enabled = true;
               // Debug.Log("same");
            }
        }


    }
    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.GetComponent<Lantern>().player != HiddenFor&& gameObject.GetComponent<Renderer>().enabled)
            Dissapear = true;
        
    }
    void OnCollisionExit(Collision collision)
    {
        if (gameObject.GetComponent<Renderer>().enabled)
        {
            if (!Dissapear)
            {
                if (collision.gameObject.GetComponent<Lantern>().player == HiddenFor)
                {
                    gameObject.GetComponent<Renderer>().enabled = false;
                    
                }
            }

        }
    }
    void GoAway()
    {
        //shrinks the object
        transform.localScale -= new Vector3(0.1F, 0.1f, 0);
        //If the object is small enough then destroy it
        if (transform.localScale.x < .1)
            Destroy(gameObject);
    }
}
