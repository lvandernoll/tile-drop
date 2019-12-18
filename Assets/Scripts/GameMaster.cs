using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {

	public bool hasStarted = false;
	public static float tickTime = .75f;
	public Transform tile;
	private Vector3 tileDropLocation = new Vector3(0, 5, 0);
	public float gravity = 10f;
	private int lastSide;
	private ArrayList sides = new ArrayList();
	private bool isFirstTile = true;
	private int random;
	private int randomAmount = 3;
	private AudioSource beatAudio;
	public Canvas startScreen;

	void Start() {
		Time.timeScale = 0;
		sides.Add(0);
		sides.Add(1);
		sides.Add(2);
		StartCoroutine(dropTiles());
		beatAudio = GetComponent<AudioSource>();
		StartCoroutine(playAudio());
		Physics.gravity = new Vector3(0, -1.0F, 0);
	}

	void Update() {
		if( Input.anyKey ) {
			startScreen.enabled = false;
			hasStarted = true;
			Time.timeScale = 1;
		}
	}

	IEnumerator playAudio() {
		while( true ) {
			if( hasStarted ) {
				beatAudio.Play();
			}
			yield return new WaitForSeconds(tickTime);
		}
	}

	IEnumerator dropTiles() {
		while( true ) {
			if( hasStarted ) {
				random = Random.Range(0, randomAmount);

				if( isFirstTile ) {
					random = 1;
					isFirstTile = false;
				}
		
				switch( (int)sides[random] ) {
					case 0://Right
						tileDropLocation.x += 3f;
						lastSide = 0;
						break;

					case 1://Forward
						tileDropLocation.z += 3f;
						lastSide = 1;
						break;

					case 2://Left
						tileDropLocation.x -= 3f;
						lastSide = 2;
						break;
				}
				sides.Clear();
				sides.Add(0);
				sides.Add(1);
				sides.Add(2);
				switch( lastSide ) {
					case 0://If last moved side is left, cant go right again
						randomAmount = 2;
						sides.Remove(2);
						break;
					case 1://If last moved side is forward, can go anywhere
						randomAmount = 3;
						break;
					case 2://If last moved side is right, cant go right left
						randomAmount = 2;
						sides.Remove(0);
						break;
				}

				Instantiate(tile, tileDropLocation, Quaternion.Euler(Vector3.zero));
				destroyTile(tile, tickTime * 2);
			}
			yield return new WaitForSeconds(tickTime);
		}
	}

	//Destroy tile over fixed time
	IEnumerator destroyTile(Transform tile, float seconds) {
		float elapsedTime = 0;
		while( elapsedTime < seconds ) {
			//Opacity


			//Timing control
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		Destroy(tile);
	}
}
