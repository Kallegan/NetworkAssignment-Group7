using System;
using Alteruna;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] private Button _startGameButton;
    [SerializeField] private TextMeshProUGUI _lobbyHeader;

    [SerializeField] private GameObject[] _playerPanels;
    
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

    public void OnPlayerJoinedRoom()
    {
        var playerIndex = Alteruna.Multiplayer.Instance.Me.Index;
        var name = Alteruna.NameGenerator.GenerateStatic();
        _playerPanels[playerIndex].GetComponentInChildren<TextMeshProUGUI>().text = name;
        _playerPanels[playerIndex].SetActive(true);
    }
}
