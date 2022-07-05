using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DeerZombieProject
{
    public class HealthBar : MonoBehaviour
    {
        #region Constant Fields

        #endregion

        #region Static Fields

        #endregion

        #region Fields
        [SerializeField]
        private GameObject heartPrefab;

        private RectTransform rectTransform;
        private int maxHealth = 10;
        private int currentHealth = 10;
        private List<HealthIcon> heartIcons;
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
            rectTransform = GetComponent<RectTransform>();
            heartIcons = new List<HealthIcon>();

            ReplaceHealthImages();
        }

        #endregion

        #region Public Methods
        public void UpdateMaxHealth(int amount)
        {
            if(maxHealth == amount)
            {
                return;
            }

            maxHealth = amount;
            ReplaceHealthImages();
        }

        public void UpdateCurrentHealth(int amount)
        {
            currentHealth = amount;
            UpdateIcons();
        }
        #endregion

        #region Internal Methods

        #endregion

        #region Protected Methods

        #endregion

        #region Private Methods
        private void ReplaceHealthImages()
        {
            int childCount = rectTransform.childCount;
            int newHearts = maxHealth / 2;
            while(childCount > 0)
            {
                Destroy(rectTransform.GetChild(childCount - 1).gameObject);
                childCount--;
            }

            heartIcons.Clear();
            for(int i =0; i < newHearts; i++)
            {
                GameObject heart = Instantiate(heartPrefab);
                RectTransform heartTransform = heart.GetComponent<RectTransform>();
                HealthIcon healthIcon = heart.GetComponent<HealthIcon>();

                heartIcons.Add(healthIcon);
                heartTransform.SetParent(rectTransform);
            }

            currentHealth = Mathf.Min(currentHealth, maxHealth);
            UpdateIcons();
        }

        private void UpdateIcons()
        {
            int remainingHealth = currentHealth;
            foreach(HealthIcon icon in heartIcons)
            {
                icon.UpdateSprite(remainingHealth);
                remainingHealth -= 2;
            }
        }
        #endregion

        #region Nested Types

        #endregion
    }
}
