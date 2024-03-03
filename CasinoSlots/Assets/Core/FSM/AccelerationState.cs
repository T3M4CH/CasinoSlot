using System;
using System.Threading;
using AxGrid;
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

                        if (time > AccelerationDelay && particlesUsed == false)
                        {
                            particlesUsed = true;
                            Model.EventManager.Invoke("AccelerationParticle");
                        }
                    }
                }
                catch
                {
                    Debug.LogError("Catch");
                }
            });
        }

        [Exit]
        private void Exit()
        {
            _tokenSource.Cancel();
            _tokenSource.Dispose();
        }
        
        
    }
}