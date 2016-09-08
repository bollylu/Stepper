using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StepperWpf {
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window {
    #region Public static variables
    /// <summary>
    /// public global reference to the main window
    /// </summary>
    public static MainWindow Self;
    #endregion Public static variables

    public MainViewModel MainItem;

    public MainWindow() {
      Self = this;
      InitializeComponent();
    }

    #region Menu
    private void mnuFileQuit_Click(object sender, RoutedEventArgs e) {
      Application.Current.Shutdown();
    }
    #endregion Menu

    private void Window_Initialized(object sender, EventArgs e) {
      this.DataContext = MainItem;
    }
  }
}
