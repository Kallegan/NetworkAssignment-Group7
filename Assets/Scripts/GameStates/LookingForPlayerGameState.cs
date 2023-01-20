using Alteruna;
using UnityEngine;

public class LookingForPlayerGameState : GameState
{
    public LookingForPlayerGameState(GameManager gameManager)
    {
        _gameManager = gameManager;
        _multiplayer = Multiplayer.Instance;
        _minPlayersRequired = _gameManager._minUsersToStart;
    }
    
    private readonly GameManager _gameManager;
    private readonly Multiplayer _multiplayer;
    private int _minPlayersRequired;

    private const float DelayBetweenChecksSeconds = 2f;
    private float _nextCheck;
    
    // ReSharper disable Unity.PerformanceAnalysis
    public override void Update()
    {
        //Debug.Log(_multiplayer);
        if (_nextCheck <= 0)
        {
            CheckIfEnoughPlayers();
            _nextCheck = DelayBetweenChecksSeconds;
            return;
        }

        _nextCheck -= Time.deltaTime;

    }

    public override void Run()
    {
        
    }

    private void CheckIfEnoughPlayers()
    {
        if (!_multiplayer.InRoom)
        {
            _gameManager.ChangeState(GameManager.State.Idle);
            return;
        }

        var amountOfPlayersInRoom = _gameManager.AmountOfPlayersInRoom();
        
#if UNITY_EDITOR
        var possibleToStart = amountOfPlayersInRoom >= _minPlayersRequired;
        _gameManager.PrintDebug("GameManager - Check if can start round: ", possibleToStart);
#endif
        
        if (amountOfPlayersInRoom >= _minPlayersRequired)
            _gameManager.ChangeState(GameManager.State.PrepareRound);
    }
    
}
