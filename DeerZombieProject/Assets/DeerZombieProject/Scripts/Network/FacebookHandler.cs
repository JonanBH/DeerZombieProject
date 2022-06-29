using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Facebook.Unity;
using System;

namespace DeerZombieProject
{
    public class FacebookHandler
    {
        #region Constant Fields

        #endregion

        #region Static Fields
        private static FacebookHandler singleton;
        #endregion

        #region Fields
        public static FacebookHandler Instance{
            get
            {
                if(singleton == null)
                {
                    singleton = new FacebookHandler();  
                }
                return singleton;
            }
        }
        public bool IsInitialized
        {
            get
            {
                return FB.IsInitialized;
            }
        }
        #endregion

        #region Events and Delegates
        public Action OnFacebookLoggedIn;
        public Action OnFacebookFailedToLogin;
        #endregion

        #region Callbacks
        private void HandleFBInitializedCallback()
        {
            if (!FB.IsInitialized)
            {
                Debug.LogError("Facebook SDK was unable to initialized");
                return;
            }

            Debug.Log("Facebook SDK has been initialized");
        }

        private void HandleAuthCallback(ILoginResult result)
        {
            if (FB.IsLoggedIn)
            {
                TriggerOnFacebookLogin();
                return;
            }

            Debug.LogErrorFormat("Error to login to facebook {0}", result.Error);
            OnFacebookFailedToLogin?.Invoke();
        }
        #endregion

        #region Constructors

        #endregion

        #region Public Methods
        public void Init()
        {
            if (!FB.IsInitialized)
            {
                FB.Init(HandleFBInitializedCallback);
            }
        }

        public void Login()
        {
            if (FB.IsLoggedIn)
            {
                TriggerOnFacebookLogin();
                return;
            }

            var perms = new List<string>() { "public_profile", "email", "user_friends" };
            FB.LogInWithReadPermissions(perms, HandleAuthCallback);
        }

        public string GetAccessToken()
        {
            if (!FB.IsLoggedIn)
            {
                return "";
            }

            return AccessToken.CurrentAccessToken.TokenString;
        }

        #endregion

        #region Internal Methods

        #endregion

        #region Protected Methods

        #endregion

        #region Private Methods

        private void TriggerOnFacebookLogin()
        {
            OnFacebookLoggedIn?.Invoke();
        }
        #endregion

        #region Nested Types

        #endregion
    }
}
