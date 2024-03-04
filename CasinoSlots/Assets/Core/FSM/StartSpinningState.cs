using AxGrid;
using AxGrid.FSM;

namespace Core.FSM
{
    [State(nameof(StartSpinningState))]
    public class StartSpinningState : FSMState
    {
        [Enter]
        private void Enter()
        {
            Model.EventManager.Invoke(FSMConstants.IsSpinning, true);
        }
    }
}