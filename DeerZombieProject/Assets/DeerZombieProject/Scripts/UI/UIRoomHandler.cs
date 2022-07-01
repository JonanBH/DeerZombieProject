using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

using TMPro;

namespace DeerZombieProject
{
    public class UIRoomHandler : MonoBehaviourPunCallbacks
    {
        #region Constant Fields

        #endregion

        #region Static Fields

        #endregion

        #region Fields
        [SerializeField]
        private RectTransform playersParentRectTransform;
        [SerializeField]
        private GameObject playerInfoPrefab;
        [SerializeField]
        private TMP_Text txRoomName;
        [SerializeField]
        private GameObject btnReady;
        [SerializeField]
        private GameObject btnStartGame;
        #endregion

        #region Events and Delegates

        #endregion

        #region Callbacks
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            UpdatePlayerSlots();
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            UpdatePlayerSlots();
        }
        #endregion

        #region Constructors

        #endregion

        #region LifeCycle Methods

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        #endregion

        #region Public Methods
        public void UpdatePlayerSlots()
        {
            int remainingChildren = playersParentRectTransform.childCount;
            while(remainingChildren > 0)
            {
                Destroy(playersParentRectTransform.GetChild(remainingChildren - 1).gameObject);
                remainingChildren--;
            }

            foreach(KeyValuePair<int, Player> keyValue in PhotonNetwork.CurrentRoom.Players)
            {
                RectTransform newPlayerInfo = Instantiate(playerInfoPrefab).GetComponent<RectTransform>();
                newPlayerInfo.SetParent(playersParentRectTransform);

                UIPlayerInfo playerInfo = newPlayerInfo.GetComponent<UIPlayerInfo>();

                playerInfo.UpdateNickname(keyValue.Value.NickName);
            }
        }

        public void UpdateRoomName()
        {
            txRoomName.text = PhotonNetwork.CurrentRoom.Name;
        }

        public void UpdateRoomDetails()
        {
            UpdatePlayerSlots();
            UpdateRoomName();

            btnReady.SetActive(false);
            btnStartGame.SetActive(PhotonNetwork.IsMasterClient);
        }

        public void LeaveRoom()
        {
            NetworkManager.Instance.LeaveRoom();
        }

        public void StartGame()
        {
            NetworkManager.Instance.StartGame();
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
