using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ourGame.Instructions
{
  class CreateCylon : Instruction
  {
    public override InstructionResult Execute(float dt)
    {
      return InstructionResult.DoneAndCreateCylon;
    }

    public override Instruction Reset()
    {
      return new CreateCylon();
    }
  }
}
