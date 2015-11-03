using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ourGame.Instructions
{
  class For : Instruction
  {
    int start, end, i;
    Func<int, Instruction> getBody;
    Instruction body;
    public For(int start, int end, Func<int, Instruction> getBody)
    {
      this.i = start;
      this.start = start;
      this.end = end;
      this.getBody = getBody;
      this.body = getBody(i);
    }

    public override InstructionResult Execute(float dt)
    {
      if (i >= end)
        return InstructionResult.Done;
      else
      {
        switch (body.Execute(dt))
        {
          case InstructionResult.Done:
            i++;
            body = getBody(i);
            return InstructionResult.Running;
          case InstructionResult.DoneAndCreateCylon:
            i++;
            body = getBody(i);
            return InstructionResult.RunningAndCreateCylon;
          case InstructionResult.Running:
            return InstructionResult.Running;
          case InstructionResult.RunningAndCreateCylon:
            return InstructionResult.RunningAndCreateCylon;
        }
        return InstructionResult.Done;
      }
    }

    public override Instruction Reset()
    {
      return new For(start, end, getBody);
    }
  }
}
