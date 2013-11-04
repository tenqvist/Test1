using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {
	
	public float count;	// Players points. They start from zero.
	public float moshPower;	// Players mosh power. Excellent hits can increase the mosh power gradually to maximum of 2.
	public int fatigue;	// Integer value to show how tired the player is with shown max value of 49 before the player has to rest
	private int fatigueReliefCounter;	// Integer value to indicate when fatigue value can be decreased by one
	private bool headDown;	// Boolean value to tell if the players head is facing down
	private bool moshAllowed;	// Boolean value to tell if the player can mosh
	public bool noteInRange;	// Boolean value to tell if there are notes that can be "moshed"
	public GUIText countText;	// GUI text to show the players points
	public GUIText winText;	// GUI text to show when the game ends or player wins, has multiple purposes
	public GUIText startText;	// GUI text that is shown in the beginning of the game, but has multiple purposes
	public GUIText instructions1Text; // shows instructions in the beginning
	public GUIText song;	// GUI text to show the name of the song that is playing
	public GUIText artist;	// GUI text to show the name of the artist
	public GUIText moshPowerText;	// GUI text that shows the value of the player's mosh power
	public GUIText fatigueText;		// GUI text to show fatigue text and value
	public GUIText moshPowerUpText;	// GUI text to show when player an excellent or perfect hit
	public bool gameStarted;	// Boolean value to tell if the player has started the game
	private bool winTextHelper;		// Boolean value to tell when to change winning text which is used for multiple purposes
	private int startTextHelper;	// Incrementally growing integer value to indicate when to erase starting text
	public bool audioStarted;	// Booleean value to indicate whether the audio is started or not
	private float gameStartTime; // Real time from start of the scene when player starts the game and presses 'any key'
	public bool moshPowerUpTextShow; // Boolean value to indicate whether or not to show "Mosh Power" GUI text
	private int moshPowerUpTextShowCounter; // Counter to indicate how many frames "Mosh Power" stays on screen
	public bool perfection; // Boolean value to indicate perfect timing to a note
				
	void Start () {
		count = 0;
		fatigue = 0;
		moshPower = 1;
		SetCountText();
		headDown = false;
		SetFatigueText();
		gameStartTime = 0;
		SetMoshPowerText();
		perfection = false;
		noteInRange = false;
		moshAllowed = false;
		gameStarted = false;
		startTextHelper = 0;
		audioStarted = false;
		winTextHelper = true;
		fatigueReliefCounter = 0;
		moshPowerUpTextShow = false;
		moshPowerUpTextShowCounter = 0;
		winText.text = "Mosh when a note is in the center of the spotlight!";
		instructions1Text.text = "Press 'Space' to mosh. Good luck!";
		startText.text = "If you are ready to mosh, press ANY KEY to start.";
		song.text = "Now playing: Beneath The Waves";
		artist.text = "by Embreach";
		moshPowerUpText.text = "";
		Time.timeScale = 0;	
	}
	
	void Update ()
	{
		if(Input.anyKey & !gameStarted) {
			gameStarted = true;
			gameStartTime = Time.realtimeSinceStartup;
		}
		if(gameStarted & !audioStarted) {
			InitializeGame();
		}
		
		if (Input.GetKeyDown ("space") & moshAllowed) {
			if (fatigue < 50 & gameStarted) {
				transform.Rotate(0, 45, 0);
				headDown = true;
			}
			if(!noteInRange & fatigue < 55 & moshAllowed) {
				fatigue++;
				fatigueReliefCounter = 0;
				if (moshPower > 1) {
					moshPower = moshPower - 0.1f;
				}
				if (moshPower <= 1 & moshPower > 0.7 & fatigue > 20) {
					moshPower = moshPower - 0.1f;
				}
			}
		}
		if (Input.GetKeyUp ("space") & moshAllowed){
			if (fatigue < 50 & headDown) {
				transform.Rotate(0, -45 ,0);
				headDown = false;
			}
		}
		
		SetCountText();
		SetMoshPowerText();
		SetFatigueText();
		fatigueRelief();
		AllowMosh();
		showMoshPowerUpText();
		
		if(moshAllowed & winTextHelper) {
			winText.text = "";
			instructions1Text.text = "";
			winTextHelper = false;
		}
		if (moshAllowed & startTextHelper < 300) {
			startText.text = "Get ready to mosh!";
			startTextHelper++;			
		} else if (moshAllowed & startTextHelper == 300) {
			startText.text = "";
			startTextHelper++;
		}
	}
	
	void SetCountText () {
		countText.text = "Points: " + count.ToString("f0") + " / 8000";
	}
	
	void SetMoshPowerText () {
		moshPowerText.text = "Mosh Power: " + moshPower.ToString("f2");
	}
	
	void SetFatigueText () {
		if (fatigue < 50) {
			fatigueText.text = "Fatigue: " + fatigue.ToString();	
		} else {
			fatigueText.text = "Fatigue overwhelming! Can't mosh!";	
		}
	}
	
	void InitializeGame() {
		Time.timeScale = 1;
		startText.text = "";
		audio.Play();
		audioStarted = true;
	}
	
	void fatigueRelief() {
		if (fatigue > 0 & fatigueReliefCounter == 100) {
			fatigue--;
			fatigueReliefCounter = 0;
		} else if (fatigueReliefCounter < 100) {
			fatigueReliefCounter++;			
		}
	}
	
	void AllowMosh() {
		if (!moshAllowed & (Time.realtimeSinceStartup - gameStartTime) > 5 & gameStarted) {
			moshAllowed = true;	
		}
	}
	
	void showMoshPowerUpText() {
		if (moshPowerUpTextShow & moshPowerUpTextShowCounter < 100) {
			if (perfection) {
				moshPowerUpText.text = "Perfect Mosh!";
			} else {
				moshPowerUpText.text = "Mosh Power!!";
			}
			moshPowerUpTextShowCounter++;
		}
		if (moshPowerUpTextShowCounter == 100) {
			moshPowerUpText.text = "";
			moshPowerUpTextShowCounter = 0;
			moshPowerUpTextShow = false;
			perfection = false;
		}
		
	}
	
}
