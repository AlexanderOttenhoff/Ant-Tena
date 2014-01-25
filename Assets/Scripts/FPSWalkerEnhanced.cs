using UnityEngine;
using System.Collections;
using XInputDotNetPure;

[RequireComponent(typeof(CharacterController))]
public class FPSWalkerEnhanced : MonoBehaviour {

	public float speed = 250f;
	public float lookSpeed = 100f;

	private Vector3 moveDirection = Vector3.zero;
	private CharacterController controller;

	bool playerIndexSet = false;
	PlayerIndex playerIndex;
	GamePadState state;
	GamePadState prevState;

	void Start() {
		controller = GetComponent<CharacterController>();
	}

	void Update() {
		if (!playerIndexSet || !prevState.IsConnected) {
			for (int i = 0; i < 4; ++i) {
				PlayerIndex testPlayerIndex = (PlayerIndex)i;
				GamePadState testState = GamePad.GetState(testPlayerIndex);
				if (testState.IsConnected) {
					Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
					playerIndex = testPlayerIndex;
					playerIndexSet = true;
				}
			}
		}

		state = GamePad.GetState(playerIndex);

		float inputX = Input.GetAxis("Horizontal");
		float inputY = Input.GetAxis("Vertical");

		moveDirection = inputY * transform.forward + inputX * transform.right;
		controller.SimpleMove(moveDirection * speed * Time.deltaTime);

		

		float rotationX = Input.GetAxis("JoyX2") * lookSpeed * Time.deltaTime + transform.localEulerAngles.y;
		transform.localEulerAngles = new Vector3(0, rotationX, 0);

		GamePad.SetVibration(playerIndex, inputX-0.5f, inputY-0.5f);
		prevState = state;
	}
}
