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
        [SerializeField]
        private LevelManager levelManager;
        [SerializeField]
        private List<BasicZombieControler> spawnedEnemies = new List<BasicZombieControler>();

        public bool HasMinions
        {
            get
            {
                return spawnedEnemies.Count > 0;
            }
        }

        #endregion

        #region Events and Delegates
        public System.Action OnAllMinionsDefeated;
        #endregion

        #region Callbacks
        private void HandleOnEnemyDeath(BasicZombieControler enemy)
        {
            spawnedEnemies.Remove(enemy);

            if(spawnedEnemies.Count == 0)
            {
                OnAllMinionsDefeated.Invoke();
            }
        }
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

            GameObject enemy = PhotonNetwork.Instantiate(enemyPrefab.name, transform.position, Quaternion.identity);
            BasicZombieControler enemyControler = enemy.GetComponent<BasicZombieControler>();

            enemyControler.OnDeath += HandleOnEnemyDeath;
            spawnedEnemies.Add(enemyControler);
        }

        public void SpawnEnemy(GameObject prefab)
        {
            if (!PhotonNetwork.IsMasterClient)
                return;

            GameObject enemy = PhotonNetwork.Instantiate(enemyPrefab.name, transform.position, Quaternion.identity);
            BasicZombieControler enemyControler = enemy.GetComponent<BasicZombieControler>();

            enemyControler.OnDeath += HandleOnEnemyDeath;
            spawnedEnemies.Add(enemyControler);
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
