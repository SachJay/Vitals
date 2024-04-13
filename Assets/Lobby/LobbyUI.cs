using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI lobbyText;
    [SerializeField] private Button joinLobbyButton;

    private string lobbyName;

    public void Init(string lobbyName)
    {
        this.lobbyName = lobbyName;
        lobbyText.text = lobbyName;

        joinLobbyButton.onClick.RemoveAllListeners();
        joinLobbyButton.onClick.AddListener(() => {
            JoinLobby();
        });
    }

    // TODO: Add code to join the lobby here
    private void JoinLobby()
    {
        Debug.Log($"Joining lobby: {lobbyName}");

        // Change the scene to Lobby Scene
        SceneChanger.ChangeSceneTo(Scenes.Lobby);
    }
}
