using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DeerZombieProject
{
    public class SplashScreenManager : MonoBehaviour
    {
        #region Constant Fields

        #endregion

        #region Static Fields

        #endregion

        #region Fields
        [SerializeField]
        [Range(0.1f, 10f)]
        private float splashDuration = 1;

        [SerializeField]
        private int loginSceneBuildIndex = 1;
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
            StartCoroutine(nameof(ISplashScreenDelay));
        }

        #endregion

        #region Public Methods

        #endregion

        #region Internal Methods

        #endregion

        #region Protected Methods

        #endregion

        #region Private Methods
        IEnumerator ISplashScreenDelay()
        {
            yield return new WaitForSeconds(splashDuration);
            SceneManager.LoadScene(loginSceneBuildIndex);
        }
        #endregion

        #region Nested Types

        #endregion
    }
}
