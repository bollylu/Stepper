using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepperLib {
  public class MoveInfo {
    public EDirection Direction { get; set; }
    public int Steps { get; set; }
    public long GapBetweenSteps { get; set; }
    public int Iterations { get; set; }


    public MoveInfo() {
      Direction = EDirection.Clockwise;
      Steps = 0;
      GapBetweenSteps = 0;
      Iterations = 0;
    }

    /// <summary>
    /// Defines how to move
    /// </summary>
    /// <param name="direction">Direction of the move</param>
    /// <param name="steps">Number of steps</param>
    /// <param name="gapBetweenSteps">How long to wait between each individual step</param>
    public MoveInfo(EDirection direction, int steps, long gapBetweenSteps = 100, int iterations = 1) {
      Direction = direction;
      Steps = steps;
      GapBetweenSteps = gapBetweenSteps;
      Iterations = iterations;
    }

    
  }
}
