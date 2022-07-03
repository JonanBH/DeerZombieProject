using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;

namespace DeerZombieProject
{
    public class LevelManager : MonoBehaviourPunCallbacks
    {
        #region Constant Fields

        #endregion

        #region Static Fields

        #endregion

        #region Fields
        [SerializeField]
        private List<EnemySpawnData> enemySpawnDataList;
        [SerializeField]
        private List<EnemySpawn> enemySpawners;
        [SerializeField]
        private List<int> enemySpawnResourcePerWave;

        [SerializeField]
        private float minDelayToSpawn = 1;
        [SerializeField]
        private float maxDelayToSpawn = 3;
        [SerializeField]
        private float delayToStartWave = 3;

        [SerializeField]
        private SString CURRENT_WAVE_KEY;
        [SerializeField]
        private SString CURRENT_LEVEL_STATE_KEY;

        [SerializeField]
        private int mainMenuSceneIndex;

        [SerializeField]
        private GameObject leaveGameMenu;

        private float remainingSpawnScore = 0;
        private int currentWave = 0;
        private List<PlayerCharacterControler> characters = new List<PlayerCharacterControler>();
        private List<PlayerCharacterControler> deadCharacters = new List<PlayerCharacterControler>();

        

        public enum LevelState
        {
            BEGIN,
            STARTING_ROUND,
            RUNNING_ROUND,
            ENDING_ROUND,
            FINISHED
        }

        private LevelState currentState = LevelState.BEGIN;
        #endregion

        #region Events and Delegates
        public System.Action<int> OnCurrentWaveUpdated;
        public System.Action<LevelState> OnLevelStateUpdated;
        #endregion

        #region Callbacks
        public override void OnLeftRoom()
        {
            if (PhotonNetwork.IsMasterClient)
            {

            }
            leaveGameMenu.SetActive(true);
        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            leaveGameMenu.SetActive(true);
        }
        #endregion

        #region Constructors

        #endregion

        #region LifeCycle Methods

        // Start is called before the first frame update
        void Start()
        {
            if(PhotonNetwork.IsMasterClient)
            {
                Init();
                ChangeState(LevelState.STARTING_ROUND);
            }
        }

        private void OnDestroy()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }

            ReleaseCallbacks();
        }
        #endregion

        #region Public Methods
        public void LeaveRoom()
        {
            if(PhotonNetwork.CurrentRoom != null)
            {
                PhotonNetwork.LeaveRoom();
            }

            SceneManager.LoadScene(mainMenuSceneIndex);
        }

        void OnApplicationQuit()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;

                foreach(KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
                {
                    if(player.Value != PhotonNetwork.LocalPlayer)
                    {
                        PhotonNetwork.CloseConnection(player.Value);
                    }
                }

            }
        }
        #endregion

        #region Internal Methods

        #endregion

        #region Protected Methods

        #endregion

        #region Private Methods
        private void Init()
        {
            ExitGames.Client.Photon.Hashtable levelProperties = new ExitGames.Client.Photon.Hashtable();
            levelProperties.Add(CURRENT_WAVE_KEY.value, 0);
            levelProperties.Add(CURRENT_LEVEL_STATE_KEY.value, LevelState.BEGIN);
            PhotonNetwork.CurrentRoom.SetCustomProperties(levelProperties);

            PrepareCallbacks();
        }

        private void ChangeState(LevelState newState)
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }

            currentState = newState;

            switch (currentState)
            {
                case LevelState.STARTING_ROUND:
                    remainingSpawnScore = enemySpawnResourcePerWave[currentWave];
                    foreach(PlayerCharacterControler characterControler in deadCharacters)
                    {
                        characterControler.Respawn();
                    }
                    deadCharacters.Clear();
                    StartCoroutine(nameof(DelayToStartRound));
                    break;
                case LevelState.RUNNING_ROUND:
                    StartCoroutine(nameof(HandleEnemiesSpawning));
                    break;
                case LevelState.ENDING_ROUND:
                    StartCoroutine(nameof(HandleEndRound));
                    break;
                case LevelState.FINISHED:
                    SceneManager.LoadScene(mainMenuSceneIndex);
                    break;
            }

            UpdateRoomCurrentWave();
        }

        private void UpdateRoomCurrentWave()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }
            ExitGames.Client.Photon.Hashtable levelProperties = new ExitGames.Client.Photon.Hashtable();
            levelProperties.Add(CURRENT_WAVE_KEY.value, currentWave);
            PhotonNetwork.CurrentRoom.SetCustomProperties(levelProperties);
        }

        private void UpdateRoomCurrentState()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }
            ExitGames.Client.Photon.Hashtable levelProperties = new ExitGames.Client.Photon.Hashtable();
            levelProperties.Add(CURRENT_WAVE_KEY.value, currentWave);
            PhotonNetwork.CurrentRoom.SetCustomProperties(levelProperties);
        }

        private void CheckWaveFinished()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }

            if(remainingSpawnScore > 0)
            {
                return;
            }

            foreach(EnemySpawn spawner in enemySpawners)
            {
                if (spawner.HasMinions)
                {
                    return;
                }
            }

            ChangeState(LevelState.ENDING_ROUND);
        }

        private void PrepareCallbacks()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }

            foreach (EnemySpawn spawner in enemySpawners)
            {
                spawner.OnAllMinionsDefeated += CheckWaveFinished;
            }
        }

        private void ReleaseCallbacks()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }

            foreach (EnemySpawn spawner in enemySpawners)
            {
                spawner.OnAllMinionsDefeated -= CheckWaveFinished;
            }
        }

        private EnemySpawnData GetMinionToSpawn()
        {
            EnemySpawnData spawn;
            do
            {
                spawn = enemySpawnDataList[Random.Range(0, enemySpawnDataList.Count)];
            } while (spawn.minSpawnWave > currentWave);

            return spawn;
        }

        private IEnumerator HandleEnemiesSpawning()
        {
            while(remainingSpawnScore > 0)
            {
                yield return new WaitForSeconds(Random.Range(minDelayToSpawn, maxDelayToSpawn));
                EnemySpawnData minionToSpawnData = GetMinionToSpawn();

                enemySpawners[Random.Range(0, enemySpawners.Count)].SpawnEnemy(minionToSpawnData.prefab);
                remainingSpawnScore -= minionToSpawnData.cost;

            }
        }

        private IEnumerator DelayToStartRound()
        {
            yield return new WaitForSeconds(delayToStartWave);
            ChangeState(LevelState.RUNNING_ROUND);
        }

        private IEnumerator HandleEndRound()
        {
            yield return new WaitForSeconds(delayToStartWave);
            currentWave++;

            if(currentWave >= enemySpawnResourcePerWave.Count)
            {
                ChangeState(LevelState.FINISHED);
            }
            else
            {
                ChangeState(LevelState.STARTING_ROUND);
            }
        }
        #endregion

        #region Nested Types

        #endregion
    }
}
