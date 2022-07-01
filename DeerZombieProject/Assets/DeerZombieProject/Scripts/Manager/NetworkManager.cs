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
        private int levelBuildIndex = 3;
        #endregion

        #region Events and Delegates
        #endregion

        #region Callbacks

        public override void OnJoinedRoom()
        {
            PreparePlayerCustomProperties();
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
            PhotonNetwork.AutomaticallySyncScene = true;
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
            options.CustomRoomProperties.Add("levelId", 1);

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
            options.CustomRoomProperties.Add("levelId", 1);

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
            PhotonNetwork.LocalPlayer.CustomProperties = new ExitGames.Client.Photon.Hashtable();
        }
        #endregion

        #region Nested Types

        #endregion
    }
}
