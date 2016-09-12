using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLTools.MVVM;
using System.Collections.ObjectModel;
using StepperLib;

namespace StepperWpf {
  public class MainViewModel : MVVMBase {

    #region --- Steps --------------------------------------------
    public ObservableCollection<TStepDisplay> Steps { get; protected set; } = new ObservableCollection<TStepDisplay>();
    public TStepDisplay SelectedStep { get; set; } 
    #endregion --- Steps --------------------------------------------

    #region --- Direction --------------------------------------------
    public EDirection Direction {
      get {
        return _Direction;
      }

      set {
        _Direction = value;
        NotifyPropertyChanged(nameof(Direction));
        NotifyPropertyChanged(nameof(DisplayClockwise));
        NotifyPropertyChanged(nameof(DisplayCounterClockwise));
      }
    }
    private EDirection _Direction;
    public bool DisplayClockwise {
      get {
        return Direction == EDirection.Clockwise;
      }
      set {
        if (value) {
          Direction = EDirection.Clockwise;
        } else {
          Direction = EDirection.CounterClockwise;
        }
      }
    }
    public bool DisplayCounterClockwise {
      get {
        return !DisplayClockwise;
      }
    }

    public string DisplayClockwisePicture {
      get {
        return App.GetPictureFullname("cw");
      }
    }
    public string DisplayCounterClockwisePicture {
      get {
        return App.GetPictureFullname("ccw");
      }
    }
    public TRelayCommand ChangeDirectionCommand { get; private set; } 
    #endregion --- Direction --------------------------------------------

    public int Speed {
      get {
        return _Speed;
      }
      set {
        _Speed = value;
        NotifyPropertyChanged(nameof(Speed));
      }
    }
    private int _Speed;

    public int Iterations {
      get {
        return _Iterations;
      }
      set {
        _Iterations = value;
        NotifyPropertyChanged(nameof(Iterations));
      }
    }
    private int _Iterations;

    public MainViewModel() {
      _Initialize();
    }

    protected void _Initialize() {
      Steps.Add(new TStepDisplay() { StepValue = 200, StepDescription = "200 (1)" });
      Steps.Add(new TStepDisplay() { StepValue = 400, StepDescription = "400 (1/2)" });
      Steps.Add(new TStepDisplay() { StepValue = 800, StepDescription = "800 (1/4)" });
      Steps.Add(new TStepDisplay() { StepValue = 1600, StepDescription = "1600 (1/8)" });
      Steps.Add(new TStepDisplay() { StepValue = 3200, StepDescription = "3200 (1/16)" });
      Steps.Add(new TStepDisplay() { StepValue = 6400, StepDescription = "6400 (1/32)" });
      SelectedStep = Steps.First();
      ChangeDirectionCommand = new TRelayCommand(() => ChangeDirectionCmd(), _ => { return true; });
      Speed = 100;
      Iterations = 1;
    }

    private void ChangeDirectionCmd() {
      if (Direction == EDirection.Clockwise) {
        Direction = EDirection.CounterClockwise;
      } else {
        Direction = EDirection.Clockwise;
      }
    }

  }
}
