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
        [SerializeField]
        private Animator animator;
        private enum ZombieStates
        {
            IDLE, FOLLOWING, ATTACKING, RECOVER, DEAD
        }

        private float maxHealth = 10;
        private float currentHealth = 10;
        private ZombieStates currentState = ZombieStates.IDLE;
        private float timer = 0;
        #endregion

        #region Events and Delegates

        #endregion

        #region Callbacks

        #endregion

        #region Constructors

        #endregion

        #region LifeCycle Methods

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
                case ZombieStates.ATTACKING:
                    HandleAttackingState();
                    break;
                case ZombieStates.RECOVER:
                    HandleRecoverState();
                    break;
                case ZombieStates.DEAD:
                    HandleDeathState();
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
                //PhotonNetwork.Destroy(gameObject);
                ChangeState(ZombieStates.DEAD);
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
        private void HandleAttackingState()
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                ChangeState(ZombieStates.RECOVER);
            }
        }

        private void HandleRecoverState()
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                ChangeState(ZombieStates.FOLLOWING);
            }
        }

        private void HandleDeathState()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }

            timer -= Time.deltaTime;
            if(timer < 0)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }

        private void ChangeState(ZombieStates newState)
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }

            currentState = newState;

            switch (currentState)
            {
                case ZombieStates.FOLLOWING:
                    animator.SetBool("IsMoving", true);
                    break;

                case ZombieStates.ATTACKING:
                    animator.SetTrigger("Attack");
                    targetPlayer.TakeDamage(1);
                    timer = 2;
                    navAgent.SetDestination(transform.position);
                    break;
                case ZombieStates.RECOVER:
                    timer = delayAfterAttack;
                    animator.SetBool("IsMoving", false);
                    break;
                case ZombieStates.DEAD:
                    timer = 5;
                    animator.SetTrigger("Died");
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
