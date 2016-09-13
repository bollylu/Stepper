using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepperLib {
  public class MoveInfo {
    public int Id { get; set; }
    public EDirection Direction { get; set; }
    public int Steps { get; set; }
    public int StepDuration { get; set; }
    public int GapBetweenSteps { get; set; }
    public int Iterations { get; set; }

    /// <summary>
    /// Blank constructor
    /// </summary>
    public MoveInfo() {
      Id = 0;
      Direction = EDirection.Clockwise;
      Steps = 0;
      StepDuration = 0;
      GapBetweenSteps = 0;
      Iterations = 0;
    }

    /// <summary>
    /// Defines how to move
    /// </summary>
    /// <param name="direction">Direction of the move</param>
    /// <param name="steps">Number of steps</param>
    /// <param name="stepDuration">Duration of a step (msec)</param>
    /// <param name="gapBetweenSteps">How long to wait between each individual step (msec)</param>
    /// <param name="iterations">Number of repetition of this movement</param>
    public MoveInfo(EDirection direction, int steps, int stepDuration = 100, int gapBetweenSteps = 100, int iterations = 1) {
      Id = 0;
      Direction = direction;
      Steps = steps;
      StepDuration = stepDuration;
      GapBetweenSteps = gapBetweenSteps;
      Iterations = iterations;
    }

    
  }
}
