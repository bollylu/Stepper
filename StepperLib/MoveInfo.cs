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

    /// <summary>
    /// Defines how to move
    /// </summary>
    /// <param name="direction">Direction of the move</param>
    /// <param name="steps">Number of steps</param>
    /// <param name="gapBetweenSteps">How long to wait between each individual step</param>
    public MoveInfo(EDirection direction, int steps, long gapBetweenSteps = 100) {
      Direction = direction;
      Steps = steps;
      GapBetweenSteps = gapBetweenSteps;
    }

    /// <summary>
    /// Starts a movement
    /// </summary>
    /// <returns></returns>
    public async Task Start() {

      SerialCom MySerial = new SerialCom();
      MySerial.Open();

      Trace.WriteLine("Enable ON");
      MySerial.SetActive(true);

      Trace.WriteLine($"Set direction to {Direction.ToString()}");
      MySerial.SetDirection(Direction);

      for (int i = 1; i <= Steps; i++) {

        Trace.WriteLine($"  Step {i}");
        MySerial.SendStep();

        Trace.WriteLine($"  == Waiting for {GapBetweenSteps}");
        await Task.Delay(TimeSpan.FromMilliseconds(GapBetweenSteps));

      }

      MySerial.Close();
      Trace.WriteLine("Enable OFF");
    }
  }
}
