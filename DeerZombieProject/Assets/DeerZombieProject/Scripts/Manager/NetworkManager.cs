using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

using System;
using UnityEngine.SceneManagement;

namespace DeerZombieProject
{
    public class NetworkManager : MonoBehaviourPunCallbacks
    {
        #region Constant Fields
        #endregion

        #region Static Fields
        private static NetworkManager instance;
        public static NetworkManager Instance
        {
            get { return instance; }
        }
        #endregion

        #region Fields
        public SString CURRENT_HEALTH_KEY;
        public SString MAX_HEALTH_KEY;
        public SString CURRENT_KILLS_KEY;
        public SString CURRENT_SCORE_KEY;
        public SString LEVEL_ID_KEY;
        private int levelBuildIndex = 3;
        #endregion

        #region Events and Delegates
        public Action<int> OnMaxHealthChanged;
        public Action<int> OnCurrentHealthChanged;
        public Action<int> OnScoreChanged;
        public Action<int> OnKillsChanged;
        #endregion

        #region Callbacks

        public override void OnJoinedRoom()
        {
            PreparePlayerCustomProperties();
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
        {
            if(targetPlayer != PhotonNetwork.LocalPlayer)
            {
                return;
            }

            if (changedProps.ContainsKey(CURRENT_HEALTH_KEY.value))
            {
                OnCurrentHealthChanged?.Invoke((int)changedProps[CURRENT_HEALTH_KEY.value]);
            }
            if (changedProps.ContainsKey(MAX_HEALTH_KEY.value))
            {
                OnMaxHealthChanged?.Invoke((int)changedProps[MAX_HEALTH_KEY.value]);
            }
            if (changedProps.ContainsKey(CURRENT_SCORE_KEY.value))
            {
                OnScoreChanged?.Invoke((int)changedProps[CURRENT_SCORE_KEY.value]);
            }
            if (changedProps.ContainsKey(CURRENT_KILLS_KEY.value))
            {
                OnKillsChanged?.Invoke((int)changedProps[CURRENT_KILLS_KEY.value]);
            }
        }

        public override void OnConnectedToMaster()
        {
            TypedLobby typedLobby = new TypedLobby("defaultLobby", LobbyType.Default);
            PhotonNetwork.JoinLobby(typedLobby);
        }
        #endregion

        #region Constructors

        #endregion

        #region LifeCycle Methods

        private void Awake()
        {
            if(instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            Init();
            DontDestroyOnLoad(gameObject);
        }

        #endregion

        #region Public Methods
        public void ConnectToMaster(AuthenticationValues authValues)
        {
            PhotonNetwork.AuthValues = authValues;
            PhotonNetwork.ConnectUsingSettings();
        }

        public void SetPlayerName(string name)
        {
            PhotonNetwork.LocalPlayer.NickName = name;
        }

        public void CreateRoom()
        {
            RoomOptions options = new RoomOptions();
            string roomName = "Room " + (PhotonNetwork.CountOfRooms + 1).ToString();

            options.MaxPlayers = 4;
            options.IsVisible = true;
            options.IsOpen = true;
            options.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable();
            options.CustomRoomProperties.Add(LEVEL_ID_KEY.value, 1);

            PhotonNetwork.CreateRoom(roomName, options);
        }

        public void JoinQuickGame()
        {
            RoomOptions options = new RoomOptions();
            string roomName = "Room " + (PhotonNetwork.CountOfRooms + 1).ToString();

            options.MaxPlayers = 4;
            options.IsVisible = true;
            options.IsOpen = true;
            options.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable();
            options.CustomRoomProperties.Add(LEVEL_ID_KEY.value, 1);

            PhotonNetwork.JoinRandomOrCreateRoom(roomOptions: options, roomName: roomName);
        }

        public void LeaveRoom()
        {
            if(PhotonNetwork.CurrentRoom == null)
            {
                return;
            }

            PhotonNetwork.LeaveRoom();
        }

        public void StartGame()
        {
            if (!PhotonNetwork.IsMasterClient) { return; }

            SceneManager.LoadScene(levelBuildIndex);
        }

        public void Init()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.LocalPlayer.CustomProperties = new ExitGames.Client.Photon.Hashtable();
            PreparePlayerCustomProperties();
        }
        #endregion

        #region Internal Methods

        #endregion

        #region Protected Methods

        #endregion

        #region Private Methods
        private void PreparePlayerCustomProperties()
        {
            ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable();

            customProperties.Add(CURRENT_HEALTH_KEY.value, 10);
            customProperties.Add(MAX_HEALTH_KEY.value, 10);
            customProperties.Add(CURRENT_KILLS_KEY.value, 0);
            customProperties.Add(CURRENT_SCORE_KEY.value, 0);

            PhotonNetwork.LocalPlayer.SetCustomProperties(customProperties);
        }
        #endregion

        #region Nested Types

        #endregion
    }
}
