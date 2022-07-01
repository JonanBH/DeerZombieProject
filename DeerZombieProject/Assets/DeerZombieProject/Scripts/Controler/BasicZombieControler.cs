using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace DeerZombieProject
{
    public class BasicZombieControler : MonoBehaviour, IDamageable
    {
        #region Constant Fields

        #endregion

        #region Static Fields

        #endregion

        #region Fields
        [SerializeField]
        private Transform aimPosition;
        [SerializeField]
        private NavMeshAgent navAgent;
        [SerializeField]
        private PlayerCharacterControler targetPlayer;
        [SerializeField]
        private float detectionRadius = 30;
        [SerializeField]
        private float attackRange = 3;
        [SerializeField]
        private float triggerAttackRange = 1.5f;
        [SerializeField]
        private LayerMask targetLayers;
        [SerializeField]
        private float delayAfterAttack = 1;

        private enum ZombieStates
        {
            IDLE, FOLLOWING, ATTACKING, RECOVER
        }

        private float maxHealth = 10;
        private float currentHealth = 10;
        private ZombieStates currentState = ZombieStates.IDLE;
        private float recoverTimer = 0;
        #endregion

        #region Events and Delegates

        #endregion

        #region Callbacks

        #endregion

        #region Constructors

        #endregion

        #region LifeCycle Methods

        private void Start()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                navAgent.enabled = false;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }

            switch (currentState)
            {
                case ZombieStates.IDLE:
                    HandleIdleState();
                    break;
                case ZombieStates.FOLLOWING:
                    HandleFollowingState();
                    break;
                case ZombieStates.RECOVER:
                    HandleRecoverState();
                    break;
            }
        }

        #endregion

        #region Public Methods
        public Transform GetAimPosition()
        {
            return aimPosition;
        }

        public void TakeDamage(float damage)
        {
            if (!PhotonNetwork.IsMasterClient)
                return;

            currentHealth -= damage;
            Debug.Log("TookDamage");

            if(currentHealth <= 0)
            {
                Debug.Log("died");
                PhotonNetwork.Destroy(gameObject);
            }
        }
        #endregion

        #region Internal Methods

        #endregion

        #region Protected Methods

        #endregion

        #region Private Methods

        /// <summary>
        /// Tries to find a player to set as target, if found, update it's target
        /// and change state to follow player
        /// </summary>
        private void HandleIdleState()
        {
            PlayerCharacterControler[] players = FindObjectsOfType<PlayerCharacterControler>();

            if (players.Length.Equals(0))
            {
                return;
            }

            targetPlayer = players[Random.Range(0, players.Length)];
            ChangeState(ZombieStates.FOLLOWING);
        }

        private void HandleFollowingState()
        {
            if(targetPlayer == null)
            {
                ChangeState(ZombieStates.IDLE);
                return;
            }

            navAgent.SetDestination(targetPlayer.transform.position);

            if(Vector3.Distance(transform.position, targetPlayer.transform.position) <= triggerAttackRange)
            {
                ChangeState(ZombieStates.ATTACKING);
            }
        }

        private void HandleRecoverState()
        {
            recoverTimer -= Time.deltaTime;
            if(recoverTimer <= 0)
            {
                ChangeState(ZombieStates.FOLLOWING);
            }
        }

        private void ChangeState(ZombieStates newState)
        {
            currentState = newState;

            switch (currentState)
            {
                case ZombieStates.ATTACKING:
                    targetPlayer.TakeDamage(1);
                    navAgent.SetDestination(transform.position);
                    ChangeState(ZombieStates.RECOVER);
                    break;
                case ZombieStates.RECOVER:
                    recoverTimer = delayAfterAttack;
                    break;
            }
        }


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, triggerAttackRange);
        }
        #endregion

        #region Nested Types

        #endregion
    }
}
