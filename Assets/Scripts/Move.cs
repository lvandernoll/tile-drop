using System.Collections;
using UnityEngine;

public class Move : MonoBehaviour {

	public float moveAmount = 3f;
	public float moveTime;
	public float gravity;
	public float jumpForce;
	private float verticalVelocity;
	private Vector3 moveVector = Vector3.zero;
	private CharacterController controller;
	
	void Start() {
		controller = gameObject.GetComponent<CharacterController>();
	}

	void Update() {
		//Calculations for jumpheight
		moveTime = GameMaster.tickTime - 0.125f;
		jumpForce = 7.5f / moveTime;
		gravity = (15f / moveTime) / moveTime;

		//Moving + Falling
		if( controller.isGrounded ) {
			if( Input.GetButtonDown("Forward") ) {
				verticalVelocity = jumpForce;
				Vector3 offset = new Vector3(0, 0, moveAmount);
				Vector3 rotationOffset = new Vector3(90f, 0, 0);
				StartCoroutine(moveOverSeconds(offset, rotationOffset, moveTime));
			} else if( Input.GetButtonDown("Backward") ) {
				verticalVelocity = jumpForce;
				Vector3 offset = new Vector3(0, 0, -moveAmount);
				Vector3 rotationOffset = new Vector3(-90f, 0, 0);
				StartCoroutine(moveOverSeconds(offset, rotationOffset, moveTime));
			} else if( Input.GetButtonDown("Left") ) {
				verticalVelocity = jumpForce;
				Vector3 offset = new Vector3(-moveAmount, 0, 0);
				Vector3 rotationOffset = new Vector3(0, 0, 90f);
				StartCoroutine(moveOverSeconds(offset, rotationOffset, moveTime));
			} else if( Input.GetButtonDown("Right") ) {
				verticalVelocity = jumpForce;
				Vector3 offset = new Vector3(moveAmount, 0, 0);
				Vector3 rotationOffset = new Vector3(0, 0, -90f);
				StartCoroutine(moveOverSeconds(offset, rotationOffset, moveTime));
			}
		} else {
			verticalVelocity -= gravity * Time.deltaTime;
		}

		moveVector.y = verticalVelocity;
		controller.Move(moveVector * Time.deltaTime);
	}

	//Moving a fixed distance in a fixed amount of seconds
	IEnumerator moveOverSeconds(Vector3 offset, Vector3 rotationOffset, float seconds) {
		Vector3 startingPos = gameObject.transform.position;
		Vector3 end = gameObject.transform.position + offset;
		Quaternion rotationNone = Quaternion.Euler(Vector3.zero);
		Quaternion rotationEnd = Quaternion.Euler(rotationNone.eulerAngles + rotationOffset);

		float elapsedTime = 0;
		while( elapsedTime < seconds ) {
			//Transform position
			startingPos.y = gameObject.transform.position.y;
			end.y = gameObject.transform.position.y;
			gameObject.transform.position = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));

			//Transform rotation
			gameObject.transform.rotation = Quaternion.Lerp(rotationNone, rotationEnd, (elapsedTime / seconds));

			//Timing control
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		gameObject.transform.position = end;
		gameObject.transform.rotation = rotationNone;
	}
}
