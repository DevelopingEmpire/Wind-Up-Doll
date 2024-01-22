using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using TMPro;

namespace Ro.FusionBites
{
    public class FusionConnection : MonoBehaviour, INetworkRunnerCallbacks
    {
        public static FusionConnection instance;

        [HideInInspector] public NetworkRunner runner;

        [SerializeField] NetworkObject playerPrefab;

        public string _playerName = null;

        [Header("Session Creation")]
        public TMP_InputField sessionNameInput;
        public TMP_InputField sessionPasswordInput;
        public GameObject createSessionCanvas;

        [Header("Session Joining")]
        public GameObject invalidText;
        public GameObject joinSessionCanvas;
        public TMP_InputField joinSessionCanvasInput;
        public string currentAttemptSessionKey;
        public string currentAttemptSessionName;


        [Header("Session List")]
        public GameObject roomListCanvas;
        private List<SessionInfo> _sessions = new List<SessionInfo>();
        public Button refreshButton;
        public Transform sessionListContent;
        public GameObject sessionEntryPrefab;
        private bool firstLobby = false;

        private void Awake()
        {
            if(instance == null) { instance = this; }
            
        }



        public void ConnectToLobby(string playerName)
        {
            roomListCanvas.SetActive(true);

            _playerName = playerName;

            if(runner == null)
            {
                runner = gameObject.AddComponent<NetworkRunner>();
            }

            runner.JoinSessionLobby(SessionLobby.Shared);
        }


        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
        {
            _sessions.Clear();
            _sessions = sessionList;

            if(firstLobby == false)
            {
                RefreshSessionListUI();
                firstLobby = true;
            }
        }

        public void RefreshSessionListUI()
        {
            foreach(Transform child in sessionListContent)
            {
                Destroy(child.gameObject);
            }

            foreach(SessionInfo session in _sessions)
            {
                if (session.IsVisible)
                {
                    GameObject entry = GameObject.Instantiate(sessionEntryPrefab, sessionListContent);
                    SessionEntryPrefab script = entry.GetComponent<SessionEntryPrefab>();
                    script.sessionName.text = session.Name;
                    script.playerCount.text = session.PlayerCount + "/" + session.MaxPlayers;

                    script.sessionKey = session.Properties.GetValueOrDefault("sessionKey").PropertyValue as string;

                    if (session.IsOpen == false || session.PlayerCount >= session.MaxPlayers)
                    {
                        script.joinButton.interactable = false;
                    }
                    else
                    {
                        script.joinButton.interactable = true;
                    }
                }
            }
        }

        public void ConnectToSession(string sessionName, string sessionKey)
        {
            currentAttemptSessionKey = sessionKey;
            currentAttemptSessionName = sessionName;

            joinSessionCanvas.SetActive(true);
            
        }

        public async void FinishConnectionToSession()
        {
            if(currentAttemptSessionKey == joinSessionCanvasInput.text)
            {
                roomListCanvas.SetActive(false);
                joinSessionCanvas.SetActive(false);

                if (runner == null)
                {
                    runner = gameObject.AddComponent<NetworkRunner>();
                }

                await runner.StartGame(new StartGameArgs
                {
                    GameMode = GameMode.Shared,
                    SessionName = currentAttemptSessionName,
                    //Scene = 3,
                    //SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
                });
            }
            else
            {
                invalidText.SetActive(true);
            }
        }

        public async void CreateSession()
        {
            string sessionName = sessionNameInput.text;
            string sessionKey = sessionPasswordInput.text;

            SessionProperty key = sessionKey;
            Dictionary<string, SessionProperty> sessionProperties = new Dictionary<string, SessionProperty>();
            sessionProperties.Add("sessionKey", key);

            
            roomListCanvas.SetActive(true);
            createSessionCanvas.SetActive(true);

            //int randomInt = UnityEngine.Random.Range(1000, 9999);
            //string randomSessionName = "Room-" + randomInt.ToString();

            if (runner == null)
            {
                runner = gameObject.AddComponent<NetworkRunner>();
            }

            await runner.StartGame(new StartGameArgs
            {
                GameMode = GameMode.Shared,
                SessionName = sessionName,
                SessionProperties = sessionProperties,
                //Scene = 3,
                PlayerCount = 2,
                //SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
            });
        }

        public void OnConnectedToServer(NetworkRunner runner)
        {
            Debug.Log("OnConnectToServer");
            NetworkObject playerObject = runner.Spawn(playerPrefab , Vector3.zero); // 위치 설정 가능. 오버라이드 많다

            runner.SetPlayerObject(runner.LocalPlayer, playerObject);
        }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            Debug.Log("OnPlayerJoined");
        }

        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
        {
        }

        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
        {
        }

        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
        {
        }

        public void OnDisconnectedFromServer(NetworkRunner runner)
        {
        }

        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
        {
        }

        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
        }

        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
        {
        }

       

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
        }

        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
        {
        }

        public void OnSceneLoadDone(NetworkRunner runner)
        {
        }

        public void OnSceneLoadStart(NetworkRunner runner)
        {
        }

       
        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {
        }

        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
        {
        }


    }
}
