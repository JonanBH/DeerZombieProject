using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DeerZombieProject
{
    public class UIPlayerHeader : MonoBehaviour
    {
        #region Constant Fields

        #endregion

        #region Static Fields

        #endregion

        #region Fields
        [SerializeField]
        private TMP_Text txPlayerName;
        #endregion

        #region Events and Delegates

        #endregion

        #region Callbacks

        #endregion

        #region Constructors

        #endregion

        #region LifeCycle Methods


        #endregion

        #region Public Methods
        public void UpdatePlayerName(string name)
        {
            txPlayerName.text = name;
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
