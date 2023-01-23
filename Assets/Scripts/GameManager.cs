using System;
using System.Collections;
using System.Collections.Generic;
using Alteruna;
using Alteruna.Trinity;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using Avatar = Alteruna.Avatar;

public class GameManager : Synchronizable
{
    [SerializeField] private bool _showDebugLogs = true;

    [Header("Game Settings")]
    [SerializeField] public int _minUsersToStart = 2;

    private Multiplayer _multiplayer;
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
    public List<User> Users {get => _users;}

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
    private readonly IdleGameState _idleGameState = new IdleGameState();
    private readonly LookingForPlayerGameState _lookingForPlayerGameState = new LookingForPlayerGameState();
    private readonly PrepareRoundGameState _prepareRoundGameState = new PrepareRoundGameState();
    private readonly StartRoundGameState _startRoundGameState = new StartRoundGameState();
    private readonly FinishRoundGameState _finishRoundGameState = new FinishRoundGameState();
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        _multiplayer = Multiplayer.Instance;
    }
    
   
    
    void Start()
    {
        Multiplayer.RegisterRemoteProcedure("ChangeMyStateProcedure", ChangeMyStateProcedure);
        _currentState = _idleGameState;
        ChangeState(State.Idle);
    }
    
    public void ChangeMyStateProcedure(ushort fromUser, ProcedureParameters parameters, uint callId, ITransportStreamReader processor)
    {
#if UNITY_EDITOR
        PrintDebug("GameManager - ", "RPC TO CHANGE STATE");
#endif
        int stateIndex = parameters.Get("stateIndex", 0);
        ChangeState((byte)stateIndex);
    }
    
    public void CallChangeMyState(State state)
    {
        ProcedureParameters parameters = new ProcedureParameters();
        parameters.Set("stateIndex", (int)state);
        Multiplayer.InvokeRemoteProcedure("ChangeMyStateProcedure", UserId.All, parameters);
        ChangeState((byte)state);
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
    
    public bool CheckIfEnoughPlayers()
    {
        int amountOfPlayersInRoom = AmountOfPlayersInRoom();
        bool enoughPlayers = amountOfPlayersInRoom >= _minUsersToStart;
        
#if UNITY_EDITOR
        PrintDebug("GameManager - Check if enough players: ", enoughPlayers);
#endif

        return enoughPlayers;
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
        SyncTheStates();
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
        _currentState.Run();

        UpdateSyncState();

#if UNITY_EDITOR
        PrintDebug("GameManager - State Changed to: ", _state);
#endif
    }

    public void UpdateSyncState()
    {
        Avatar avatar = _multiplayer.GetAvatar();
        if (avatar != null)
        {
            GameObject player = avatar.gameObject;
            PlayerStateSync playerStateSync = player.GetComponentInChildren<PlayerStateSync>();
                playerStateSync.currentGameState = (byte)_state;
        }
    }

    private void SyncTheStates()
    {
        Avatar avatar = _multiplayer.GetAvatar();
        if (avatar != null)
        {
            GameObject player = avatar.GameObject();
            if (player != null)
            {
                PlayerStateSync playerStateSync = player.GetComponentInChildren<PlayerStateSync>();
                playerStateSync.SyncMyState();
            }
        }
    }
    
    // DEBUG PRINT
#if UNITY_EDITOR
    public void PrintDebug<T> (string text, T debugData)
    {
        if (_showDebugLogs)
            Debug.Log("<color=olive>" + text + "</color><color=teal>" + debugData.ToString() + "</color>");
    }
#endif
    public override void AssembleData(Writer writer, byte LOD = 100)
    {
  
    }

    public override void DisassembleData(Reader reader, byte LOD = 100)
    {

    }
}
