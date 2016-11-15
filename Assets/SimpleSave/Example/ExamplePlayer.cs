using SimpleSave.Core;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ExamplePlayer : MonoBehaviour {

	public string Rank;
	public int Level;
	public float Experience;

	public Text RankText;
	public Text LevelText;
	public Text ExperienceText;

	void Start ()
	{
		Rank = (string)DataUtility.GetProperty("Player","Rank");
		Level = (int)DataUtility.GetProperty("Player","Level");
		Experience = (float)DataUtility.GetProperty("Player","Experience");
		DisplayPlayerData();
	}

	void DisplayPlayerData ()
	{
		RankText.text = Rank;
		LevelText.text = Level.ToString();
		ExperienceText.text = Experience.ToString();
	}

	public void AddExperience ()
	{
		Experience += Random.Range(1,100);
		DisplayPlayerData();
	}

	public void AddLevel ()
	{
		Level += 1;
		CheckRank();
		DisplayPlayerData();
	}

	void CheckRank ()
	{
		if (Level <= 5)
		{
			Rank = (string)DataUtility.GetProperty("Ranks","Rank 1");
		}
		if (Level > 5 && Level <= 10)
		{
			Rank = (string)DataUtility.GetProperty("Ranks","Rank 2");
		}
		if (Level > 10 && Level <= 15)
		{
			Rank = (string)DataUtility.GetProperty("Ranks","Rank 3");
		}
		if (Level > 15 && Level <= 20)
		{
			Rank = (string)DataUtility.GetProperty("Ranks","Rank 4");
		}
		if (Level > 20 && Level <= 25)
		{
			Rank = (string)DataUtility.GetProperty("Ranks","Rank 5");
		}
	}
}
