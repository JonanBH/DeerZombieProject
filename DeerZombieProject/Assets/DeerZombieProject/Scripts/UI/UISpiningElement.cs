using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeerZombieProject
{
    public class UISpiningElement : MonoBehaviour
    {
        #region Constant Fields

        #endregion

        #region Static Fields

        #endregion

        #region Fields
        [SerializeField]
        private float angularVelocity = 0;
        [SerializeField]
        private RectTransform rectTransform;
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
            rectTransform.rotation *= Quaternion.Euler(Vector3.forward * angularVelocity * Time.deltaTime);
        }

        #endregion

        #region Public Methods

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
