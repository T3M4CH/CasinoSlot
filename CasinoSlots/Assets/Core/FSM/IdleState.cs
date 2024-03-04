using System;
using System.Linq;
using AxGrid.FSM;

namespace Core.FSM
{
    [State(nameof(IdleState))]
    public class IdleState : FSMState
    {
        [Enter]
        private void Enter()
        {
            var random = Enum.GetValues(typeof(ECellType)).Cast<ECellType>().RandomElement();
            Model.SilentSet("ECellType", random);
        }
        
        //TODO: Воспроизводить Idle анимки
    }
}