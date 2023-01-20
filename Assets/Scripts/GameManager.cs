using System;
using System.Collections;
using System.Collections.Generic;
using Alteruna;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using Avatar = Alteruna.Avatar;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool _showDebugLogs = true;
    [SerializeField] private Multiplayer _multiplayer;

    [Header("Game Settings")]
    [SerializeField] public int _minUsersToStart = 2;

    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                throw new NullReferenceException("_instance is null!");
            }
            return _instance;
        }
    }
    
    private List<User> _users;

    private State _state;
    public enum State
    {
        Idle,
        LookingForPlayers,
        PrepareRound,
        StartRound,
        FinishRound,
        Restart
    }

    private GameState _currentState;
    private IdleGameState _idleGameState;
    private LookingForPlayerGameState _lookingForPlayerGameState;
    private PrepareRoundGameState _prepareRoundGameState;
    private StartRoundGameState _startRoundGameState;
    private FinishRoundGameState _finishRoundGameState;
    private void Awake()
    {
        
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        
        _idleGameState = new IdleGameState();
        _lookingForPlayerGameState = new LookingForPlayerGameState(_instance);
        _prepareRoundGameState = new PrepareRoundGameState();
        _startRoundGameState = new StartRoundGameState();
        _finishRoundGameState = new FinishRoundGameState();
        
        _currentState = _idleGameState;
        ChangeState(State.Idle);
    }

    void Start()
    {
        
    }
    
    void Update()
    {
        _currentState.Update();
    }

    public void UpdateUsersInRoomList()
    {
        _users = _multiplayer.GetUsers();

#if UNITY_EDITOR
        PrintDebug("GameManager - Users in room: ", _users.Count);
#endif
    }

    public int AmountOfPlayersInRoom()
    {
        UpdateUsersInRoomList();
        int amount = _users.Count;
        return amount;
    }
    
    public void JoinedRoom()
    {
        if (_state == State.Idle)
            ChangeState(State.LookingForPlayers);
        
#if UNITY_EDITOR
        PrintDebug("GameManager - Joined room: ", _multiplayer.CurrentRoom.Name);
#endif
    }
    
    public void OtherJoinedRoom()
    {
#if UNITY_EDITOR
        PrintDebug("GameManager - ", "Other player joined the room.");
#endif
    }
    
    public void LeftRoom()
    {
        ChangeState(State.Idle);
#if UNITY_EDITOR
        PrintDebug("GameManager - ", "Left room.");
#endif
    }
    
    public void OtherLeftRoom()
    {
#if UNITY_EDITOR
        PrintDebug("GameManager - ", "Other player left the room.");
#endif
    }

    // CHANGING STATES
    public void ChangeState(byte stateIndex)
    {
        AssignState((State)stateIndex);
    }
    public void ChangeState(State state)
    {
        AssignState(state);
    }
    private void AssignState(State state)
    {
        if (_state == state)
            return;
        
        _state = state;

        _currentState = _state switch
        {
            State.Idle => _idleGameState,
            State.LookingForPlayers => _lookingForPlayerGameState,
            State.PrepareRound => _prepareRoundGameState,
            State.StartRound => _startRoundGameState,
            State.FinishRound => _finishRoundGameState,
            State.Restart => _prepareRoundGameState,
            _ => _currentState
        };
        
        if (_multiplayer.GetAvatar(_multiplayer.Me.Index).GameObject() != null)
        {
            PlayerGameStateSync playerGameStateSync = _multiplayer.GetAvatar(_multiplayer.Me.Index).GameObject().GetComponentInChildren<PlayerGameStateSync>();
            playerGameStateSync.currentGameState = (byte)_state;
        }
        
        // if (_multiplayer.GetAvatar(_multiplayer.Me.Index).GameObject().TryGetComponent<PlayerGameStateSync>(out var playerGameStateSync))
        // {
        //     Debug.Log("yes");
        //     playerGameStateSync.currentGameState = (byte)_state;
        //     Debug.Log("yes");
        // }
        
#if UNITY_EDITOR
        PrintDebug("GameManager - State Changed to: ", _state);
#endif
    }
    
    // DEBUG PRINT
#if UNITY_EDITOR
    public void PrintDebug<T> (string text, T debugData)
    {
        if (_showDebugLogs)
            Debug.Log("<color=olive>" + text + "</color><color=teal>" + debugData.ToString() + "</color>");
    }
#endif
}
