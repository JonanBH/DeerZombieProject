using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using static UnityEngine.InputSystem.InputAction;

namespace DeerZombieProject
{
    public class PlayerCharacterControler : MonoBehaviour, IDamageable
    {
        #region Constant Fields
        public SString CURRENT_HEALTH_KEY;
        public SString MAX_HEALTH_KEY;
        public SString CURRENT_KILLS_KEY;
        public SString SCORE_KEY;
        #endregion

        #region Static Fields

        #endregion

        #region Fields
        [SerializeField]
        private PlayerInput playerInputs;
        [SerializeField]
        private CharacterController characterController;
        [SerializeField]
        private PhotonView photonView;
        [SerializeField]
        private float maxSpeed = 10;
        [SerializeField]
        private float accelerationMod = 0.5f;
        [SerializeField]
        private float minSpeedToMove = 0.1f;
        [SerializeField]
        private LayerMask enemyMask;
        [SerializeField]
        private GameObject damageTarget;

        [SerializeField]
        private int maxHealth = 10;
        [SerializeField]
        private int currentHealth;
        [SerializeField]
        private Animator animator;

        private Vector3 moveVelocity = Vector3.zero;
        private Camera currentCamera;
        private int kills = 0;
        private int score = 0;
        private bool isAlive = true;
        #endregion

        #region Events and Delegates
        System.Action OnDeath;
        System.Action OnRespawn;
        #endregion

        #region Callbacks
        
        #endregion

        #region Constructors

        #endregion

        #region LifeCycle Methods

        // Start is called before the first frame update
        private void Start()
        {
            currentHealth = maxHealth;
            currentCamera = Camera.main;
        }

        // Update is called once per frame
        private void Update()
        {
            if (!photonView.IsMine)
                return;

            GetInputs();
        }

        private void FixedUpdate()
        {
            if (!photonView.IsMine)
                return;
            characterController.Move(moveVelocity * Time.fixedDeltaTime);

            if(moveVelocity.normalized.magnitude > 0)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveVelocity.normalized), 0.7f);
            }
            
        }

        #endregion

        #region Public Methods

        public void HandleShootInput(CallbackContext context)
        {
            if (!photonView.IsMine)
            {
                return;
            }

            if (context.interaction is TapInteraction)
            {
                if (damageTarget == null)
                {
                    FindTarget();
                }

                if (damageTarget != null)
                {
                    if(damageTarget.GetComponent<BasicZombieControler>().IsAlive == false)
                    {
                        damageTarget = null;
                        return;
                    }
                    photonView.RPC(nameof(RPCDoDamage), RpcTarget.MasterClient);
                }
            }
        }

        public void TakeDamage(float damage, GameObject attacker = null)
        {
            if (!PhotonNetwork.IsMasterClient)
                return;

            Debug.Log("Player took damage");
            currentHealth -= (int)damage;
            if(currentHealth <= 0)
            {
                OnDeath();

            }

            UpdatePlayerStatus();
        }

        public Transform GetAimPosition()
        {
            return transform;
        }

        public void Respawn()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }

            photonView.RPC(nameof(RPCUpdateDied), RpcTarget.All, false);
        }

        public void AddScore(int amount)
        {
            ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable();

            score += amount;
            properties.Add(SCORE_KEY.value, score);
            photonView.Owner.SetCustomProperties(properties);
        }
        #endregion

        #region Internal Methods

        #endregion

        #region Protected Methods

        #endregion

        #region Private Methods
        private void Init()
        {
            if (!photonView.IsMine) return;

            UpdatePlayerStatus();
        }

        private void GetInputs()
        {
            Vector2 moveInput = playerInputs.actions["Move"].ReadValue<Vector2>();
            Vector3 moveDirection = (Vector3.forward * moveInput.y + Vector3.right * moveInput.x).normalized;

            if (moveInput.Equals(Vector2.zero) && moveVelocity.magnitude <= minSpeedToMove)
            {
                moveVelocity = Vector3.zero;
            }
            else{
                moveVelocity = Vector3.Lerp(moveVelocity, moveDirection * maxSpeed, accelerationMod * Time.deltaTime);
            }

            animator.SetBool("IsMoving", moveVelocity != Vector3.zero);
            animator.SetBool("IsRunning", moveVelocity.magnitude > maxSpeed / 2);
        }

        private void FindTarget()
        {
            photonView.RPC(nameof(CmdFindTarget), RpcTarget.MasterClient, transform.position, 20f, photonView.ViewID);
        }

        private void UpdatePlayerStatus()
        {
            ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable();
            customProperties.Add(CURRENT_HEALTH_KEY.value, currentHealth);
            customProperties.Add(MAX_HEALTH_KEY.value, maxHealth);
            customProperties.Add(CURRENT_KILLS_KEY.value, kills);
            customProperties.Add(SCORE_KEY.value, score);

            photonView.Owner.SetCustomProperties(customProperties);
        }

        [PunRPC]
        private void CmdFindTarget(Vector3 position, float radius, int requestView)
        {
            Collider[] enemies = Physics.OverlapSphere(transform.position, 20, enemyMask);
            if (enemies.Length == 0)
            {
                return;
            }

            Collider target = enemies[0];

            foreach (Collider enemy in enemies)
            {
                if(Vector3.Distance(enemy.transform.position, position) < Vector3.Distance(target.transform.position, position) &&
                    enemy.gameObject.GetComponent<BasicZombieControler>().IsAlive)
                {
                    target = enemy;
                }
            }

            damageTarget = target.gameObject;
            photonView.RPC(nameof(RPCSetTarget), RpcTarget.Others, target.gameObject.GetComponent<PhotonView>().ViewID, requestView);
        }

        [PunRPC]
        private void RPCSetTarget(int targetId, int requestView)
        {
            if(photonView.ViewID != requestView)
            {
                return;
            }

            PhotonView target = PhotonView.Find(targetId);

            if (target)
            {
                damageTarget = target.gameObject;
            }
        }

        [PunRPC]
        private void RPCDoDamage()
        {
            if (damageTarget == null)
                return;

            damageTarget.GetComponent<IDamageable>().TakeDamage(1, gameObject);
        }

        [PunRPC]
        private void RPCUpdateDied(bool value)
        {
            isAlive = !value;

            if (isAlive)
            {
                OnRespawn?.Invoke();
                return;
            }

            OnDeath?.Invoke();

        }
        #endregion

        #region Nested Types

        #endregion
    }
}
