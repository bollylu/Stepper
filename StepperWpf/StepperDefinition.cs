using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepperWpf {
  public class StepperDefinition {

    public string Name { get; set; }
    public int MinRpm { get; set; }
    public int MaxRpm { get; set; }

    public StepperDefinition() { }
    public StepperDefinition(string name, int minRpm, int maxRpm) {
      Name = name;
      MinRpm = minRpm;
      MaxRpm = maxRpm;
    }
  }
}
