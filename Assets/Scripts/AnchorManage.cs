using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorManage : MonoBehaviour {

	GameObject gameOver;

	void Start () {
		gameOver = GameObject.FindGameObjectWithTag("GameOverFloor");
	}
	
	void Update () {
		if(this.gameObject.transform.position.y <= gameOver.transform.position.y){
			Destroy(this.gameObject);
		}
	}
}
