using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ourGame
{
    enum RotationState
    {
        CCW,
        CW,
        NONE
    }

    interface IGameInput
    {
        RotationState CurrentRotationState { get; }
        Boolean ShouldIncreaseSpeed { get;  }

        Boolean ShouldDecreaseSpeed { get; }

        void Update();
    }
}
