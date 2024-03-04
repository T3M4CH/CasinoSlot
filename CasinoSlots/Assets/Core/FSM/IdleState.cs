using System;
using System.Linq;
using System.Threading;
using AxGrid;
using AxGrid.FSM;
using Cysharp.Threading.Tasks;
using log4net;
using UnityEngine;
using UnityEngine.Networking;

namespace Core.FSM
{
    [State(nameof(IdleState))]
    public class IdleState : FSMState
    {
        private bool? _isInternetConnected;
        private CancellationTokenSource _tokenSource;

        [Enter]
        private void Enter()
        {
            if (!_isInternetConnected.HasValue)
            {
                _tokenSource = new CancellationTokenSource();

                UniTask.Void(async () =>
                {
                    try
                    {
                        var token = _tokenSource.Token;
                        var webRequest = await UnityWebRequest.Get("http://localhost:5143/API/Account/GetCellType").SendWebRequest().WithCancellation(token);

                        var jsonResponse = webRequest.downloadHandler.text;

                        var response = JsonUtility.FromJson<ResponseData>(jsonResponse);
                        Model.SilentSet("ECellType", response.cellType);
                    }
                    catch
                    {
                        Log.Warn("ИНТЕРНЕТА НЕТ");
                        _isInternetConnected = false;
                    }
                });
            }

            if (_isInternetConnected == false)
            {
                var random = Enum.GetValues(typeof(ECellType)).Cast<ECellType>().RandomElement();
                Model.SilentSet("ECellType", random);
            }
        }

        [Exit]
        private void Exit()
        {
            _tokenSource?.Dispose();
        }

        [System.Serializable]
        public class ResponseData
        {
            public int cellType;
        }
        //TODO: Воспроизводить Idle анимки
    }
}