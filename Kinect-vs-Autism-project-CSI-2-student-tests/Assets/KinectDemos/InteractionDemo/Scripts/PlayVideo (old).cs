using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent (typeof(AudioSource))]

public class PlayVideo1 : MonoBehaviour {

    public MovieTexture movie;
    private AudioSource audio;
    public GameObject video;
    public GameObject button;

	public GameObject canv;
	public GameObject scenePeices;


    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space) && movie.isPlaying)
        {
            movie.Pause();
        }

        else if (Input.GetKeyDown(KeyCode.Space) && !movie.isPlaying)
        {
            movie.Play();
        }


        if (!movie.isPlaying)
        {
            Disable_Video();
            Enable_Objects();

            //OR
            
            //Disable_Button(); since you
            //want player to play game again

            //Then here would be some code
            //that would return to playing
            //the game
        }

    }

    //To hide the video
    public void Disable_Video()
    {
        video.SetActive(false);
    }

    //To reveal the video
    public void Enable_Video()
    {
        video.SetActive(true);
    }

    //To hide the UI button
    public void Disable_Objects()
    {
        button.SetActive(false);
		//canv.SetActive(false);
		scenePeices.SetActive(false);
    }

    //To reveal the UI button
    public void Enable_Objects()
    {
        button.SetActive(true);
		canv.SetActive(true);
		scenePeices.SetActive(true);
    }

    //When clicked on the UI button do this:
    public void Button_Click()
    {
        //Set up the video
        GetComponent<RawImage>().texture = movie as MovieTexture;
        audio = GetComponent<AudioSource>();
        audio.clip = movie.audioClip;

        //To play the video
        movie.Play();
        audio.Play();
    }
}
