using System;
using System.Collections.Generic;
using Alteruna;
using Alteruna.Trinity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Application = UnityEngine.Application;

public class UiManager : AttributesSync
{
    [SerializeField] private Button _startGameButton;
    [SerializeField] private TextMeshProUGUI _lobbyHeader;

    [SerializeField] private GameObject[] _playerPanels;

    public List<string> playerNames;
    public string playerName;
    
    private static UiManager _instance;
    
    public static UiManager Instance
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

    private void Start()
    {
        Multiplayer.RegisterRemoteProcedure("UpdateNameListRemote", UpdateNameListRemote);
        Multiplayer.RegisterRemoteProcedure("UpdateLobbyUiRemote", UpdateLobbyUiRemote);
    }

    public void ButtonSetActive(Button button)
    {
        button.interactable = true;
    }
    public void ButtonSetInaAtive(Button button)
    {
        button.interactable = false;
    }

    public void ShowPanel(GameObject panel)
    {
        panel.SetActive(true);
    }

    public void HidePanel(GameObject panel)
    {
        panel.SetActive(false);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    public void JoinRoom()
    {
        var name = Alteruna.Multiplayer.Instance.CurrentRoom.Name;
        _lobbyHeader.text = name;
    }
    
    public void LeaveRoom()
    {
        Multiplayer.Instance.CurrentRoom.Leave();
    }

    public void CanStart()
    {
        _startGameButton.interactable = true;
    }

    public void OnPlayerJoinedRoomLocal()
    {
        // Generate Name
        UInt16 myIndex = Alteruna.Multiplayer.Instance.Me.Index;
        playerName = Alteruna.NameGenerator.GenerateStatic();
        playerNames[myIndex] = playerName;
        
        ProcedureParameters parameters = new ProcedureParameters();
        parameters.Set("fromPlayerIndex", myIndex);
        parameters.Set("otherPlayerName", playerName);
        
        Multiplayer.InvokeRemoteProcedure("UpdateNameListRemote", UserId.All, parameters);
        Multiplayer.InvokeRemoteProcedure("UpdateLobbyUiRemote", UserId.All, parameters);
    }

    public void UpdateNameListRemote(ushort fromUser, ProcedureParameters parameters, uint callId, ITransportStreamReader processor)
    {
        UInt16 fromPlayerIndex = parameters.Get("fromPlayerIndex", (UInt16)0);
        string otherPlayerName = parameters.Get("otherPlayerName", "");
        playerNames[fromPlayerIndex] = otherPlayerName;
    }
    
    public void UpdateLobbyUiRemote(ushort fromUser, ProcedureParameters parameters, uint callId, ITransportStreamReader processor)
    {
        for (int i = 0; i <  Multiplayer.Instance.CurrentRoom.Users.Count; i++)
        {
            _playerPanels[i].GetComponentInChildren<TextMeshProUGUI>().text = playerNames[i];
            _playerPanels[i].SetActive(true);
        }
    }
}
