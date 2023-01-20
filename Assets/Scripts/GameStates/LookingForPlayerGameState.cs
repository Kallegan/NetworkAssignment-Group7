using Alteruna;
using UnityEngine;

public class LookingForPlayerGameState : GameState
{
    private const float DelayBetweenChecksSeconds = 2f;
    private float _nextCheck;
    
    // ReSharper disable Unity.PerformanceAnalysis
    public override void Update()
    {
        if (_nextCheck <= 0)
        {
            CheckIfPlayersFound();
            _nextCheck = DelayBetweenChecksSeconds;
            return;
        }

        _nextCheck -= Time.deltaTime;
    }

    public override void Run()
    {
        _nextCheck = DelayBetweenChecksSeconds;
    }

    private void CheckIfPlayersFound()
    {
        var canProceedToPrep = GameManager.Instance.CheckIfEnoughPlayers();
        
#if UNITY_EDITOR
        GameManager.Instance.PrintDebug("GameManager - Check if can change to prep state: ", canProceedToPrep);
#endif
        
        if (canProceedToPrep)
            GameManager.Instance.ChangeState(GameManager.State.PrepareRound);
    }
    
}
