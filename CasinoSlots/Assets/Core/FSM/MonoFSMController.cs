using AxGrid.Model;
using AxGrid.Base;
using AxGrid.FSM;
using AxGrid;
using Sirenix.OdinInspector;

public class MonoFsmController : MonoBehaviourExtBind
{
    [OnStart]
    private void PerformStart()
    {
        Settings.Fsm = new FSM();
    }

    [Bind("OnSpinButtonClick")]
    private void PerformSpinButtonClicked()
    {
        Log.Info("Spin Clicked");
        Model.EventManager.Invoke("IsSpinning", true);
    }

    //TEST
    [Button]
    private void StopSpinning()
    {
        Model.EventManager.Invoke("IsSpinning", false);
    }
}
