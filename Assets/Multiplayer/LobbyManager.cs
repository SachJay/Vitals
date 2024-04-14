using Steamworks;
using Steamworks.Data;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    [SerializeField]
    SteamManager steamManager;
    bool isJoined = false;

    void Start()
    {
        asyncCreateLobby();

    }

    async Task asyncCreateLobby()
    {
        bool result = await steamManager.CreateLobby(0);

        if (result)
        {
            print("failed to create lobby");
        } else
        {
            print("succeed to create lobby");
        }

        InvokeRepeating("refreshLobbyAsync", 0, 2.0f);
    }

    private async Task refreshLobbyAsync()
    {
        bool apiResult = await steamManager.RefreshMultiplayerLobbies();

        displayLobbies();
        List<Lobby> lobbyList = GetLobbies();
        if (lobbyList.Count > 0)
        {
            RoomEnter room = await lobbyList[0].Join();
            print("Enters room: " + room);
        } 
    }

    private void testFlow()
    {
        displayLobbies();
        if (!isJoined && steamManager.activeUnrankedLobbies.Count > 0)
        {
            joinLobbyAsync();
        }
    }

    public List<Lobby> GetLobbies()
    {
        return steamManager.activeUnrankedLobbies;
    }

    private void displayLobbies()
    {
        GetLobbies().ForEach(x =>
        {
            print("Lobby Found");
            foreach(KeyValuePair<string, string> keyPair in x.Data)
            {
                print(keyPair.Key + "  " + keyPair.Value);
            }
        });
    }

    private async Task joinLobbyAsync()
    {
        RoomEnter room = await steamManager.activeUnrankedLobbies[0].Join();
        isJoined = true;
        print(room);
    }
}
