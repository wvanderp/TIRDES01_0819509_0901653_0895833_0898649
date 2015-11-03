using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace ourGame
{
    class WASDKeyboardInputController : GameInput
    {
        KeyboardState state;

        override public void Update()
        {
            state = Keyboard.GetState();

        }

		override public RotationState CurrentRotationState { get {
            if (state.IsKeyDown(Keys.A) && state.IsKeyDown(Keys.D)) {
                return RotationState.NONE;
            }
            else {
                if (state.IsKeyDown(Keys.A)) {
                    return RotationState.CCW;
                }
                if(state.IsKeyDown(Keys.D)) {
                    return RotationState.CW;
                }
            }
            return RotationState.NONE;
        } }

		override public Boolean ShouldIncreaseSpeed
        {
            get { return state.IsKeyDown(Keys.W); }
        }

		override public Boolean ShouldDecreaseSpeed
        {
            get { return state.IsKeyDown(Keys.S); }
        }

		override public Boolean TriggerPressed {
            get {
                return state.IsKeyDown(Keys.Space);
            }
        }
    }
}
