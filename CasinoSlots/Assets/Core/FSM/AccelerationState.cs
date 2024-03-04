using System;
using System.Threading;
using AxGrid;
using AxGrid.Base;
using Cysharp.Threading.Tasks;
using AxGrid.FSM;
using UnityEngine;

namespace Core.FSM
{
    [State(nameof(AccelerationState))]
    public class AccelerationState : FSMState
    {
        private const float ParticlesDelay = 0.3f;
        private const float AccelerationDelay = 1f;
        private CancellationTokenSource _tokenSource;

        [Enter]
        private void Enter()
        {
            // Why can't use Path inside??)
            // No destroy attribute =((

            Model.Set("GhostLaugh", true);

            _tokenSource = new CancellationTokenSource();

            UniTask.Void(async () =>
            {
                var token = _tokenSource.Token;

                var time = 0f;
                var particlesUsed = false;
                try
                {
                    while (time < AccelerationDelay)
                    {
                        time += Time.deltaTime;
                        await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken: token);
                        Model.SilentSet("Speed", Mathf.Lerp(40, 100, time / AccelerationDelay));

                        if (time > ParticlesDelay && particlesUsed == false)
                        {
                            particlesUsed = true;
                            Model.EventManager.Invoke("AccelerationEffect");
                        }
                    }

                    if (Parent.CurrentStateName == nameof(AccelerationState))
                    {
                        Parent.Change(nameof(SpinningState));
                    }
                }
                catch
                {
                    Log.Warn("Accelerate State was corrupted");
                }
            });
        }

        [Exit]
        private void Exit()
        {
            _tokenSource.Cancel();
            _tokenSource.Dispose();

            Model.SilentSet("Speed", 100);
        }
    }
}