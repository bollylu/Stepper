using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepperLib {
  public class MoveInfoCollection {

    public List<MoveInfo> Items { get; set; } = new List<MoveInfo>();

    public MoveInfoCollection() { }

    public void AddItem(MoveInfo moveInfo) {
      Items.Add(moveInfo);
    }

  }
}
