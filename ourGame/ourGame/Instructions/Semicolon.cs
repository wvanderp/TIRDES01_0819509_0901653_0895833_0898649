using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ourGame.Instructions
{
  class Semicolon : Instruction
  {
    Instruction A, B;
    bool isADone = false, isBDone = false;
    public Semicolon(Instruction A, Instruction B)
    {
      this.A = A;
      this.B = B;
    }

    public override InstructionResult Execute(float dt)
    {
      if (!isADone)
      {
        var Ares = A.Execute(dt);
        switch (Ares)
        {
          case InstructionResult.Done:
            isADone = true;
            return InstructionResult.Running;
          case InstructionResult.DoneAndCreateCylon:
            isADone = true;
            return InstructionResult.RunningAndCreateCylon;
          default:
            return Ares;
        }
      }
      else
      {
        if (!isBDone)
        {
          var Bres = B.Execute(dt);
          switch (Bres)
          {
            case InstructionResult.Done:
              isBDone = true;
              break;
            case InstructionResult.DoneAndCreateCylon:
              isBDone = true;
              break;
          }
          return Bres;
        }
        else
        {
          return InstructionResult.Done;
        }
      }
    }

    public override Instruction Reset()
    {
      return new Semicolon(A.Reset(), B.Reset());
    }
  }
}
