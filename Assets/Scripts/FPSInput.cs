using UnityEngine;
using System.Collections;

// basic WASD-style movement control
// commented out line demonstrates that transform.Translate instead of charController.Move doesn't have collision detection

[RequireComponent(typeof(CharacterController))]
[AddComponentMenu("Control Script/FPS Input")]
public class FPSInput : MonoBehaviour {
	public float speed = 6.0f;
	public float gravity = -9.8f;

	private CharacterController _charController;
	private Rigidbody _rigidBody;
	private float myMass = 1;

	void Start() {
		_charController = GetComponent<CharacterController>();
		_rigidBody = GetComponent<Rigidbody> ();
		myMass = _rigidBody.mass;
	}
	
	void Update() {
		//transform.Translate(Input.GetAxis("Horizontal") * speed * Time.deltaTime, 0, Input.GetAxis("Vertical") * speed * Time.deltaTime);
		float deltaX = Input.GetAxis("Horizontal") * speed;
		float deltaZ = Input.GetAxis("Vertical") * speed;
		Vector3 movement = new Vector3(deltaX, 0, deltaZ);
		movement = Vector3.ClampMagnitude(movement, speed);

		movement.y = gravity;

		movement *= Time.deltaTime;
		movement = transform.TransformDirection(movement);
		_charController.Move(movement);
	}

	void OnControllerColliderHit(ControllerColliderHit hit) {

		Rigidbody body = hit.collider.attachedRigidbody;
		float pushPower = 0;

		if (body == null || body.isKinematic)
			return;

		if (hit.moveDirection.y < -0.3F)
			return;

		if (body.mass < myMass) {
			pushPower = myMass / body.mass;
		} else {
			return;
		}

		Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
		body.velocity = Vector3.ClampMagnitude(pushDir * pushPower, speed);
	}
}
