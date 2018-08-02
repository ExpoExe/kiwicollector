using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

	public GameObject[] itemsToSpawn;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	void Spawn(){
		Instantiate(
			itemsToSpawn[Random.Range(0, itemsToSpawn.Length)],
			new Vector3(Random.Range(-3f, 3f), this.transform.position.y,5),
			Quaternion.identity
			);
	}

}
