using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeerZombieProject
{
    public class EnemySpawn : MonoBehaviour
    {
        #region Constant Fields

        #endregion

        #region Static Fields

        #endregion

        #region Fields
        [SerializeField]
        private GameObject enemyPrefab;
        [SerializeField]
        private bool spawnOnStart;
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
            if (spawnOnStart)
            {
                SpawnEnemy();
            }
        }

        #endregion

        #region Public Methods
        public void SpawnEnemy()
        {
            if (!PhotonNetwork.IsMasterClient)
                return;

            PhotonNetwork.Instantiate(enemyPrefab.name, transform.position, Quaternion.identity);
        }

        public void SpawnEnemy(GameObject prefab)
        {
            if (!PhotonNetwork.IsMasterClient)
                return;

            PhotonNetwork.Instantiate(enemyPrefab.name, transform.position, Quaternion.identity);
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
