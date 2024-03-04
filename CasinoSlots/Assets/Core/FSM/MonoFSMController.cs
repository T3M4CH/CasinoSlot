using Sirenix.OdinInspector;
using AxGrid.Model;
using UnityEngine;
using AxGrid.Base;
using AxGrid.FSM;
using Core.FSM;
using AxGrid;

public class MonoFsmController : MonoBehaviourExtBind
{
    private float _multiplier;

    [OnStart]
    private void PerformStart()
    {
        Settings.Fsm = new FSM();
        Settings.Fsm.Add(new IdleState(), new StartSpinningState(), new SpinningState(), new AccelerationState(), new StopSpinning());

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

    [Bind("CompleteRoll")]
    private void PerformAddBalance()
    {
        Model.Inc("Money", Random.Range(20, 50));
    }

    [Bind("StartAcceleration")]
    private void PerformAcceleration()
    {
        if (Settings.Fsm.CurrentStateName != FSMConstants.StartSpinningState) return;

        Settings.Fsm.Change(FSMConstants.AccelerationState);
    }

    [Bind("OnStopButtonClick")]
    private void PerformStopButtonClicked()
    {
        Settings.Fsm.Change(FSMConstants.StopSpinning);
    }

    //TEST
    [Button]
    private void StopSpinning()
    {
        Model.EventManager.Invoke(FSMConstants.IsSpinning, false);
    }
}