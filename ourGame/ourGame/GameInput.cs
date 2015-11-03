using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ourGame
{
    enum RotationState
    {
        CCW = -1,
        CW = 1,
        NONE = 0
    }

	abstract class GameInput
    {
        public abstract RotationState CurrentRotationState { get; }
		public abstract Boolean ShouldIncreaseSpeed { get; }
		public abstract Boolean ShouldDecreaseSpeed { get; }
		public abstract Boolean TriggerPressed { get; }

		public abstract void Update();

		private class SumInputController : GameInput {
			GameInput a;
			GameInput b;

			public SumInputController(GameInput a, GameInput b) {
				this.a = a;
				this.b = b;
			}

			public override void Update ()
			{
				a.Update ();
				b.Update ();
			}

			public override RotationState CurrentRotationState {
				get {
					int direction = 0;
					direction += (int)a.CurrentRotationState;
					direction += (int)b.CurrentRotationState;
					if (direction < 0) {
						return RotationState.CCW;
					}

					if (direction > 0) {	
						return RotationState.CW;
					}

					return RotationState.NONE;
				}
			}

			public override bool ShouldDecreaseSpeed {
				get {
					return a.ShouldDecreaseSpeed || b.ShouldDecreaseSpeed;
				}
			}

			public override bool ShouldIncreaseSpeed {
				get {
					return a.ShouldIncreaseSpeed || b.ShouldIncreaseSpeed;
				}
			}

			public override bool TriggerPressed {
				get {
					return a.TriggerPressed || b.TriggerPressed;
				}
			}
		}

		public static GameInput operator +(GameInput a, GameInput b){
			return new SumInputController (a, b);
		}
    }
}