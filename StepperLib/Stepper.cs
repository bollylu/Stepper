using BLTools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepperLib {
  public class Stepper {

    public static bool IsDebug = false;

    public SerialCom StepperSerialCom { get; set; }

    public Stepper(string serialComPort) {
      StepperSerialCom = new SerialCom(serialComPort);
    }

    public async Task Execute(MoveInfoCollection sequence) {
      if (sequence == null || sequence.Items.Count()==0) {
        Trace.WriteLine("Unable to execute sequence when it's null or items are empty", Severity.Error);
        return;
      }

      StepperSerialCom.Open();
      StepperSerialCom.SetActive(true);

      foreach (MoveInfo MoveInfoItem in sequence.Items) {
        StepperSerialCom.SetDirection(MoveInfoItem.Direction);

        for (int RepeatSteps = 1; RepeatSteps <= MoveInfoItem.Iterations; RepeatSteps++) {
          for (int i = 1; i <= MoveInfoItem.Steps; i++) {

            Trace.WriteLineIf(IsDebug, $"  Step {i}");
            StepperSerialCom.SendStep();

            if (MoveInfoItem.GapBetweenSteps > 0) {
              Trace.WriteLineIf(IsDebug, $"  == Waiting for {MoveInfoItem.GapBetweenSteps}");
              await Task.Delay(TimeSpan.FromMilliseconds(MoveInfoItem.GapBetweenSteps));
            }

          }
        }
      }

      StepperSerialCom.SetActive(false);
      StepperSerialCom.Close();
    }

    public async Task Execute(MoveInfo moveInfo) {

      if (moveInfo == null) {
        Trace.WriteLine("Unable to execute moveInfo when it's null", Severity.Error);
        return;
      }

      StepperSerialCom.Open();
      StepperSerialCom.SetActive(true);

      StepperSerialCom.SetDirection(moveInfo.Direction);

      for (int RepeatSteps = 1; RepeatSteps <= moveInfo.Iterations; RepeatSteps++) {
        for (int i = 1; i <= moveInfo.Steps; i++) {

          Trace.WriteLineIf(IsDebug, $"  Step {i}");
          StepperSerialCom.SendStep();

          if (moveInfo.GapBetweenSteps > 0) {
            Trace.WriteLineIf(IsDebug, $"  == Waiting for {moveInfo.GapBetweenSteps}");
            await Task.Delay(TimeSpan.FromMilliseconds(moveInfo.GapBetweenSteps));
          }

        }
      }

      StepperSerialCom.SetActive(false);
      StepperSerialCom.Close();

    }

  }
}
