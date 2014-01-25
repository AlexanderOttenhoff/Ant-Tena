using UnityEngine;
using System.Collections;
using XInputDotNetPure;

//[RequireComponent(typeof(CharacterController))]
public class AntController : MonoBehaviour {

	public float speed = 250f;
	public float lookSpeed = 100f;
	public GameManager manager;

	private Vector3 moveDirection = Vector3.zero;
	private CharacterController controller;

	bool playerIndexSet = false;
	public PlayerIndex playerIndex;
	GamePadState state;
	GamePadState prevState;

	AudioSource audioSource;
	bool isPlayingAudio = false;


	void Start() {
		controller = GetComponent<CharacterController>();
		audioSource = GetComponent<AudioSource>();

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

		float inputX = state.ThumbSticks.Left.X;
		float inputY = state.ThumbSticks.Left.Y;

		moveDirection = inputY * transform.forward + inputX * transform.right;
		controller.SimpleMove(moveDirection * speed * Time.deltaTime);

		float rotationX = state.ThumbSticks.Right.X * lookSpeed * Time.deltaTime + transform.localEulerAngles.y;
		transform.localEulerAngles = new Vector3(0, rotationX, 0);

		//GamePad.SetVibration(playerIndex, Mathf.Abs(transform.position.x) / 20f, Mathf.Abs(transform.position.z) / 20f);

		if (!audioSource.isPlaying) {
			if (state.Buttons.A == ButtonState.Pressed && prevState.Buttons.A != ButtonState.Pressed) {
				audioSource.PlayOneShot(manager.antClips[0]);
			}
			if (state.Buttons.B == ButtonState.Pressed && prevState.Buttons.B != ButtonState.Pressed) {
				audioSource.PlayOneShot(manager.antClips[1]);
			}
			if (state.Buttons.X == ButtonState.Pressed && prevState.Buttons.X != ButtonState.Pressed) {
				audioSource.PlayOneShot(manager.antClips[2]);
			}
			if (state.Buttons.Y == ButtonState.Pressed && prevState.Buttons.Y != ButtonState.Pressed) {
				audioSource.PlayOneShot(manager.antClips[3]);
			}
		}

		prevState = state;
		//if (controller.collisionflags & CollisionFlags.sides) {
		//	print("sides touching object");
		//}
	}

	void OnControllerColliderHit(ControllerColliderHit hit) {
		float left=0, right=0;
		VibrationSource source = hit.collider.GetComponent<VibrationSource>() as VibrationSource;
		if (source != null) {
			Debug.Log(source.name);
			if (source.motor== VibrationSource.Motors.Hard) {
				left += source.GetVibration();
			} else {
				right += source.GetVibration();
			}
		}
		GamePad.SetVibration(playerIndex, left, right);
	}
}
