using System.Collections.Generic;
using UnityEngine;

public class LobbyFinder : MonoBehaviour
{
    public static LobbyFinder Instance { get; private set; }

    private const int MAX_LOBBIES = 100;

    [SerializeField] private bool isDebugOn = false;
    [SerializeField] private LobbyUI lobbyUIPrefab;
    [SerializeField] private Transform lobbyContainerTF;

    private LobbyUI[] lobbies;

    private void Awake()
    {
        Instance = this;

        lobbies = new LobbyUI[MAX_LOBBIES];
        for (int i = 0; i < MAX_LOBBIES; i++)
        {
            LobbyUI lobbyObject = Instantiate(lobbyUIPrefab, lobbyContainerTF);
            lobbies[i] = lobbyObject;
            lobbies[i].gameObject.SetActive(false);
        }    
    }

    private void Start()
    {
        if (!isDebugOn)
            return;

        List<string> testLobbies = new()
        {
            "One", "Two", "Three"
        };
        UpdateLobbies(testLobbies);
    }

    public void UpdateLobbies(List<string> updatedLobbies)
    {
        DisableLobbies();

        for (int i = 0; i < updatedLobbies.Count; i++)
        {
            lobbies[i].gameObject.SetActive(true);
            lobbies[i].Init(updatedLobbies[i]);
        }
    }

    private void DisableLobbies()
    {
        foreach (LobbyUI lobbyUI in lobbies)
            lobbyUI.gameObject.SetActive(false);
    }
}
