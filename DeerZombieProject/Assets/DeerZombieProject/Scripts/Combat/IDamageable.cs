using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeerZombieProject
{
    public interface IDamageable
    {
        #region Field
        
        #endregion

        #region Events

        #endregion

        #region Methods
        public void TakeDamage(float damage);
        public Transform GetAimPosition();
        #endregion

        #region Nested Types

        #endregion
    }
}
