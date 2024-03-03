// ReSharper disable once InconsistentNaming
public static class FSMConstants
{
    #region States

    public const string IdleState = nameof(Core.FSM.IdleState);
    public const string SpinningState = nameof(Core.FSM.SpinningState);
    public const string AccelerationState = nameof(Core.FSM.AccelerationState);
    public const string StartSpinningState = nameof(Core.FSM.StartSpinningState);

    #endregion

    #region Events

    public const string IsSpinning = nameof(IsSpinning);
    public const string FastSpinning = nameof(FastSpinning);
    public const string StartAcceleration = nameof(StartAcceleration);

    #endregion
}