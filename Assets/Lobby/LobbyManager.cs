using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    public static LobbyManager Instance { get; private set; }

    [SerializeField] private GameObject playerInfoPrefab;
    [SerializeField] private Transform playerContainerTF;

    private readonly List<GameObject> players = new();

    private void Awake()
    {
        Instance = this;
    }

    public void AddPlayer(GameObject player, string playerName = "")
    {
        players.Add(player);
        GameObject playerObject = Instantiate(playerInfoPrefab, playerContainerTF);
        TextMeshProUGUI playerText = playerObject.GetComponentInChildren<TextMeshProUGUI>();
        playerText.text = playerName != ""
            ? playerName 
            : $"Player {playerObject.transform.GetSiblingIndex()}";
    }
}
