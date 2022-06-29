using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

namespace DeerZombieProject
{
    public class MainMenuManager : MonoBehaviourPunCallbacks
    {
        #region Constant Fields

        #endregion

        #region Static Fields

        #endregion

        #region Fields
        [SerializeField]
        private UIPlayerHeader uiPlayerHeader;
        [SerializeField]
        private GameObject mainMenuGameObject;
        [SerializeField]
        private GameObject adventureLobbyGameObject;
        [SerializeField]
        private GameObject gameRoomGameObject;
        [SerializeField]
        private UIRoomHandler roomHandler;
        #endregion

        #region Events and Delegates

        #endregion

        #region Callbacks
        public override void OnCreatedRoom()
        {
            base.OnCreatedRoom();
            ChangeToGameRoom();
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            ChangeToGameRoom();
        }
        #endregion

        #region Constructors

        #endregion

        #region LifeCycle Methods

        // Start is called before the first frame update
        void Start()
        {
            uiPlayerHeader.UpdatePlayerName(PhotonNetwork.LocalPlayer.NickName);
        }

        #endregion

        #region Public Methods
        public void ChangeToMainMenu()
        {
            uiPlayerHeader.gameObject.SetActive(true);
            mainMenuGameObject.SetActive(true);
            adventureLobbyGameObject.SetActive(false);
            gameRoomGameObject.SetActive(false);
        }

        public void ChangeToAdventureLobby()
        {
            uiPlayerHeader.gameObject.SetActive(true);
            mainMenuGameObject.SetActive(false);
            adventureLobbyGameObject.SetActive(true);
            gameRoomGameObject.SetActive(false);
        }

        public void ChangeToGameRoom()
        {
            uiPlayerHeader.gameObject.SetActive(false);
            mainMenuGameObject.SetActive(false);
            adventureLobbyGameObject.SetActive(false);
            gameRoomGameObject.SetActive(true);

            roomHandler.UpdatePlayerSlots();
        }

        public void CreateRoom()
        {
            RoomOptions options = new RoomOptions();
            options.MaxPlayers = 4;
            options.IsVisible = true;
            options.IsOpen = true;
            options.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable();
            options.CustomRoomProperties.Add("levelId", 1);
            PhotonNetwork.CreateRoom("test room", options);
        }

        public void JoinQuickGame()
        {
            RoomOptions options = new RoomOptions();

            options.MaxPlayers = 4;
            options.IsVisible = true;
            options.IsOpen = true;
            options.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable();
            options.CustomRoomProperties.Add("levelId", 1);

            PhotonNetwork.JoinRandomOrCreateRoom(roomOptions: options);
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
