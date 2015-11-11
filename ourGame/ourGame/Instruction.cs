using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ourGame
{
  abstract class Instruction
  {
    public abstract InstructionResult Execute(float dt);
    public abstract Instruction Reset();

    static public Instruction operator +(Instruction A, Instruction B)
    {
      return new Instructions.Semicolon(A, B);
    }
  }

    class nop : Instruction {
        public override InstructionResult Execute(float dt) {
            return InstructionResult.Done;
        }

        public override Instruction Reset() {
            return new nop();
        }
    }
}
