using System.Collections.Generic;
using Alteruna;
using Unity.VisualScripting;
using UnityEngine;
using Avatar = UnityEngine.Avatar;

public class PrepareRoundGameState : GameState
{
    private const float DelayBetweenChecksSeconds = 2f;
    private float _nextCheck = DelayBetweenChecksSeconds;
    
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
        if (GameManager.Instance.CheckIfEnoughPlayers())
        {
            if (Multiplayer.Instance.Me.Index != 0)
                return;
            TryToStart();
        }
        else
            GameManager.Instance.ChangeState(GameManager.State.LookingForPlayers);
    }

    private void TryToStart()
    {
#if UNITY_EDITOR
        GameManager.Instance.PrintDebug("GameManager - ", "TRYING TO START");
#endif
        bool canStart = true;
        List<User> users = GameManager.Instance.Users;
        foreach (var user in users)
        {
            ushort index = user.Index;
            GameObject player = GameObject.FindGameObjectWithTag("Player"); //Multiplayer.Instance.GetAvatar(index).gameObject;
            PlayerStateSync playerStateSync = player.GetComponentInChildren<PlayerStateSync>();
            if (playerStateSync.currentGameState != (byte)GameManager.State.PrepareRound)
                canStart = false;
        }
        if (canStart)
        {
            GameManager.Instance.CallChangeMyState(GameManager.State.StartRound);
        }
        else
        {
#if UNITY_EDITOR
            GameManager.Instance.PrintDebug("GameManager - ", "UNABLE TO START (PEOPLE NOT SYNCED).");
#endif
        }
    }

}