using Sirenix.OdinInspector;
using AxGrid.Model;
using UnityEngine;
using AxGrid.Base;
using AxGrid.FSM;
using Core.FSM;
using AxGrid;

public class MonoFsmController : MonoBehaviourExtBind
{
    [OnStart]
    private void PerformStart()
    {
        Settings.Fsm = new FSM();
        Settings.Fsm.Add(new IdleState(), new StartSpinningState(), new SpinningState(), new AccelerationState());
        
        Settings.Fsm.Start(nameof(IdleState));
    }

    [OnUpdate]
    private void PerformUpdate()
    {
        Settings.Fsm.Update(Time.deltaTime);
    }

    [Bind("OnSpinButtonClick")]
    private void PerformSpinButtonClicked()
    {
        Settings.Fsm.Change(FSMConstants.StartSpinningState);
    }

    [Bind("StartAcceleration")]
    private void PerformAcceleration()
    {
        Settings.Fsm.Change(FSMConstants.AccelerationState);
    }
    
    //TEST
    [Button]
    private void StopSpinning()
    {
        Model.EventManager.Invoke(FSMConstants.IsSpinning, false);
    }
}
