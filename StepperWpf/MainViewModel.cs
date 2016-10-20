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
    public ObservableCollection<TComboBoxItem> MicroSteps { get; protected set; } = new ObservableCollection<TComboBoxItem>();
    public TComboBoxItem SelectedMicroStep {
      get {
        return _SelectedMicroStep;
      }
      set {
        _SelectedMicroStep = value;
        NotifyPropertyChanged(nameof(SelectedMicroStep));
        NotifyPropertyChanged(nameof(IsMovementValid));
      }
    }
    private TComboBoxItem _SelectedMicroStep;
    #endregion --- MicroSteps --------------------------------------------

    #region --- Pas de vis --------------------------------------------
    public ObservableCollection<TComboBoxItem> ThreadedShafts { get; protected set; } = new ObservableCollection<TComboBoxItem>();
    public TComboBoxItem SelectedThreadedShaft {
      get {
        return _SelectedThreadedShaft;
      }
      set {
        _SelectedThreadedShaft = value;
        NotifyPropertyChanged(nameof(SelectedThreadedShaft));
        NotifyPropertyChanged(nameof(IsMovementValid));
      }
    }
    private TComboBoxItem _SelectedThreadedShaft;
    #endregion --- Pas de vis --------------------------------------------

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

    private double _StepsForOneMm {
      get {
        return (double)SelectedMicroStep.Value / (double)SelectedThreadedShaft.Value;
      }
    }

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

    public bool rbMoveAsSteps {
      get {
        return _rbMoveAsSteps;
      }
      set {
        _rbMoveAsSteps = value;
        NotifyPropertyChanged(nameof(rbMoveAsSteps));
      }
    }
    private bool _rbMoveAsSteps;

    public bool rbMoveAsDistance {
      get {
        return _rbMoveAsDistance;
      }
      set {
        _rbMoveAsDistance = value;
        NotifyPropertyChanged(nameof(rbMoveAsDistance));
      }
    }
    private bool _rbMoveAsDistance;

    public int txtMoveAsSteps {
      get {
        return _txtMoveAsSteps;
      }
      set {
        _txtMoveAsSteps = value;
        NotifyPropertyChanged(nameof(txtMoveAsSteps));
        _txtMoveAsDistance = Math.Round(_txtMoveAsSteps / _StepsForOneMm, 2);
        NotifyPropertyChanged(nameof(txtMoveAsDistance));
      }
    }
    private int _txtMoveAsSteps;

    public double txtMoveAsDistance {
      get {
        return _txtMoveAsDistance;
      }
      set {
        _txtMoveAsDistance = value;
        NotifyPropertyChanged(nameof(txtMoveAsDistance));
        _txtMoveAsSteps = (int)Math.Round(_StepsForOneMm * _txtMoveAsDistance, 0);
        NotifyPropertyChanged(nameof(txtMoveAsSteps));

      }
    }
    private double _txtMoveAsDistance;

    public ObservableCollection<StepperDefinition> StepperDefinitions { get; protected set; } = new ObservableCollection<StepperDefinition>();
    public StepperDefinition SelectedStepperDefinition {
      get {
        return _SelectedStepperDefinition;
      }
      set {
        _SelectedStepperDefinition = value;
        NotifyPropertyChanged(nameof(SelectedStepperDefinition));
        NotifyPropertyChanged(nameof(IsMovementValid));
      }
    }
    private StepperDefinition _SelectedStepperDefinition;

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
    public TRelayCommand<string> ChangeStepperCommand { get; private set; }

    public MainViewModel() {
      _Initialize();
    }

    protected void _Initialize() {
      FileOpenCommand = new TRelayCommand(() => _FileOpenCommand(), _ => { return true; });
      HelpContactCommand = new TRelayCommand(() => _HelpContactCommand(), _ => { return true; });
      HelpAboutCommand = new TRelayCommand(() => _HelpAboutCommand(), _ => { return true; });
      MicroSteps.Clear();
      MicroSteps.Add(new TComboBoxItem() { Value = 200, Description = "200 (1)" });
      MicroSteps.Add(new TComboBoxItem() { Value = 400, Description = "400 (1/2)" });
      MicroSteps.Add(new TComboBoxItem() { Value = 800, Description = "800 (1/4)" });
      MicroSteps.Add(new TComboBoxItem() { Value = 1600, Description = "1600 (1/8)" });
      MicroSteps.Add(new TComboBoxItem() { Value = 3200, Description = "3200 (1/16)" });
      MicroSteps.Add(new TComboBoxItem() { Value = 6400, Description = "6400 (1/32)" });
      SelectedMicroStep = MicroSteps.First();

      ThreadedShafts.Clear();
      ThreadedShafts.Add(new TComboBoxItem() { Value = 0.8f, Description = "M5 (0.80)" });
      ThreadedShafts.Add(new TComboBoxItem() { Value = 1, Description = "M6 (1.00)" });
      ThreadedShafts.Add(new TComboBoxItem() { Value = 1.25f, Description = "M8 (1.25)" });
      ThreadedShafts.Add(new TComboBoxItem() { Value = 1.5f, Description = "M10 (1.50)" });
      ThreadedShafts.Add(new TComboBoxItem() { Value = 1.75f, Description = "M12 (1.75)" });
      SelectedThreadedShaft = ThreadedShafts.FirstOrDefault(x => x.Value == 1.25f);

      ChangeDirectionCommand = new TRelayCommand(() => _ChangeDirectionCommand(), _ => { return true; });
      AddToListCommand = new TRelayCommand(() => _AddToListCommand(), _ => { return IsMovementValid; });
      RemoveMovementCommand = new TRelayCommand<int>((x) => _RemoveMovementCommand(x), _ => { return Sequence.Count > 0; });
      MovementUpCommand = new TRelayCommand<int>((x) => _MovementUpCommand(x), _ => { return Sequence.Count > 1; });
      MovementDownCommand = new TRelayCommand<int>((x) => _MovementDownCommand(x), _ => { return Sequence.Count > 1; });
      Speed = 50;
      Iterations = 1;
      Sequence.Clear();

      rbMoveAsSteps = false;
      rbMoveAsDistance = true;

      StepperDefinitions.Clear();
      StepperDefinitions.Add(new StepperDefinition("Nema 17", 1, 1407) { ChangeCommand = ChangeStepperCommand });
      StepperDefinitions.Add(new StepperDefinition("Nema 23", 1, 1340) { ChangeCommand = ChangeStepperCommand });
      ChangeStepperCommand = new TRelayCommand<string>((x) => _ChangeStepperCommand(x), _ => { return true; });
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
      MoveInfoVM NewMoveInfo = new MoveInfoVM(new MoveInfo(Direction, (int)SelectedMicroStep.Value, Speed, Iterations));
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
      Sequence[id].Id = Sequence[id].Id - 1;
      NotifyPropertyChanged(nameof(Sequence));
    }

    private void _ChangeStepperCommand(string stepperDefinitionName) {
      if (stepperDefinitionName == null) {
        return;
      }
      if (StepperDefinitions==null ||StepperDefinitions.Count==0) {
        return;
      }
      SelectedStepperDefinition = StepperDefinitions.FirstOrDefault(x => x.Name == stepperDefinitionName);
    }

    private void _FileOpenCommand() { }
    private void _HelpContactCommand() { }
    private void _HelpAboutCommand() {
      MessageBox.Show("Stepper v0.1");
    }

  }
}
