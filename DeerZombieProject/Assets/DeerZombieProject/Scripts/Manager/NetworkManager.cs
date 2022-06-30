using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

using System;
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
        #endregion

        #region Events and Delegates
        #endregion

        #region Callbacks
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
        #endregion

        #region Internal Methods

        #endregion

        #region Protected Methods

        #endregion

        #region Private Methods

        #endregion

        #region Nested Types

        #endregion
    }
}
