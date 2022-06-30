using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        private float maxHealth = 10;
        private float currentHealth = 10;
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

        #endregion

        #region Nested Types

        #endregion
    }
}
