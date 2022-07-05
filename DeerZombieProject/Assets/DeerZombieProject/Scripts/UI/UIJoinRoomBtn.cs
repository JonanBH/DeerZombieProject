using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using TMPro;

namespace DeerZombieProject
{
    public class UIJoinRoomBtn : MonoBehaviour
    {
        #region Constant Fields

        #endregion

        #region Static Fields

        #endregion

        #region Fields
        [SerializeField]
        private TMP_Text txRoomName;
        [SerializeField]
        private TMP_Text txPlayersOnRoom;
        private string roomName;
        #endregion

        #region Events and Delegates

        #endregion

        #region Callbacks

        #endregion

        #region Constructors

        #endregion

        #region LifeCycle Methods

        #endregion

        #region Public Methods
        public void JoinRoom()
        {
            PhotonNetwork.JoinRoom(roomName);
        }

        public void SetRoomData(string name, int joinedPlayers, int maxPlayers)
        {
            roomName = name;
            txRoomName.text = name;
            txPlayersOnRoom.text = joinedPlayers.ToString() + " / " + maxPlayers.ToString();
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
