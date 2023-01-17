using System;
using System.Collections;
using System.Collections.Generic;
using Alteruna;
using JetBrains.Annotations;
using UnityEngine;
using Avatar = Alteruna.Avatar;

public class GameManager : Synchronizable
{
    [SerializeField] private bool _showDebugLogs = true;
    [SerializeField] private Multiplayer _multiplayer;
    
    [Header("Game Settings")]
    [SerializeField] private int _minUsersToStart = 2;
        
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

    private void Awake()
    {
        _instance = this;
        ChangeState(State.Idle);
    }

    void Start()
    {
    }
    
    void Update()
    {
        if (_state == State.Idle)
            return;
    }
    
    public override void AssembleData(Writer writer, byte LOD = 100)
    {
    }

    public override void DisassembleData(Reader reader, byte LOD = 100)
    {
    }
    
    public void UpdateUsersInRoomList()
    {
        _users = _multiplayer.GetUsers();
        
#if UNITY_EDITOR
        PrintDebug("GameManager - Users in room: ", _users.Count);
#endif
        
    }

    public void JoinedRoom()
    {
        if (_state == State.Idle)
            ChangeState(State.Looking);
        
#if UNITY_EDITOR
        PrintDebug("GameManager - Join room: ", _multiplayer.CurrentRoom.Name);
#endif
        
        CheckIfCanStartGame();
    }

    public void CheckIfCanStartGame() 
    {
        UpdateUsersInRoomList();

#if UNITY_EDITOR
        var possibleToStart = _users.Count >= _minUsersToStart;
            PrintDebug("GameManager - Check if can start round: ", possibleToStart);
#endif
        
        if (_users.Count >= _minUsersToStart)
            ChangeState(State.Start);
    }

    // CHANGING STATES
    public void ChangeState(short stateIndex)
    {
        FinalizeStateChange((State)stateIndex);
    }
    public void ChangeState(State state)
    {
        FinalizeStateChange(state);
    }
    private void FinalizeStateChange(State state)
    {
        _state = state;
        
#if UNITY_EDITOR
        PrintDebug("GameManager - State Changed to: ", _state);
#endif
    }
    
    // DEBUG PRINT
#if UNITY_EDITOR
    private void PrintDebug<T> (string text, T debugData)
    {
        if (_showDebugLogs);
            Debug.Log("<color=olive>" + text + "</color><color=teal>" + debugData.ToString() + "</color>");
    }
#endif
}
