using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

namespace DeerZombieProject
{
    public class PlayerSpawnerManager : MonoBehaviour
    {
        #region Constant Fields

        #endregion

        #region Static Fields

        #endregion

        #region Fields
        [SerializeField]
        private List<Transform> spawnPoints;
        [SerializeField]
        private GameObject playerPrefab;
        #endregion

        #region Events and Delegates

        #endregion

        #region Callbacks

        #endregion

        #region Constructors

        #endregion

        #region LifeCycle Methods

        // Start is called before the first frame update
        void Start()
        {
            // Spawn player avatar
            PhotonNetwork.Instantiate(playerPrefab.name, spawnPoints[Random.Range(0, spawnPoints.Count)].position, Quaternion.identity);
        }

        #endregion

        #region Public Methods

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
