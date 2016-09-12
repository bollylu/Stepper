using BLTools;
using BLTools.Debugging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace StepperWpf {
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application {
    private void Application_Startup(object sender, StartupEventArgs e) {
      SplitArgs Args = new SplitArgs(Environment.GetCommandLineArgs());

      TraceFactory.AddTraceDefaultLogFilename();

      ApplicationInfo.ApplicationStart();
    }

    private void Application_Exit(object sender, ExitEventArgs e) {
      ApplicationInfo.ApplicationStop();
    }

    public static string GetPictureFullname(string name = "default") {
      return string.Format("/StepperWpf;component/Pictures/{0}.png", name);
    }
  }
}
