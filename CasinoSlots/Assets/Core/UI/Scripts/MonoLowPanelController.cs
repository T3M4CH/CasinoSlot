using AxGrid;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using UnityEngine;
using AxGrid.Base;
using AxGrid.Model;
using TMPro;

public class MonoLowPanelController : MonoBehaviourExtBind
{
    [SerializeField] private TMP_Text balanceText;
    [SerializeField] private Color selectedButtonColor;
    [SerializeField] private Color deselectedButtonColor;
    
    [SerializeField, FoldoutGroup("SpinButton")] private Button spinButton;
    [SerializeField, FoldoutGroup("SpinButton")] private TMP_Text spinText;
    
    [SerializeField, FoldoutGroup("StopButton")] private Button stopButton;
    [SerializeField, FoldoutGroup("StopButton")] private TMP_Text stopText;

    [Bind("IsSpinning")]
    private void ValidateSpinButton(bool interactable)
    {
        stopButton.interactable = interactable;
        stopText.color = interactable ? deselectedButtonColor : selectedButtonColor;
        
        interactable = !interactable;
        spinButton.interactable = interactable;
        spinText.color = interactable ? deselectedButtonColor : selectedButtonColor;
    }
    
    private void PerformStopButton()
    {
        ValidateStopButton(false);
    }

    private void ValidateStopButton(bool interactable)
    {
        stopButton.interactable = interactable;
        stopText.color = interactable ? deselectedButtonColor : selectedButtonColor;
    }

    [Bind("OnMoneyChanged")]
    private void PerformUpdateBalance(int value)
    {
        balanceText.text = $"{value}$";
    }

    [OnDestroy]
    private void DestroyThis()
    {
        spinButton.onClick.RemoveAllListeners(); 
        stopButton.onClick.RemoveAllListeners(); 
    }
}
