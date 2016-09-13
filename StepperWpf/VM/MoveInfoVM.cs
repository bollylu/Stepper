using BLTools.MVVM;
using StepperLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepperWpf {
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

    public int Iterations {
      get {
        return _Data.Iterations;
      }
      set {
        _Data.Iterations = value;
        NotifyPropertyChanged(nameof(Iterations));
      }
    }

    public int StepDuration {
      get {
        return _Data.StepDuration;
      }
      set {
        _Data.StepDuration = value;
        NotifyPropertyChanged(nameof(StepDuration));
      }
    }

    public int GapBetweenSteps {
      get {
        return _Data.GapBetweenSteps;
      }
      set {
        _Data.GapBetweenSteps = value;
        NotifyPropertyChanged(nameof(GapBetweenSteps));
      }
    }

    public float Speed {
      get {
        return 1000f / (StepDuration + GapBetweenSteps);
      }
    }
    
    public int Id {
      get {
        return _Data.Id;
      }
      set {
        _Data.Id = value;
        NotifyPropertyChanged(nameof(Id));
      }
    }

    public string UpPicture {
      get {
        return App.GetPictureFullname("up");
      }
    }
    public string DownPicture {
      get {
        return App.GetPictureFullname("down");
      }
    }

    public MoveInfoVM() : base() {
      _Data = new MoveInfo(EDirection.Clockwise, 200, 100, 50, 1);
    }

    public MoveInfoVM(MoveInfo moveInfo) : base() {
      _Data = moveInfo;
    }

    
  }
}
