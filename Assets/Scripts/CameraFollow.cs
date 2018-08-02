using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public Controller2D target;

	private void Start() {
		float windowAspect = (float)Screen.width / (float)Screen.height;
		float scaleHeight = windowAspect / (9/16);

		if (scaleHeight < 1.0f) {
			Camera.main.orthographicSize = Camera.main.orthographicSize / scaleHeight;
		}
		//Camera.main.orthographicSize = (Screen.height / 2.0f) / 64;
	}

	void Update() {
		if(target.transform.position.y > 0){
			if(target.transform.position.y > transform.position.y){
				this.transform.position = new Vector3(0, target.transform.position.y, 0);
			}
		}

	}

}
