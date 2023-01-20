using System.Collections.Generic;
using System.Threading;
using Alteruna;
using Unity.VisualScripting;
using UnityEngine;
using Avatar = Alteruna.Avatar;

public class PrepareRoundGameState : GameState
{
    private const float DelayBetweenChecksSeconds = 2f;
    private float _nextCheck;
    
    // ReSharper disable Unity.PerformanceAnalysis
    public override void Update()
    {
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
    }

    private void CheckIfCanStart()
    {
        
#if UNITY_EDITOR
        GameManager.Instance.PrintDebug("GameManager - ", "Checking if can start.");
#endif
        
        bool canStart = true;
        
        if (GameManager.Instance.CheckIfEnoughPlayers())
        {
            var avatars = Multiplayer.Instance.GetAvatars();
            
            foreach (var avatar in avatars)
            {
                if (avatar == null)
                    break;
                
                GameObject player = avatar.GameObject();
                
                if (player != null)
                {
                    var playerGameStateSync = player.GetComponentInChildren<PlayerGameStateSync>();
                    if (playerGameStateSync.currentGameState == (byte)GameManager.State.PrepareRound) continue;
                }

                canStart = false;
                break;
            }
        }
        else
        {
            GameManager.Instance.ChangeState(GameManager.State.LookingForPlayers);
            return;
        }

        if (canStart)
        {
            GameManager.Instance.ChangeState(GameManager.State.StartRound);
        }

        
        
    }
}
