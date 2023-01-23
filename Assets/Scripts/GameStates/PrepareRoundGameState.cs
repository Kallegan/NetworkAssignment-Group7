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
        _startCountdown = DelayBeforeStartSeconds;
    }

    private void CheckIfCanStart()
    {
#if UNITY_EDITOR
        GameManager.Instance.PrintDebug("GameManager - ", "Checking if can start.");
#endif
        if (GameManager.Instance.CheckIfEnoughPlayers())
        {
            //GameManager.Instance.ChangeState(GameManager.State.StartRound);
            
        }
            
        else
            GameManager.Instance.ChangeState(GameManager.State.LookingForPlayers);
    }

    private void TryToStart()
    {
        
    }

}
 