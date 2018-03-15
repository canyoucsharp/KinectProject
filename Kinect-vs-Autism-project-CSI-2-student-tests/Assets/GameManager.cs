using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour {
	private static GameManager instance;
	private int currentScene;
	private int totalScore;


	void Awake()
	{
		
		instance=null;
		if(instance==null){
			instance=this;
			currentScene=0;
			totalScore=0;
		}

		DontDestroyOnLoad(gameObject);



	}

	public static GameManager getInstance(){
		if(instance==null)
		{
			instance=new GameManager();
		}
		return instance;
	}
		
	public int getTotalScore()
	{
		return totalScore;
	}

	public void increaseScore(int points){
		totalScore +=points;
	}

	public void setCurrentSceneIndex(int index)
	{
		GameManager.getInstance().currentScene=index;
	}

	public int getCurrentSceneIndex()
	{
		return GameManager.getInstance().currentScene;
	}
}
