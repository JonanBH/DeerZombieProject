using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
        #endregion

        #region Nested Types

        #endregion
    }
}
