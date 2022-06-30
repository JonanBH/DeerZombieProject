using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DeerZombieProject
{
    public class UIPlayerInfo : MonoBehaviour
    {
        #region Constant Fields

        #endregion

        #region Static Fields

        #endregion

        #region Fields
        [SerializeField]
        private TMP_Text txNickname;
        #endregion

        #region Events and Delegates

        #endregion

        #region Callbacks

        #endregion

        #region Constructors

        #endregion

        #region LifeCycle Methods

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        #endregion

        #region Public Methods
        public void UpdateNickname(string name)
        {
            txNickname.text = name;
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
