using AxGrid.Path;
using AxGrid.Utils;

public static class ExtensionClassAx
{
    private static CPath Apply(
        CPath path,
        CPathEasingAction action,
        DEasing method,
        float from,
        float to,
        float time)
    {
        return path.Add(p =>
        {
            if (p.DeltaF < (double)time)
            {
                action(method(p.DeltaF, from, to, time));
                return Status.Continue;
            }

            action(to);
            return Status.OK;
        });
    }

    public static CPath EasingBackOut(
        this CPath path,
        float time,
        float from,
        float to,
        CPathEasingAction action)
    {
        return Apply(path, action, BackEaseOut, from, to, time);
    }
    
    /// <summary>
    /// 1.701 - overshoot is default const in dotween plugin that how it works : 
    /// return (EaseFunction) ((time, duration, overshootOrAmplitude, period) =&gt; (float) ((double) (time = (float) ((double) time / (double) duration - 1.0)) * (double) time * (((double) overshootOrAmplitude + 1.0) * (double) time + (double) overshootOrAmplitude) + 1.0)); 
    /// </summary>
    /// <returns></returns>
    public static float BackEaseOut(float t, float b, float c, float d) => (float)((double)(t = (float)((double)t / d - 1.0)) * t * (2.701 * t + 1.703) + 1.0);

    private delegate float DEasing(float delta, float from, float to, float time);
}