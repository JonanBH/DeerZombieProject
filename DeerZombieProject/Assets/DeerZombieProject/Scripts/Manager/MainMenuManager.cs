using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

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

        public override void OnLeftRoom()
        {
            base.OnLeftRoom();
            ChangeToAdventureLobby();
        }
        #endregion

        #region Constructors

        #endregion

        #region LifeCycle Methods

        // Start is called before the first frame update
        void Start()
        {
            uiPlayerHeader.UpdatePlayerName(PhotonNetwork.LocalPlayer.NickName);
            if(PhotonNetwork.CurrentRoom != null)
            {
                ChangeToGameRoom();
            }
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
            roomHandler.UpdateRoomName();
        }

        public void CreateRoom()
        {
            NetworkManager.Instance.CreateRoom();
        }

        public void JoinQuickGame()
        {
            NetworkManager.Instance.JoinQuickGame();
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
