﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterMovement : MonoBehaviour {

	// the horizontal velocity
	float hVelocity;

	// the vertical velocity
	float vVelocity;

	// number of jumps
	public int jumps = 0;

	// horizontal speed multiplier
	public float hSpeed = .05f;

	// vertical jump value
	public float jumpVal = 1.0f;

	// rigibody for character
	Rigidbody2D charRB;

	// character relation
	public bool lookingRight = true;
	// true / false on ground
	public bool onGround;

	public GameObject projectilePrefab;
	GameObject projectileInstance;
	public float posOffset;

	// Use this for initialization
	void Start () {
		charRB = gameObject.GetComponent<Rigidbody2D> ();
		lookingRight = true;
	}

	// Update is called once per frame
	void Update () {
		// calls player hor input function
		getHorizontal ();
		getVertical ();
		getShoot ();
		// calls function that moves character
		move ();
	}

	void getShoot(){
		if(Input.GetKeyDown(KeyCode.F)){
			if (lookingRight) {
				Vector3 projPosition = new Vector3(gameObject.transform.position.x + posOffset, gameObject.transform.position.y, gameObject.transform.position.z);
				projectileInstance = Instantiate (projectilePrefab, projPosition, Quaternion.identity) as GameObject;
			} else {
				Vector3 projPosition = new Vector3(gameObject.transform.position.x - posOffset, gameObject.transform.position.y, gameObject.transform.position.z);
				projectileInstance = Instantiate (projectilePrefab, projPosition, Quaternion.Euler(Vector3.down * 180f))  as GameObject;
			}
		}
	}

	void getHorizontal(){
		hVelocity = Input.GetAxis ("Horizontal") * hSpeed * Time.deltaTime;
		print (hVelocity);
	}

	void getVertical(){
		if (Input.GetKeyDown (KeyCode.Space)) {
			// triple jump
			if (jumps == 2) {
				vVelocity = jumpVal / 2f;
				jumps++;
			}
			// for double jump
			if (jumps == 1) {
				vVelocity = jumpVal;
				jumps++;
			}
			// for regular jump
			if (onGround) {
				vVelocity = jumpVal;
				jumps++;
			}
		} else {
			vVelocity = 0;
		}
	}

	void move(){
		if (hVelocity > 0 && !lookingRight) {
			flip ();
		} else if (hVelocity < 0 && lookingRight) {
			flip ();
		}

		// changes horizontal position
		charRB.transform.position = new Vector2 (charRB.transform.position.x + hVelocity, charRB.transform.position.y);

		// changes vertical velocity
		charRB.velocity += (Vector2.up * vVelocity);
	}

	void flip(){
		lookingRight = !lookingRight;
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
	}

	void OnTriggerEnter2D(Collider2D coll){
		if (coll.CompareTag ("ground")) {
			onGround = true;
			jumps = 0;
		}
	}

	void OnTriggerExit2D(Collider2D coll){
		if (coll.CompareTag ("ground")) {
			onGround = false;
		}
	}
}