using AxGrid.Base;
using AxGrid.Model;
using AxGrid.Path;
using UnityEngine;

public class MonoParticleController : MonoBehaviourExtBind
{
    [SerializeField] private Transform leftSkull;
    [SerializeField] private Transform rightSkull;
    [SerializeField] private Animator ghostAnimator;

    private static readonly int Laugh = Animator.StringToHash("Laugh");

    [Bind("IsSpinning")]
    private void PerformGhostLaugh(bool value)
    {
        if (!value)
        {
            PerformStopEffects();
        }
        
        ghostAnimator.SetBool(Laugh, value);
    }

    [Bind("AccelerationEffect")]
    private void PerformAccelerationEffect()
    {
        MoveSkull();
    }

    [Bind("ShutAccelerationEffects")]
    private void PerformStopEffects()
    {
        Path = new CPath();

        var startInitScale = leftSkull.localScale;
        Path.EasingCubicEaseOut(0.4f, 1, 0, value =>
        {
            var scale = startInitScale * value;
            leftSkull.localScale = scale;
            scale.x *= -1;
            rightSkull.localScale = scale;

            ghostAnimator.SetBool(Laugh, false);
        });
    }

    private void MoveSkull()
    {
        Path = new CPath();
        Path.EasingBackOut(2, 0, 1, value =>
        {
            var scale = Vector3.one * (Mathf.Min(value, 0.4f) / 0.4f);
            leftSkull.localScale = scale;
            scale.x *= -1;
            rightSkull.localScale = scale;

            var multiplier = Mathf.Abs(Mathf.Sin(2 * Mathf.PI * value));
            var angle = new Vector3(0, 0, 20 * multiplier);
            leftSkull.localEulerAngles = angle;
            angle *= -1;
            rightSkull.localEulerAngles = angle;
        }).Action(() => { PerformGhostLaugh(false); });
    }
}