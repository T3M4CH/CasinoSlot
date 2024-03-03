using AxGrid;
using AxGrid.FSM;
using AxGrid.Model;

namespace Core.FSM
{
    [State(nameof(SpinningState))]
    public class SpinningState : FSMState
    {
        [Enter]
        private void Enter()
        {
        }
    }
}