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

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }

    private void Start()
    {
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

    public void CanStart(bool canStart)
    {
        _startGameButton.interactable = canStart;
    }

    public void OnPlayerJoinedRoomLocal()
    {
        if (Multiplayer.Instance.CurrentRoom.Users.Count == 0)
            return;
        
        ProcedureParameters parameters = new ProcedureParameters();
        Multiplayer.InvokeRemoteProcedure("UpdateLobbyUiRemote", UserId.AllInclusive, parameters);
    }
    
    public void UpdateLobbyUiRemote(ushort fromUser, ProcedureParameters parameters, uint callId, ITransportStreamReader processor)
    {
        foreach (var user in GameManager.Instance.GetUserListInRoom())
        {
            _playerPanels[user.Index].GetComponentInChildren<TextMeshProUGUI>().text = user.Name;
            _playerPanels[user.Index].SetActive(true);
        }
    }
}
