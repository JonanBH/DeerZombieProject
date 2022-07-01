using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using static UnityEngine.InputSystem.InputAction;

namespace DeerZombieProject
{
    public class PlayerCharacterControler : MonoBehaviour
    {
        #region Constant Fields

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
        private IDamageable damageTarget;

        private Vector3 moveVelocity = Vector3.zero;
        private Camera currentCamera;
        #endregion

        #region Events and Delegates

        #endregion

        #region Callbacks

        #endregion

        #region Constructors

        #endregion

        #region LifeCycle Methods

        // Start is called before the first frame update
        private void Start()
        {
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
                    photonView.RPC(nameof(RPCDoDamage), RpcTarget.MasterClient);
                }
            }
        }
        #endregion

        #region Internal Methods

        #endregion

        #region Protected Methods

        #endregion

        #region Private Methods
        private void GetInputs()
        {
            Vector2 moveInput = playerInputs.actions["Move"].ReadValue<Vector2>();
            Vector3 moveDirection = (Vector3.forward * moveInput.y + Vector3.right * moveInput.x).normalized;

            if (moveInput.Equals(Vector2.zero) && moveVelocity.magnitude <= minSpeedToMove)
            {
                moveVelocity = Vector3.zero;
                return;
            }

            moveVelocity = Vector3.Lerp(moveVelocity, moveDirection * maxSpeed, accelerationMod * Time.deltaTime);

        }

        private void FindTarget()
        {
            photonView.RPC(nameof(CmdFindTarget), RpcTarget.MasterClient, transform.position, 20f, photonView.ViewID);
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
                if(Vector3.Distance(enemy.transform.position, position) < Vector3.Distance(target.transform.position, position))
                {
                    target = enemy;
                }
            }

            damageTarget = target.gameObject.GetComponent<IDamageable>();
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
                damageTarget = target.gameObject.GetComponent<IDamageable>();
            }
        }

        [PunRPC]
        private void RPCDoDamage()
        {
            damageTarget.TakeDamage(1);
        }
        #endregion

        #region Nested Types

        #endregion
    }
}
