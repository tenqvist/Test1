using UnityEngine;
using System.Collections;

public class NoteControl : MonoBehaviour {

	public int speed;
	private int hit;
	private float xpos;
	private bool passedPlayer;
	private PlayerControl player;
	private bool moshPowerAllowed;
	private bool perfectHit;
		
	void Start () {
		hit = 0;
		passedPlayer = false;
		moshPowerAllowed = false;
		perfectHit = false;
		player = (PlayerControl) FindObjectOfType(typeof(PlayerControl));	
	}
	
	void Update () {
		xpos = transform.position.x;
		informPlayer();
		checkNote();	
	}
	
	void FixedUpdate () {
		transform.Translate(-Time.deltaTime * speed, 0, 0, Space.World);
	}
	
	void checkNote () {
		if (Input.GetKeyDown ("space")) {
			if (xpos < 0 & xpos > -1 & hit == 0) {
				hit = 1;
				if (xpos < -0.6 & xpos > -1) {
					moshPowerAllowed = true;
				}
				if (xpos < -0.8 & xpos > -1) {
					perfectHit = true;
				}
				awardPoints();
			}
		}
	}
	
	void awardPoints() {
		if (player.fatigue < 50) {
			player.count = player.count + (Mathf.Pow(xpos, 2) * 500 * player.moshPower);
			if (moshPowerAllowed & player.moshPower < 2) {
				player.moshPower = player.moshPower + 0.1f;
				player.moshPowerUpTextShow = true;
			}
			if (perfectHit) {
				player.perfection = true;
			}
		}
	}
	
	void informPlayer() {
		if (xpos < 0 & xpos > -1) {
			player.noteInRange = true;
		}
		if (xpos < -1 & !passedPlayer) {
			player.noteInRange = false;
			passedPlayer = true;			
		}
	}
}
