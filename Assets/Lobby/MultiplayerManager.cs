using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Steamworks;
using Steamworks.Data;
using System.Threading.Tasks;
using System.Collections.Generic;

public class MultiplayerManager : MonoBehaviour
{
    public SteamManager steamManager = null;
    [SerializeField] private Button newLobbyButton;

    private void Awake()
    {
        steamManager = GameObject.Find("SteamManager").GetComponent<SteamManager>(); //FIXME potentially slow

        setupNewLobbyButtonListener();
    }

    private void setupNewLobbyButtonListener() //FIXME move this to a button function class?
    {
        newLobbyButton.onClick.AddListener(() => CreateNewLobby());
    }

    public async void CreateNewLobby()
    {
        bool result = await steamManager.CreateLobby(0);

        if (result)
        {
            print("succeed to create lobby");
        }
        else
        {
            print("failed to create lobby");
        }
    }

    public async Task<List<string>> RefreshLobby()
    {
        bool apiResult = await steamManager.RefreshMultiplayerLobbies();
        if (!apiResult)
        {
            print("failed to refresh lobbies");
        }

        List<Lobby> lobbyList = steamManager.activeUnrankedLobbies;
        List<string> steamLobbies = new List<string>(){};

        lobbyList.ForEach(x =>
        {
            foreach (KeyValuePair<string, string> keyPair in x.Data)
            {

                if (keyPair.Key.Equals("lobby_name") && !steamLobbies.Contains(keyPair.Value))
                {
                    steamLobbies.Add(keyPair.Value);
                    //print("New Lobby Created: " + keyPair.Value);
                }
                else
                {
                    print("Invalid key: " + keyPair.Key);
                }

            }
        });

        return steamLobbies;
    }
}
