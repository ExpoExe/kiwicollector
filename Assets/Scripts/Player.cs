using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent (typeof(Controller2D))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]


public class Player : MonoBehaviour {

	public float jumpHeight = 5;
	public float timeToJumpApex = .4f;
	public float accelerationTimeAirborne = 4f;
	public float accelerationTimeGrounded = .1f;
	public float launchMultiplier = 40;
	float moveSpeed = 6;

	int score = 0;
	int highscore;
	public Text scoreText;
	public Text highScoreText;

	float gravity;
	float jumpVelocity;
    public Vector2 velocity;
	float targetVelocityX;
	float velocityXSmoothing;
	bool flipSprite = false;

	//hook stuff
	public int hookMaxDistance = 8;
	public bool isLaunchingTowards = false;
	Vector2 hookHitLocation;
	public LayerMask hookCollisionMask;
	public LineRenderer lineRenderer;

	//animation states
	const int STATE_IDLE = 0;
	const int STATE_THROW = 1;
	const int STATE_LAUNCH = 2;
	const int STATE_FLUTTER = 3;

	int _currentAnimationState = STATE_IDLE;

	private SpriteRenderer sr;
	private Animator animator;

	Controller2D controller;
	public GameObject spawner;

	void Start () {

        controller = GetComponent<Controller2D>();
		gravity = (2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
		jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;

		lineRenderer = GetComponent<LineRenderer> ();
		lineRenderer.enabled = false;
		lineRenderer.SetVertexCount(2);

		sr = GetComponent<SpriteRenderer>();
		animator = GetComponent<Animator>();

		spawner = GameObject.FindGameObjectWithTag("Spawner");
		scoreText = GameObject.FindGameObjectWithTag("Score").GetComponent<Text>();
		highScoreText = GameObject.FindGameObjectWithTag("Highscore").GetComponent<Text>();

		if (PlayerPrefs.GetInt("highscore") >= 0){
			highscore = PlayerPrefs.GetInt("highscore");
		} else {
			highscore = 0;
		}

	}

	void Update () {
		//game over check
		if (controller.gameOver) {
			SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
		}

		// collision checks
		if (controller.collisionInfo.above || controller.collisionInfo.below) {
			velocity.y = 0;
		}
		if (controller.collisionInfo.left || controller.collisionInfo.right) {
			velocity.x = velocity.x * -1;
			velocity.x = (velocity.x < 0) ? velocity.x * 0.75f : velocity.x * 0.75f;
		}

		//sprite management
		flipSprite = (sr.flipX ? (velocity.x > 0.01f) : (velocity.x < -0.01f));
		if (flipSprite){
			sr.flipX = !sr.flipX;
		}

		//user inputs
		Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		if(Input.GetKeyDown(KeyCode.Space) && controller.collisionInfo.below){
			velocity.y = jumpVelocity;
		}
		for (var i = 0; i < Input.touchCount; ++i) {
			if (Input.GetTouch(i).phase == TouchPhase.Began) {
				RaycastHit2D hit = controller.TouchHit(hookMaxDistance, hookCollisionMask);
				if (hit) {
					changeState(STATE_THROW);
					hookHitLocation = hit.point;
					isLaunchingTowards = true;
					lineRenderer.enabled = true;
					lineRenderer.SetPosition(1, hookHitLocation);
				}
			}
		}

		if (Input.GetMouseButtonDown(0)){
			RaycastHit2D hit = controller.MouseClickHit(hookMaxDistance, hookCollisionMask);
			if (hit) {
				changeState(STATE_THROW);
				hookHitLocation = hit.point;
				isLaunchingTowards = true;
				lineRenderer.enabled = true;
				lineRenderer.SetPosition (1, hookHitLocation);
			}
				
		}

		//velocity modifications
		if (isLaunchingTowards) {
			LaunchTowards();
		} else if (velocity.y < 0 && transform.position.y > 5) {
			velocity.y *= .8f;
			changeState(STATE_FLUTTER);
		} else {
			targetVelocityX = input.x * moveSpeed;
			float velocityXSmoothTime = (controller.collisionInfo.below ? accelerationTimeGrounded : accelerationTimeAirborne);
			velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, velocityXSmoothTime);
		}
		velocity.y -= (gravity * Time.deltaTime);

		controller.Move(velocity * Time.deltaTime);

		//score tracking
		if (velocity.y >= 0){
			score = (int)this.transform.position.y;
		}
		scoreText.text = "Score: " + score;
		if (score > highscore){
			highscore = score;
			PlayerPrefs.SetInt("highscore", highscore);
		}
		highscore = PlayerPrefs.GetInt("highscore");
		highScoreText.text = "Highscore: " + highscore;

	}

	public void LaunchTowards() {

		if (Vector2.Distance(transform.position, hookHitLocation) > .5f) {

			velocity = hookHitLocation - (Vector2)transform.position;
			velocity.Normalize ();
			velocity += velocity * launchMultiplier;
			lineRenderer.SetPosition(0, transform.position);
		} else {
			isLaunchingTowards = false;
			lineRenderer.enabled = false;
			changeState(STATE_LAUNCH);
			spawner.SendMessage("Spawn");
		}
	}

	void changeState(int state) {

		if (_currentAnimationState == state)
			return;

		switch (state) {

			case STATE_IDLE:
				animator.SetInteger("state", STATE_IDLE);
				break;

			case STATE_THROW:
				animator.SetInteger("state", STATE_THROW);
				break;

			case STATE_LAUNCH:
				animator.SetInteger("state", STATE_LAUNCH);
				break;

			case STATE_FLUTTER:
				animator.SetInteger("state", STATE_FLUTTER);
				break;

		}

		_currentAnimationState = state;
	}

}
