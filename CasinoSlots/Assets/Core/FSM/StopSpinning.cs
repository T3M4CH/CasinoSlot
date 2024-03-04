using AxGrid;
using AxGrid.FSM;
using Unity.VisualScripting;

namespace Core.FSM
{
    [AxGrid.FSM.State(nameof(StopSpinning))]
    public class StopSpinning : FSMState
    {
        [Enter]
        private void Enter()
        {
            if (Parent.PreviousStateName == FSMConstants.StartSpinningState)
            {
                Model.SilentSet("Speed", 100);
            }
            
            Model.EventManager.Invoke("IsSpinning", false);
        }
    }
}