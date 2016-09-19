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
      ComPortSpeed = 250000;
      ComPortParity = Parity.None;
      ComPortDataBits = 8;
      ComPortStopBits = StopBits.One;
    }
    #endregion Constructor(s)

    public void Open() {
      Trace.WriteLine($"Opening com port {ComPortName} ...");
      if (ComPort == null) {
        ComPort = new SerialPort(ComPortName, ComPortSpeed, ComPortParity, ComPortDataBits, ComPortStopBits);
      }
      if (ComPort.IsOpen) {
        Trace.WriteLine($"Com port {ComPortName} is already opened");
        Close();
        return;
      }
      try {
        ComPort.Open();
      } catch (Exception ex) {
        Trace.WriteLine($"Unable to open com port {ComPortName} : {ex.Message}");
        return;
      }
    }

    public void Close() {
      Trace.WriteLine("Closing com port...");
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
        Trace.WriteLine("No associated com port", Severity.Error);
        return;
      }
      if (!ComPort.IsOpen) {
        Trace.WriteLine("Com port is not opened", Severity.Error);
        return;
      }

      try {
        Trace.WriteLine($"Set direction to {direction.ToString()}");
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
        Trace.WriteLine("No associated com port", Severity.Error);
        return;
      }
      if (!ComPort.IsOpen) {
        Trace.WriteLine("Com port is not opened", Severity.Error);
        return;
      }

      try {
        if (isActive) {
          Trace.WriteLine("Enable ON");
          ComPort.Write("E");
        } else {
          Trace.WriteLine("Enable OFF");
          ComPort.Write("e");
        }
      } catch (Exception ex) {
        Trace.WriteLine($"Unable to send step on com port {ComPortName} : {ex.Message}", Severity.Error);
      }
    }

    public void SendStep() {
      if (ComPort == null) {
        Trace.WriteLine("No associated com port", Severity.Error);
        return;
      }
      if (!ComPort.IsOpen) {
        Trace.WriteLine("Com port is not opened", Severity.Error);
        return;
      }

      try {
        ComPort.Write("C");
      } catch (Exception ex) {
        Trace.WriteLine($"Unable to send step on com port {ComPortName} : {ex.Message}", Severity.Error);
      }
    }
  }
}
