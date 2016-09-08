using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
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
      ComPortSpeed = 9600;
      ComPortParity = Parity.None;
      ComPortDataBits = 8;
      ComPortStopBits = StopBits.One;
    }
    #endregion Constructor(s)

    public void Open() {
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
      if (ComPort.IsOpen) {
        ComPort.Close();
        ComPort.Dispose();
        ComPort = null;
        return;
      }
    }

    public void SetDirection(EDirection direction) {
      if (ComPort == null) {
        Trace.WriteLine("No associated com port");
        return;
      }
      if (!ComPort.IsOpen) {
        Trace.WriteLine("Com port is not opened");
        return;
      }

      try {
        ComPort.DtrEnable = (direction == EDirection.Clockwise);
      } catch (Exception ex) {
        Trace.WriteLine($"Unable to set direction {direction.ToString()} on com port {ComPortName} : {ex.Message}");
      }
    }

    public void SetActive(bool isActive) {
      if (ComPort == null) {
        Trace.WriteLine("No associated com port");
        return;
      }
      if (!ComPort.IsOpen) {
        Trace.WriteLine("Com port is not opened");
        return;
      }

      try {
        ComPort.RtsEnable = isActive;
      } catch (Exception ex) {
        Trace.WriteLine($"Unable to set active to {isActive.ToString()} on com port {ComPortName} / {ex.Message}");
      }
    }

    public void SendStep() {
      if (ComPort == null) {
        Trace.WriteLine("No associated com port");
        return;
      }
      if (!ComPort.IsOpen) {
        Trace.WriteLine("Com port is not opened");
        return;
      }

      try {
        ComPort.Write("X");
      } catch (Exception ex) {
        Trace.WriteLine($"Unable to send step on com port {ComPortName} : {ex.Message}");
      }
    }
  }
}
