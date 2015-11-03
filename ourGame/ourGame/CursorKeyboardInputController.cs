using System;
using Microsoft.Xna.Framework.Input;

namespace ourGame
{
	class CursorKeyboardInputController : GameInput
	{
		KeyboardState state;

		override public void Update()
		{
			state = Keyboard.GetState();

		}

		override public RotationState CurrentRotationState { get {
				if (state.IsKeyDown(Keys.Left) && state.IsKeyDown(Keys.Right)) {
					return RotationState.NONE;
				}
				else {
					if (state.IsKeyDown(Keys.Left)) {
						return RotationState.CCW;
					}
					if(state.IsKeyDown(Keys.Right)) {
						return RotationState.CW;
					}
				}
				return RotationState.NONE;
			} }

		override public Boolean ShouldIncreaseSpeed
		{
			get { return state.IsKeyDown(Keys.Up); }
		}

		override public Boolean ShouldDecreaseSpeed
		{
			get { return state.IsKeyDown(Keys.Down); }
		}

		override public Boolean TriggerPressed {
			get {
				return state.IsKeyDown(Keys.RightShift);
			}
		}
	}
}

