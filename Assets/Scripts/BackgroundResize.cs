using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundResize : MonoBehaviour {

	void Awake()
	{
		SpriteRenderer sr = GetComponent<SpriteRenderer>();
		if (sr == null)
			return;

		// Set filterMode
		sr.sprite.texture.filterMode = FilterMode.Point;

		// Get stuff
		double width = sr.sprite.bounds.size.x;
		double worldScreenHeight = Camera.main.orthographicSize * 2.0;
		double worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

		// Resize
		transform.localScale = new Vector2(1, 1) * (float)(worldScreenWidth / width);
	}
}
