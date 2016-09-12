using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLTools;
using StepperLib;
using BLTools.ConsoleExtension;
using BLTools.Debugging;

namespace StepperConsole {
  class Program {
    static void Main(string[] args) {

      SplitArgs Args = new SplitArgs(args);

      //TraceFactory.AddTraceConsole();
      TraceFactory.AddTraceDefaultLogFilename();

      ApplicationInfo.ApplicationStart();

      Test();

      ConsoleExtension.Pause();
      ApplicationInfo.ApplicationStop();
    }

    public static void Test() {
      Stepper CurrentStepper = new Stepper("COM3");

      MoveInfoCollection MySequence = new MoveInfoCollection();
      MySequence.AddItem(new MoveInfo(EDirection.CounterClockwise, 5, 100));
      MySequence.AddItem(new MoveInfo(EDirection.Clockwise, 5, 100));

      Task.Run(() => CurrentStepper.Execute(MySequence)).Wait();

      //Task.Run(() => CurrentStepper.Execute(new MoveInfo(EDirection.Clockwise, 5, 100))).Wait();
    }
  }
}