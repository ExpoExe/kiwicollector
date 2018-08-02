using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundGenerate : MonoBehaviour {

	private GameObject[,] tileArr;
	private string[] tiles = { "Tile Plain", "Tile Plain", "Tile Plain", "Tile Plain", "Tile Plain", "Tile Plain", "Tile Plain", "Tile Rock", "Tile Vine", "Tile Combo", };
	GameObject gameOver;

	void Awake() {

		gameOver = GameObject.FindGameObjectWithTag("GameOverFloor");

		double worldScreenHeight = 5 + Camera.main.orthographicSize * 2.0;
		double worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;
		worldScreenWidth = Mathf.Ceil((float)worldScreenWidth);

		tileArr = new GameObject[(int)worldScreenHeight, (int)worldScreenWidth];

		for (int y = 0; y <= worldScreenHeight-1; y++){

			for (int x = 0; x <= worldScreenWidth-4; x++){

				int i = Random.Range(0, tiles.Length);
				GameObject go = Instantiate(GameObject.Find(tiles[i]), new Vector3(x - 3.75f, y - 9, 0), Quaternion.identity) as GameObject;
				go.transform.SetParent(GameObject.Find("Background").transform, false);
				tileArr[y,x] = go;
			}

		}

	}

	private void Update() {

		for (int y = 0; y <= tileArr.GetLength(0)-1; y++){
			float tileYPos = tileArr[y, 1].transform.position.y;
			if (tileYPos < gameOver.transform.position.y + 1.3) {

				for (int x = 0; x < 9; x++) {
					//Debug.Log(tileArr[y, x].transform.position);
					tileArr[y, x].transform.position = new Vector3(x - 3.75f, tileYPos + 20, 0);
				}

			}

		}

	}

}
