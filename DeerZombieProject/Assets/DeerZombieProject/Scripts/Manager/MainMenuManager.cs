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
        [SerializeField]
        private RectTransform roomsParentRectTransform;
        [SerializeField]
        private GameObject joinButtonPrefab;

        private Dictionary<string, RoomInfo> roomsCache = new Dictionary<string, RoomInfo>();
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

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            base.OnRoomListUpdate(roomList);
            UpdateCachedRooms(roomList);
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

            roomHandler.UpdateRoomDetails();
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
        private void UpdateRooms()
        {
            int remainingRooms = roomsParentRectTransform.childCount;
            while(remainingRooms > 0)
            {
                Destroy(roomsParentRectTransform.GetChild(remainingRooms - 1).gameObject);
                remainingRooms--;
            }

            foreach(KeyValuePair<string, RoomInfo> room in roomsCache)
            {
                GameObject newItem = Instantiate(joinButtonPrefab);
                newItem.GetComponent<UIJoinRoomBtn>().SetRoomData(room.Value.Name, room.Value.PlayerCount, room.Value.MaxPlayers);
                newItem.GetComponent<RectTransform>().SetParent(roomsParentRectTransform);
            }
        }

        private void UpdateCachedRooms(List<RoomInfo> roomList)
        {
            for (int i = 0; i < roomList.Count; i++)
            {
                RoomInfo info = roomList[i];
                if (info.RemovedFromList)
                {
                    roomsCache.Remove(info.Name);
                }
                else
                {
                    roomsCache[info.Name] = info;
                }
            }

            UpdateRooms();
        }
        #endregion

        #region Nested Types

        #endregion
    }
}
