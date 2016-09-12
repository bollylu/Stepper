using BLTools.MVVM;
using StepperLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepperWpf.VM {
  public class MoveInfoVM : MVVMBase {

    protected MoveInfo _Data { get; set; }

    public EDirection Direction {
      get {
        return _Data.Direction;
      }
      set {
        _Data.Direction = value;
        NotifyPropertyChanged(nameof(Direction));
      }
    }
    public string DirectionText {
      get {
        if (Direction == EDirection.Clockwise) {
          return "CW";
        } else {
          return "CCW";
        }
      }
    }

    public int Steps {
      get {
        return _Data.Steps;
      }
      set {
        _Data.Steps = value;
        NotifyPropertyChanged(nameof(Steps));
      }
    }
  }
}
