using Alteruna;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] private GameObject _menu;
    [SerializeField] private GameObject _lobby;
    [SerializeField] private GameObject _alteruna;
    [SerializeField] private Button _startGameButton;

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

    public void LeaveRoom()
    {
        Multiplayer.Instance.CurrentRoom.Leave();
    }

    public void CanStart()
    {
        _startGameButton.interactable = true;
    }
}
