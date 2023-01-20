using System.Collections.Generic;
using Alteruna;
using Unity.VisualScripting;
using UnityEngine;
using Avatar = UnityEngine.Avatar;

public class PrepareRoundGameState : GameState
{
    private const float DelayBetweenChecksSeconds = 2f;
    private float _nextCheck = DelayBetweenChecksSeconds;
    
    private bool _canStart;
    
    private const float DelayBeforeStartSeconds = 5f;
    private float _startCountdown = DelayBeforeStartSeconds;
    
    // ReSharper disable Unity.PerformanceAnalysis
    public override void Update()
    {
        // if (_canStart)
        // {
        //     StartAfterDelay(Time.deltaTime);
        //     return;
        // }
        
        if (_nextCheck <= 0)
        {
            CheckIfCanStart();
            _nextCheck = DelayBetweenChecksSeconds;
        }

        _nextCheck -= Time.deltaTime;
    }

    public override void Run()
    {
        _nextCheck = DelayBetweenChecksSeconds;
        //_canStart = false;
        _startCountdown = DelayBeforeStartSeconds;
    }

    private void CheckIfCanStart()
    {
#if UNITY_EDITOR
        GameManager.Instance.PrintDebug("GameManager - ", "Checking if can start.");
#endif
        if (GameManager.Instance.CheckIfEnoughPlayers())
            //_canStart = true;
            GameManager.Instance.ChangeState(GameManager.State.StartRound);
        else
            GameManager.Instance.ChangeState(GameManager.State.LookingForPlayers);
    }

//     private void StartAfterDelay(float time)
//     {
//         _startCountdown -= time;
// #if UNITY_EDITOR
//         GameManager.Instance.PrintDebug("GameManager - Round starting in: ", _startCountdown);
// #endif
//         if (_startCountdown > 0) return;
//
//         if (GameManager.Instance.CheckIfEnoughPlayers())
//         {
//             foreach (var avatar in Multiplayer.Instance.GetAvatars())
//             {
//                 if (avatar != null)
//                 {
//                     GameObject player = avatar.gameObject;
//                     PlayerGameStateSync playerGameStateSync = player.GetComponentInChildren<PlayerGameStateSync>();
//                     if (playerGameStateSync.currentGameState != (byte)GameManager.State.PrepareRound)
//                     {
//                         _canStart = false;
// #if UNITY_EDITOR
//                         GameManager.Instance.PrintDebug("GameManager - UNABLE TO START: ", _startCountdown);
// #endif
//                         break;
//                     }
//                 }
//             }
//
//             if (_canStart)
//                 GameManager.Instance.ChangeState(GameManager.State.StartRound);
//         }
//         else
//             _canStart = false;
//         
//     }
}
 