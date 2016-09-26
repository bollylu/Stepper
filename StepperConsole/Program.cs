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

      foreach(string SerialPortItem in SerialCom.GetSerialPorts()) {
        Console.WriteLine(SerialPortItem);
      }

      Stepper CurrentStepper = new Stepper("COM4");

      MoveInfoCollection MySequence = new MoveInfoCollection();
      MySequence.AddItem(new MoveInfo(EDirection.Clockwise, 20, 1));
      //MySequence.AddItem(new MoveInfo(EDirection.Clockwise, 20, 0.8));
      //MySequence.AddItem(new MoveInfo(EDirection.Clockwise, 20, 0.6));
      //MySequence.AddItem(new MoveInfo(EDirection.Clockwise, 20, 1));
      //MySequence.AddItem(new MoveInfo(EDirection.Clockwise, 4500, 0.2));
      //MySequence.AddItem(new MoveInfo(EDirection.Clockwise, 100, 0.5));
      //MySequence.AddItem(new MoveInfo(EDirection.CounterClockwise, 4690, 0));

      for (int i = 1; i <= 10; i++) {
        TChrono NewChrono = new TChrono();
        NewChrono.Start();
        Task.Run(() => CurrentStepper.Execute(MySequence)).Wait();
        Console.WriteLine($"Execution time = {NewChrono.ElapsedTime.TotalMilliseconds}");
      }
      //Task.Run(() => CurrentStepper.Execute(new MoveInfo(EDirection.Clockwise, 5, 100))).Wait();
    }
  }
}