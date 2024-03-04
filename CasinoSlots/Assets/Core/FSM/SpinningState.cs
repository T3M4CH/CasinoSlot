using AxGrid;
using AxGrid.Base;
using AxGrid.FSM;

namespace Core.FSM
{
    [State(nameof(SpinningState))]
    public class SpinningState : FSMState
    {
        [OnDelay(2f)]
        private void ShutEffect()
        {
            Model.EventManager.Invoke("ShutAccelerationEffects");
        }
        
        [One(1f)]
        private void Stop()
        {
            Parent.Change(FSMConstants.StopSpinning);
        }
    }
}