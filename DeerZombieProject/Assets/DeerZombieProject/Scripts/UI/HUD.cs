using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DeerZombieProject
{
    public class HUD : MonoBehaviour
    {
        #region Constant Fields

        #endregion

        #region Static Fields
        private static HUD instance;
        public static HUD Instance
        {
            get { return instance; }
        }
        #endregion

        #region Fields
        [SerializeField]
        private HealthBar healthBar;
        [SerializeField]
        private TMP_Text txScoreValue;

        #endregion

        #region Events and Delegates

        #endregion

        #region Callbacks

        #endregion

        #region Constructors

        #endregion

        #region LifeCycle Methods

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            NetworkManager.Instance.OnMaxHealthChanged += UpdateMaxHealth;
            NetworkManager.Instance.OnCurrentHealthChanged += UpdateCurrentHealth;
            NetworkManager.Instance.OnScoreChanged += UpdateScore;
        }

        private void OnDestroy()
        {
            NetworkManager.Instance.OnMaxHealthChanged -= UpdateMaxHealth;
            NetworkManager.Instance.OnCurrentHealthChanged -= UpdateCurrentHealth;
            NetworkManager.Instance.OnScoreChanged -= UpdateScore;
        }

        #endregion

        #region Public Methods
        public void UpdateCurrentHealth(int currentHealth)
        {
            healthBar.UpdateCurrentHealth(currentHealth);
        }

        public void UpdateMaxHealth(int maxHealth)
        {
            healthBar.UpdateMaxHealth(maxHealth);
        }

        public void UpdateScore(int amount)
        {

            txScoreValue.text = amount.ToString();
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
