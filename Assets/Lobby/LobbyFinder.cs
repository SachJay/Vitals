using UnityEngine;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;
using Steamworks.Data;

public class LobbyFinder : MonoBehaviour
{
    public static LobbyFinder Instance { get; private set; }

    private const int MAX_LOBBIES = 10;

    [SerializeField] private bool isDebugOn = false;
    [SerializeField] private LobbyUI lobbyUIPrefab;
    [SerializeField] private Transform lobbyContainerTF;
    [SerializeField] MultiplayerManager multiplayerManager;
    [SerializeField] private Button refreshButton;

    public SteamManager steamManager = null;

    public LobbyUI[] lobbies;

    List<string> steamLobbyNames = new(){};

    private void Awake()
    {
        steamManager = GameObject.Find("SteamManager").GetComponent<SteamManager>(); //FIXME potentially slow
        Instance = this;

        lobbies = new LobbyUI[MAX_LOBBIES];
        for (int i = 0; i < MAX_LOBBIES; i++)
        {
            LobbyUI lobbyObject = Instantiate(lobbyUIPrefab, lobbyContainerTF);
            lobbies[i] = lobbyObject;
            lobbies[i].gameObject.SetActive(false);
        }

        setupRefreshButtonListener();
    }

    private void Start()
    {
        if (!isDebugOn)
            return;
    }

    private void setupRefreshButtonListener() //FIXME move this to a button function class?
    {
        refreshButton.onClick.AddListener(async () =>
        {
            steamLobbyNames = await multiplayerManager.RefreshLobby();
            UpdateLobbies();
        });
    }

    public void UpdateLobbies()
    {

        print("Lobby Count: " + steamLobbyNames.Count);

        DisableLobbies();

        for (int i = 0; i < steamLobbyNames.Count; i++)
        {
            if (lobbies[i] != null && lobbies[i].gameObject != null)
            {
                lobbies[i].gameObject.SetActive(true);
            }
            lobbies[i].Init(steamLobbyNames[i]);
        }
    }

    private void DisableLobbies()
    {
        foreach (LobbyUI lobbyUI in lobbies)
        {
            if (lobbyUI != null && lobbyUI.gameObject != null)
            {
                lobbyUI.gameObject.SetActive(false);
            }
        }
    }
}
