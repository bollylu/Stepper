using BLTools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StepperLib {
  public class SerialCom {

    public static bool IsDebug = false;

    #region Public properties
    public SerialPort ComPort { get; private set; }
    public string ComPortName { get; set; }
    public int ComPortSpeed { get; set; }
    public Parity ComPortParity { get; set; }
    public int ComPortDataBits { get; set; }
    public StopBits ComPortStopBits { get; set; }
    #endregion Public properties

    #region Constructor(s)
    public SerialCom(string comName = "COM1") {
      ComPortName = comName;
      ComPortSpeed = 115200;
      ComPortParity = Parity.None;
      ComPortDataBits = 8;
      ComPortStopBits = StopBits.One;
    }
    #endregion Constructor(s)

    public void Open() {
      Trace.WriteLineIf(IsDebug, $"Opening com port {ComPortName} ...");
      if (ComPort == null) {
        ComPort = new SerialPort(ComPortName, ComPortSpeed, ComPortParity, ComPortDataBits, ComPortStopBits);
      }
      if (ComPort.IsOpen) {
        Trace.WriteLineIf(IsDebug, $"Com port {ComPortName} is already opened");
        Close();
        return;
      }
      try {
        ComPort.Open();
      } catch (Exception ex) {
        Trace.WriteLineIf(IsDebug, $"Unable to open com port {ComPortName} : {ex.Message}");
        return;
      }
    }

    public void Close() {
      Trace.WriteLineIf(IsDebug, "Closing com port...");
      if (ComPort.IsOpen) {
        ComPort.BaseStream.Flush();
        ComPort.Close();
        ComPort.Dispose();
        ComPort = null;
        return;
      }
    }

    public void SetDirection(EDirection direction) {
      if (ComPort == null) {
        Trace.WriteLineIf(IsDebug, "No associated com port", Severity.Error);
        return;
      }
      if (!ComPort.IsOpen) {
        Trace.WriteLineIf(IsDebug, "Com port is not opened", Severity.Error);
        return;
      }

      try {
        Trace.WriteLineIf(IsDebug, $"Set direction to {direction.ToString()}");
        if (direction == EDirection.Clockwise) {
          ComPort.Write("D");
        } else {
          ComPort.Write("d");
        }
      } catch (Exception ex) {
        Trace.WriteLine($"Unable to set direction {direction.ToString()} on com port {ComPortName} : {ex.Message}", Severity.Error);
      }
    }

    public void SetActive(bool isActive) {
      if (ComPort == null) {
        Trace.WriteLineIf(IsDebug, "No associated com port", Severity.Error);
        return;
      }
      if (!ComPort.IsOpen) {
        Trace.WriteLineIf(IsDebug, "Com port is not opened", Severity.Error);
        return;
      }

      try {
        if (isActive) {
          Trace.WriteLineIf(IsDebug, "Enable ON");
          ComPort.Write("E");
        } else {
          Trace.WriteLineIf(IsDebug, "Enable OFF");
          ComPort.Write("e");
        }
      } catch (Exception ex) {
        Trace.WriteLineIf(IsDebug, $"Unable to send step on com port {ComPortName} : {ex.Message}", Severity.Error);
      }
    }

    public void SendStep() {
      if (ComPort == null) {
        Trace.WriteLineIf(IsDebug, "No associated com port", Severity.Error);
        return;
      }
      if (!ComPort.IsOpen) {
        Trace.WriteLineIf(IsDebug, "Com port is not opened", Severity.Error);
        return;
      }

      try {
        ComPort.Write("C");
      } catch (Exception ex) {
        Trace.WriteLine($"Unable to send step on com port {ComPortName} : {ex.Message}", Severity.Error);
      }
    }

    public static IEnumerable<string> GetSerialPorts() {
      return SerialPort.GetPortNames();
    }

  }
}
