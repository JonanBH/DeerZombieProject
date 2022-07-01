using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

using Photon.Pun;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

namespace DeerZombieProject
{
    public class LoginManager : MonoBehaviourPunCallbacks
    {
        #region Constant Fields

        #endregion

        #region Static Fields

        #endregion

        #region Fields
        [SerializeField]
        private TMP_InputField ifUsername;
        [SerializeField]
        private TMP_InputField ifPassword;
        [SerializeField]
        private Button btnLogin;
        [SerializeField]
        private Button btnFacebookLogin;
        [SerializeField]
        private Button btnRegister;

        [SerializeField]
        private GameObject registerPanel;
        [SerializeField]
        private TMP_InputField ifRegisterUsername;
        [SerializeField]
        private TMP_InputField ifRegisterPassword;
        [SerializeField]
        private TMP_InputField ifRegisterEmail;
        [SerializeField]
        private TMP_InputField ifRegisterDisplayName;
        [SerializeField]
        private Button btnRegisterClose;
        [SerializeField]
        private Button btnRegisterCreate;

        [SerializeField]
        private GameObject loginPanelGameObject;
        [SerializeField]
        private GameObject DisplayNamePanelGameObject;

        [SerializeField]
        private TMP_InputField ifNewDisplayName;
        [SerializeField]
        private Button btnUpdateDisplayName;

        [SerializeField]
        private int mainMenuSceneIndex = 2;

        [SerializeField]
        private List<GameObject> loadingGears;

        private FacebookHandler fbHandler;
        private PlayfabHandler playfabHandler;
        #endregion

        #region Events and Delegates

        #endregion

        #region Callbacks
        private void HandleOnPlayfabLoginSuccess()
        {
            NetworkManager.Instance.ConnectToMaster(playfabHandler.GetPhotonAuthenticationValues());
        }

        private void HandleOnPlayfabLoginFailed()
        {
            SetInputsInteractable(true);
        }

        public override void OnConnectedToMaster()
        {
            // TODO LOGIN SUCCESS
            Debug.Log("Connected to master!");

            if(playfabHandler.PlayerProfile.DisplayName == null || playfabHandler.PlayerProfile.DisplayName.Length < 3)
            {
                RequestDisplayName();
                return;
            }
            SceneManager.LoadScene(mainMenuSceneIndex);
        }

        private void HandleOnFacebookFailedToLogin()
        {
            SetInputsInteractable(true);
        }

        private void HandleOnFacebookLoggedIn()
        {
            playfabHandler.LoginWithFacebook(fbHandler.GetAccessToken());
        }
        private void HandleOnPlayfabRegisterSuccess()
        {
            ifUsername.text = ifRegisterUsername.text;
            ifPassword.text = ifRegisterPassword.text;
            LoginUsingPlayfabCredentials();
        }

        private void HandleOnPlayfabRegisterFailed()
        {
            SetInputsInteractable(true);
        }

        private void HandleOnPlayfabDisplayNameChangeSuccess()
        {
            SceneManager.LoadScene(mainMenuSceneIndex);
        }

        private void HandleOnPlayfabDisplayNameChangeFailed()
        {
            SetInputsInteractable(true);
        }

        #endregion

        #region Constructors

        #endregion

        #region LifeCycle Methods
        private void Awake()
        {
            fbHandler = FacebookHandler.Instance;
            playfabHandler = PlayfabHandler.Instance;

            fbHandler.Init();
            playfabHandler.Init();

            SetCallbacks();
        }

        private void OnDestroy()
        {
            ReleaseCallbacks();
        }

        #endregion

        #region Public Methods
        public void LoginUsingPlayfabCredentials()
        {
            playfabHandler.Login(ifUsername.text, ifPassword.text);
            SetInputsInteractable(false);
        }

        public void Register()
        {
            registerPanel.SetActive(true);
        }

        public void RequestPlayfabRegister()
        {
            playfabHandler.Register(ifRegisterUsername.text, ifRegisterPassword.text, ifRegisterEmail.text, ifRegisterDisplayName.text);
            SetInputsInteractable(false);
        }

        public void LoginUsingFacebook()
        {
            fbHandler.Login();
            SetInputsInteractable(false);
        }

        public void UpdateDisplayName()
        {
            if(ifNewDisplayName.text.Length < 3 || ifNewDisplayName.text.Length > 25)
            {
                return;
            }
            SetInputsInteractable(false);
            playfabHandler.ChangeDisplayName(ifNewDisplayName.text);
        }
        #endregion

        #region Internal Methods

        #endregion

        #region Protected Methods

        #endregion

        #region Private Methods
        private void SetCallbacks()
        {
            playfabHandler.OnPlayfabLoginSuccess += HandleOnPlayfabLoginSuccess;
            playfabHandler.OnPlayfabLoginFailed += HandleOnPlayfabLoginFailed;
            playfabHandler.OnPlayfabRegisterSuccess += HandleOnPlayfabRegisterSuccess;
            playfabHandler.OnPlayfabRegisterFailed += HandleOnPlayfabRegisterFailed;
            playfabHandler.OnPlayfabDisplayNameUpdated += HandleOnPlayfabDisplayNameChangeSuccess;
            playfabHandler.OnPlayfabDisplayNameUpdateFailed += HandleOnPlayfabDisplayNameChangeFailed;
            fbHandler.OnFacebookLoggedIn += HandleOnFacebookLoggedIn;
            fbHandler.OnFacebookFailedToLogin += HandleOnFacebookFailedToLogin;
        }

        private void ReleaseCallbacks()
        {
            playfabHandler.OnPlayfabLoginSuccess -= HandleOnPlayfabLoginSuccess;
            playfabHandler.OnPlayfabLoginFailed -= HandleOnPlayfabLoginFailed;
            playfabHandler.OnPlayfabRegisterSuccess -= HandleOnPlayfabRegisterSuccess;
            playfabHandler.OnPlayfabRegisterFailed -= HandleOnPlayfabRegisterFailed;
            playfabHandler.OnPlayfabDisplayNameUpdated -= HandleOnPlayfabDisplayNameChangeSuccess;
            playfabHandler.OnPlayfabDisplayNameUpdateFailed -= HandleOnPlayfabDisplayNameChangeFailed;
            fbHandler.OnFacebookLoggedIn -= HandleOnFacebookLoggedIn;
            fbHandler.OnFacebookFailedToLogin -= HandleOnFacebookFailedToLogin;
        }

        private void SetInputsInteractable(bool state)
        {
            ifUsername.interactable = state;
            ifPassword.interactable = state;
            btnLogin.interactable = state;
            btnFacebookLogin.interactable = state;
            btnRegister.interactable = state;

            ifRegisterUsername.interactable = state;
            ifRegisterPassword.interactable = state;
            ifRegisterDisplayName.interactable = state;
            ifRegisterEmail.interactable = state;
            btnRegisterClose.interactable = state;
            btnRegisterCreate.interactable = state;

            ifNewDisplayName.interactable = state;
            btnUpdateDisplayName.interactable = state;

            foreach(GameObject gear in loadingGears)
            {
                gear.SetActive(!state);
            }
        }

        private void RequestDisplayName()
        {
            loginPanelGameObject.SetActive(false);
            DisplayNamePanelGameObject.SetActive(true);
            SetInputsInteractable(true);
        }
        #endregion

        #region Nested Types

        #endregion
    }
}
