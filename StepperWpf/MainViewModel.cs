using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLTools.MVVM;
using System.Collections.ObjectModel;
using StepperLib;
using System.Windows;

namespace StepperWpf {
  public class MainViewModel : MVVMBase {

    #region --- MicroSteps --------------------------------------------
    public ObservableCollection<TStepDisplay> MicroSteps { get; protected set; } = new ObservableCollection<TStepDisplay>();
    public TStepDisplay SelectedMicroStep {
      get {
        return _SelectedMicroStep;
      }
      set {
        _SelectedMicroStep = value;
        NotifyPropertyChanged(nameof(SelectedMicroStep));
        NotifyPropertyChanged(nameof(IsMovementValid));
      }
    }
    private TStepDisplay _SelectedMicroStep;
    #endregion --- MicroSteps --------------------------------------------

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
        if (Direction == EDirection.Clockwise) {
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
        if (SelectedMicroStep == null) {
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

    public ObservableCollection<MoveInfoVM> Sequence { get; set; } = new ObservableCollection<MoveInfoVM>();
    public MoveInfoVM SelectedMoveInfo {
      get {
        return _SelectedMoveInfo;
      }
      set {
        _SelectedMoveInfo = value;
        NotifyPropertyChanged(nameof(SelectedMoveInfo));
      }
    }
    private MoveInfoVM _SelectedMoveInfo;

    public TRelayCommand AddToListCommand { get; private set; }
    public TRelayCommand FileOpenCommand { get; private set; }
    public TRelayCommand HelpContactCommand { get; private set; }
    public TRelayCommand HelpAboutCommand { get; private set; }
    public TRelayCommand<int> RemoveMovementCommand { get; private set; }
    public TRelayCommand<int> MovementUpCommand { get; private set; }
    public TRelayCommand<int> MovementDownCommand { get; private set; }

    public MainViewModel() {
      _Initialize();
    }

    protected void _Initialize() {
      FileOpenCommand = new TRelayCommand(() => _FileOpenCommand(), _ => { return true; });
      HelpContactCommand = new TRelayCommand(() => _HelpContactCommand(), _ => { return true; });
      HelpAboutCommand = new TRelayCommand(() => _HelpAboutCommand(), _ => { return true; });
      MicroSteps.Clear();
      MicroSteps.Add(new TStepDisplay() { StepValue = 200, StepDescription = "200 (1)" });
      MicroSteps.Add(new TStepDisplay() { StepValue = 400, StepDescription = "400 (1/2)" });
      MicroSteps.Add(new TStepDisplay() { StepValue = 800, StepDescription = "800 (1/4)" });
      MicroSteps.Add(new TStepDisplay() { StepValue = 1600, StepDescription = "1600 (1/8)" });
      MicroSteps.Add(new TStepDisplay() { StepValue = 3200, StepDescription = "3200 (1/16)" });
      MicroSteps.Add(new TStepDisplay() { StepValue = 6400, StepDescription = "6400 (1/32)" });
      SelectedMicroStep = MicroSteps.First();
      ChangeDirectionCommand = new TRelayCommand(() => _ChangeDirectionCommand(), _ => { return true; });
      AddToListCommand = new TRelayCommand(() => _AddToListCommand(), _ => { return IsMovementValid; });
      RemoveMovementCommand = new TRelayCommand<int>((x) => _RemoveMovementCommand(x), _ => { return Sequence.Count > 0; });
      MovementUpCommand = new TRelayCommand<int>((x) => _MovementUpCommand(x), _ => { return Sequence.Count > 1; });
      MovementDownCommand = new TRelayCommand<int>((x) => _MovementDownCommand(x), _ => { return Sequence.Count > 1; });
      Speed = 50;
      Iterations = 1;
      Sequence.Clear();
    }


    private void _ChangeDirectionCommand() {
      if (Direction == EDirection.Clockwise) {
        Direction = EDirection.CounterClockwise;
      } else {
        Direction = EDirection.Clockwise;
      }
    }

    private void _AddToListCommand() {
      if (!IsMovementValid) {
        return;
      }
      MoveInfoVM NewMoveInfo = new MoveInfoVM(new MoveInfo(Direction, SelectedMicroStep.StepValue, Speed, Speed, Iterations));
      int LastId = Sequence.Count == 0 ? 0 : Sequence.OrderBy(x => x.Id).Last().Id;
      NewMoveInfo.Id = LastId + 1;
      Sequence.Add(NewMoveInfo);
    }

    private void _RemoveMovementCommand(int id) {
      List<MoveInfoVM> IterationList = new List<MoveInfoVM>(Sequence);
      Sequence.RemoveAt(IterationList.FindIndex(x => x.Id == id));
      for (int i = 1; i <= Sequence.Count; i++) {
        Sequence[i - 1].Id = i;
      }
    }

    private void _MovementUpCommand(int id) {
      if (id == 1) {
        return;
      }
      Sequence[id - 1].Id = Sequence[id - 2].Id;
      Sequence[id - 2].Id = Sequence[id - 2].Id + 1;
      NotifyPropertyChanged(nameof(Sequence));
    }

    private void _MovementDownCommand(int id) {
      if (id == Sequence.Count) {
        return;
      }
      Sequence[id - 1].Id = Sequence[id].Id;
      Sequence[id].Id = Sequence[id].Id -1;
      NotifyPropertyChanged(nameof(Sequence));
    }

    private void _FileOpenCommand() { }
    private void _HelpContactCommand() { }
    private void _HelpAboutCommand() {
      MessageBox.Show("Stepper v0.1");
    }

  }
}
