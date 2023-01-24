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
        playerNames = new List<string>();
        for (int i = 0; i < 10; i++)
            playerNames.Insert(i, "");

        playerName = Alteruna.NameGenerator.GenerateStatic();
        Multiplayer.Instance.SetUsername(playerName);
        
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

    public void SetLobbyName()
    {
        var name =  Alteruna.Multiplayer.Instance.CurrentRoom.Name;
        _lobbyHeader.text = "Room: " + name;
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
        UInt16 myIndex = Alteruna.Multiplayer.Instance.Me.Index;
        playerNames.Insert(myIndex, playerName);
        
        // Create RPC-params 
        ProcedureParameters parameters = new ProcedureParameters();
        parameters.Set("fromPlayerIndex", myIndex);
        parameters.Set("otherPlayerName", playerName);
        
        if (Multiplayer.Instance.CurrentRoom.Users.Count == 0)
            return;
        
        //Multiplayer.InvokeRemoteProcedure("UpdateNameListRemote", UserId.AllInclusive, parameters);
        Multiplayer.InvokeRemoteProcedure("UpdateLobbyUiRemote", UserId.AllInclusive, parameters);
    }

    public void UpdateLobbyUiLocal()
    {
        UInt16 myIndex = Alteruna.Multiplayer.Instance.Me.Index;
        _playerPanels[myIndex].GetComponentInChildren<TextMeshProUGUI>().text = playerName;
        _playerPanels[myIndex].SetActive(true);
    }
    
    public void UpdateNameListRemote(ushort fromUser, ProcedureParameters parameters, uint callId, ITransportStreamReader processor)
    {
        /*
        UInt16 fromPlayerIndex = parameters.Get("fromPlayerIndex", (UInt16)0);
        string otherPlayerName = parameters.Get("otherPlayerName", "");
        playerNames.Insert(fromPlayerIndex, otherPlayerName);
        */
        foreach (var user in GameManager.Instance.GetUserListInRoom())
        {
            playerNames.Insert(user.Index, user.Name);
        }
    }
    
    public void UpdateLobbyUiRemote(ushort fromUser, ProcedureParameters parameters, uint callId, ITransportStreamReader processor)
    {
        foreach (var user in GameManager.Instance.GetUserListInRoom())
        {
            _playerPanels[user.Index].GetComponentInChildren<TextMeshProUGUI>().text = user.Name;
            _playerPanels[user.Index].SetActive(true);
        }

        /*
        
        for (int i = 0; i <  Multiplayer.Instance.CurrentRoom.Users.Count; i++)
        {
            _playerPanels[i].GetComponentInChildren<TextMeshProUGUI>().text = playerNames[i];
            _playerPanels[i].SetActive(true);
        }
        */
    }
}
