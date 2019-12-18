using UnityEngine;

public class CameraControl : MonoBehaviour {

	public Transform target;
	public Vector3 offset = new Vector3(0, 2, 4);

	void Update() {
		Vector3 desiredPosition = target.position + offset;
		desiredPosition.y = transform.position.y;
		transform.position = desiredPosition;
	}
}
