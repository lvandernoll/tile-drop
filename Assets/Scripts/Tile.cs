using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

	public float floorPos = -0.75f;
	public float destroyDelay = GameMaster.tickTime * 4;
	private Renderer mesh;
	public Color startingColor;
	public Color endColor;

	void Start () {
		mesh = GetComponent<Renderer>();
		startingColor = mesh.material.color;
		endColor = startingColor;
		endColor.a = 0;
		Vector3 endPos = new Vector3(0, floorPos, 0);
		StartCoroutine(moveDown(endPos, GameMaster.tickTime));
	}
	
	IEnumerator moveDown(Vector3 endPos, float seconds) {
		Vector3 startingPos = gameObject.transform.position;

		float elapsedTime = 0;
		while( elapsedTime < seconds ) {
			//Moving down
			startingPos.x = gameObject.transform.position.x;
			startingPos.z = gameObject.transform.position.z;
			endPos.x = gameObject.transform.position.x;
			endPos.z = gameObject.transform.position.z;
			gameObject.transform.position = Vector3.Lerp(startingPos, endPos, (elapsedTime / seconds));

			//Timing control
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		gameObject.transform.position = endPos;
		StartCoroutine(destroyTile(seconds));
	}

	IEnumerator destroyTile(float seconds) {

		float elapsedTime = 0;
		while( elapsedTime < destroyDelay ) {
			//Opacity
			mesh.material.color = Color.Lerp(startingColor, endColor, (elapsedTime / destroyDelay));

			//Time control
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		endColor = new Color(255, 255, 255, 0);
		Destroy(gameObject);
	}
}
