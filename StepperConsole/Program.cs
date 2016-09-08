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

      TraceFactory.AddTraceConsole();
      TraceFactory.AddTraceDefaultLogFilename();

      ApplicationInfo.ApplicationStart();

      Task.Run(() => Test()).Wait();

      ConsoleExtension.Pause();
      ApplicationInfo.ApplicationStop();
    }

    public static async Task Test() {
      MoveInfo TestMove = new MoveInfo(EDirection.Clockwise, 5);
      await TestMove.Start();
    }
  }
}