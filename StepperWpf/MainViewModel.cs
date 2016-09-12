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
    public TStepDisplay SelectedStep {
      get {
        return _SelectedStep;
      }
      set {
        _SelectedStep = value;
        NotifyPropertyChanged(nameof(SelectedStep));
        NotifyPropertyChanged(nameof(IsMovementValid));
      }
    }
    private TStepDisplay _SelectedStep;
    public TRelayCommand AddToListCommand { get; private set; }
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
    public string DirectionText {
      get {
        if (Direction==EDirection.Clockwise) {
          return "CW";
        } else {
          return "CCW";
        }
      }
    }
    public string DisplayClockwisePicture {
      get {
        return App.GetPictureFullname("cw-color");
      }
    }
    public string DisplayCounterClockwisePicture {
      get {
        return App.GetPictureFullname("ccw-color");
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
        NotifyPropertyChanged(nameof(IsMovementValid));
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
        NotifyPropertyChanged(nameof(IsMovementValid));
      }
    }
    private int _Iterations;

    public bool IsMovementValid {
      get {
        if (SelectedStep == null) {
          return false;
        }

        if (Speed <= 0 || Speed > 100) {
          return false;
        }

        if (Iterations <= 0 || Iterations > 100) {
          return false;
        }

        return true;
      }
    }

    public ObservableCollection<MoveInfo> Sequence { get; set; } = new ObservableCollection<MoveInfo>();
    public MoveInfo SelectedMoveInfo {
      get {
        return _SelectedMoveInfo;
      }
      set {
        _SelectedMoveInfo = value;
        NotifyPropertyChanged(nameof(SelectedMoveInfo));
      }
    }
    private MoveInfo _SelectedMoveInfo;

    public MainViewModel() {
      _Initialize();
    }

    protected void _Initialize() {
      Steps.Clear();
      Steps.Add(new TStepDisplay() { StepValue = 200, StepDescription = "200 (1)" });
      Steps.Add(new TStepDisplay() { StepValue = 400, StepDescription = "400 (1/2)" });
      Steps.Add(new TStepDisplay() { StepValue = 800, StepDescription = "800 (1/4)" });
      Steps.Add(new TStepDisplay() { StepValue = 1600, StepDescription = "1600 (1/8)" });
      Steps.Add(new TStepDisplay() { StepValue = 3200, StepDescription = "3200 (1/16)" });
      Steps.Add(new TStepDisplay() { StepValue = 6400, StepDescription = "6400 (1/32)" });
      SelectedStep = Steps.First();
      ChangeDirectionCommand = new TRelayCommand(() => ChangeDirectionCmd(), _ => { return true; });
      AddToListCommand = new TRelayCommand(() => AddToListCmd(), _ => { return true; });
      Speed = 50;
      Iterations = 0;
      Sequence.Clear();
    }

    private void ChangeDirectionCmd() {
      if (Direction == EDirection.Clockwise) {
        Direction = EDirection.CounterClockwise;
      } else {
        Direction = EDirection.Clockwise;
      }
    }

    private void AddToListCmd() {
      if (!IsMovementValid) {
        return;
      }
      MoveInfo NewMoveInfo = new MoveInfo(Direction, SelectedStep.StepValue, Speed, Iterations);
      Sequence.Add(NewMoveInfo);
    }

  }
}
