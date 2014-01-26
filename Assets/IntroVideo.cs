using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class IntroVideo : MonoBehaviour {

	public MovieTexture movie;

	private PlayerIndex playerIndex;
	private GamePadState state;

	void OnGUI() {
		movie.Play();
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), movie, ScaleMode.StretchToFill);

	}

	// Update is called once per frame
	void Update() {
		if (playerIndex == null) {
			for (int i = 0; i < 4; ++i) {
				PlayerIndex testPlayerIndex = (PlayerIndex)i;
				GamePadState testState = GamePad.GetState(testPlayerIndex);
				if (testState.IsConnected) {
					Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
					playerIndex = testPlayerIndex;
				}
			}
		}

		state = GamePad.GetState(playerIndex);
		if (state.Buttons.A == ButtonState.Pressed || state.Buttons.Start == ButtonState.Pressed || (!movie.isPlaying && !audio.isPlaying)) {
			Screen.showCursor = true;
			Application.LoadLevel(1);
		}
	}
}
