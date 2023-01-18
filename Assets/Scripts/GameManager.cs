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
    

    public void CheckIfEnoughPlayers() 
    {
#if UNITY_EDITOR
        var possibleToStart = _users.Count >= _minUsersToStart;
            PrintDebug("GameManager - Check if can start round: ", possibleToStart);
#endif
        
        if (_users.Count >= _minUsersToStart)
            ChangeState(State.StartRound);
        else
            ChangeState(State.LookingForPlayers);
    }

    // CHANGING STATES
    public void ChangeState(byte stateIndex)
    {
        FinalizeStateChange((State)stateIndex);
    }
    public void ChangeState(State state)
    {
        FinalizeStateChange(state);
    }
    private void FinalizeStateChange(State state)
    {
        if (_state == state)
            return;
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
