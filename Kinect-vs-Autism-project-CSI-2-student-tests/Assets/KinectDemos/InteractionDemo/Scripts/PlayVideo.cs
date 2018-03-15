using UnityEngine;
using System.Collections;
using UnityEngine.UI;
[RequireComponent (typeof(AudioSource))]    //If current object does not have a AudioSource, it creates one

public class PlayVideo : MonoBehaviour {

    //Programmer: Gabriel Goldstein, Ivan Cuenca

    //This Script is attended to be attached to Misc -> MainCamera
    //VARIABLES NEED TO BE ASSIGNED IN EDITOR:  GameObject video
    //                                          GameObject button

    // SUMMARY: Handles the playing of a reward video when selected in the Results page after completing a level
    // FUNCTION: 	- Assigns all videos in file to array movies
    //              - Plays the video and audio of the movie
    //              - Pressing Space stops the video

    //NOTE FOR COMMENTS:    
    //      Movie refers to combination of video and audio
    //
    //RECENT CHANGES:
    //  Switched attached object from RawImage to MainCamera
    //  Reduced un needed code
    //  Took out single line functions

    /* THINGS TO ADD: */


    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    private AudioSource audio;      //Audio of the Movie
    public GameObject video;        //The RawImage that displays the Movie (Assigned in the Editor)
    public GameObject button;       //The Reward Button from the Results Panel (Assigned in the Editor)
    private MovieTexture[] movies;  //Array of all Movies in the Folder (Resources -> Videos)
    
    int randNum; //Random Number used to randomly select a Movie
    
    void Start () {
		movies = Resources.LoadAll<MovieTexture>("Videos"); //Stores all Movies in the Folder (Resources -> Videos) in to the Array Movies
        audio = video.GetComponent<AudioSource>(); //Gets videos AudioSource
    }
	
	void Update () {
        //Hitting Space will stop the video
        if (Input.GetKeyDown(KeyCode.Space)) {
			movies[randNum].Stop(); //Stops the Movie
            video.SetActive(false); //Deactivates the RawImage
            button.SetActive(true); //Activates the Button
        }
        
        //If the video ends, then hide the video and show the play button
        if (randNum<movies.Length && !movies[randNum].isPlaying){
            video.SetActive(false);
            button.SetActive(true);
        }
    }

    public void playVideo() {
        Button_Click();
        video.SetActive(true);
        button.SetActive(false);
    }

    
    public void Button_Click() {
        randNum = Random.Range(0, movies.Length); //Creates a Random Number to Randomly select a Movie from the Movies array
                
        video.GetComponent<RawImage>().texture = movies[randNum] as MovieTexture; //Sets the video for the RawImage
        audio.clip = movies[randNum].audioClip; //Sets the audioclip for the Movie to the AudioSource

        //Plays Video and Audio
        movies[randNum].Play();
        audio.Play();
    }


}
