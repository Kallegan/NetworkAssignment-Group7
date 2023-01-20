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
        GameManager.Instance.ChangeState(GameManager.Instance.CheckIfEnoughPlayers()
            ? GameManager.State.StartRound
            : GameManager.State.LookingForPlayers);
    }
}
 