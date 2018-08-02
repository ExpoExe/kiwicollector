using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent (typeof(Controller2D))]

public class Player : MonoBehaviour {

	public float jumpHeight = 7;
	public float timeToJumpApex = .6f;
	public float accelerationTimeAirborne = .15f;
	public float accelerationTimeGrounded = .1f;
	float moveSpeed = 5;

	float gravity;
	float jumpVelocity;
    Vector2 velocity;
	float velocityXSmoothing;

    Controller2D controller;

	// Use this for initialization
	void Start () {

        controller = GetComponent<Controller2D>();
		gravity = (2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
		jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;


	}

	// Update is called once per frame
	void Update () {

		if (controller.collisionInfo.above || controller.collisionInfo.below) {
			velocity.y = 0;
		} else{

		}

		Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		if(Input.GetKeyDown(KeyCode.Space) && controller.collisionInfo.below){
			velocity.y = jumpVelocity;
		}

		float targetVelocityX = input.x * moveSpeed;
		float velocityXSmoothTime = (controller.collisionInfo.below ? accelerationTimeGrounded : accelerationTimeAirborne);
		velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, velocityXSmoothTime);
		velocity.y -= (gravity * Time.deltaTime);
		controller.Move(velocity * Time.deltaTime);
	}
}
