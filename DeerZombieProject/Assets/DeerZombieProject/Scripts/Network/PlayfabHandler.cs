using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PlayFab;
using PlayFab.ClientModels;

using Photon.Realtime;
using Photon.Pun;
using System;

namespace DeerZombieProject
{
    public class PlayfabHandler 
    {
        #region Constant Fields

        #endregion

        #region Static Fields
        private static PlayfabHandler instance;
        public static PlayfabHandler Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PlayfabHandler();
                }

                return instance;
            }
        }
        #endregion

        #region Fields
        private LoginResult loginResult;
        private string photonCustomAuthToken = "";
        private PlayerProfileModel playerProfile;

        public bool IsAuthenticated = false;
        public PlayerProfileModel PlayerProfile
        {
            get
            {
                return playerProfile;
            }
        }
        #endregion

        #region Events and Delegates
        public Action OnPlayfabLoginSuccess;
        public Action OnPlayfabLoginFailed;
        public Action OnPlayfabRegisterSuccess;
        public Action OnPlayfabRegisterFailed;
        public Action OnPlayfabDisplayNameUpdated;
        public Action OnPlayfabDisplayNameUpdateFailed;
        public Action<PlayerProfileModel> OnRequestedPlayerProfileLoaded;

        #endregion

        #region Callbacks
        private void HandlePlayfabLoginSuccess(LoginResult result)
        {
            loginResult = result;
            IsAuthenticated = true;
            RequestPlayerProfile(loginResult.PlayFabId, true);
        }

        private void HandlePlayfabLoginFailed(PlayFabError error)
        {
            IsAuthenticated = false;
            Debug.LogErrorFormat("Failed login with PlayFab, reason : {0}", error.ErrorMessage);
            OnPlayfabLoginFailed?.Invoke();
        }

        private void HandleGetPhotonTokenSuccess(GetPhotonAuthenticationTokenResult result)
        {
            IsAuthenticated = true;
            photonCustomAuthToken = result.PhotonCustomAuthenticationToken;
            OnPlayfabLoginSuccess?.Invoke();
        }

        private void HandleGetPhotonTokenFailed(PlayFabError error)
        {
            IsAuthenticated = false;
            Debug.LogError(error.ErrorMessage);
            OnPlayfabLoginFailed?.Invoke();
        }



        private void HandleOnRegisterSuccess(RegisterPlayFabUserResult result)
        {
            OnPlayfabRegisterSuccess?.Invoke();
        }

        private void HandleOnRegisterFailed(PlayFabError result)
        {
            Debug.LogWarning(result.ToString());
            OnPlayfabRegisterFailed?.Invoke();
        }

        private void HandleGetPlayerProfilleFailed(PlayFabError error)
        {
            // TODO
        }

        private void UpdateLocalPlayerProfile(GetPlayerProfileResult result)
        {
            playerProfile = result.PlayerProfile;
            Debug.Log("Local player profile updated");
            NetworkManager.Instance.SetPlayerName(playerProfile.DisplayName);
            RequestPhotonAuthenticationToken();
        }

        private void HandleGetPlayerProfileSuccess(GetPlayerProfileResult result)
        {
            OnRequestedPlayerProfileLoaded?.Invoke(result.PlayerProfile);
        }

        private void HandleOnDisplayNameUpdate(UpdateUserTitleDisplayNameResult result)
        {
            playerProfile.DisplayName = result.DisplayName;
            NetworkManager.Instance.SetPlayerName(result.DisplayName);
            OnPlayfabDisplayNameUpdated?.Invoke();
        }
        #endregion

        #region Constructors

        #endregion

        #region Public Methods
        public void Init()
        {
            //RequestPhotonAuthenticationToken();
        }

        public void Login(string username, string password)
        {
            if (IsAuthenticated)
            {
                HandlePlayfabLoginSuccess(loginResult);
                return;
            }

            LoginWithPlayFabRequest request = new LoginWithPlayFabRequest();
            request.Username = username;
            request.Password = password;
            PlayFabClientAPI.LoginWithPlayFab(request, HandlePlayfabLoginSuccess, HandlePlayfabLoginFailed);
        }

        public void LoginWithFacebook(string accessToken)
        {
            LoginWithFacebookRequest request = new LoginWithFacebookRequest();
            request.CreateAccount = true;
            request.AccessToken = accessToken;
            PlayFabClientAPI.LoginWithFacebook(request, HandlePlayfabLoginSuccess, HandlePlayfabLoginFailed);
        }

        public void Register(string username, string password, string email, string displayName)
        {
            RegisterPlayFabUserRequest request = new RegisterPlayFabUserRequest();
            request.Username = username;
            request.Password = password;
            request.Email = email;
            request.DisplayName = displayName;
            PlayFabClientAPI.RegisterPlayFabUser(
                request,
                HandleOnRegisterSuccess,
                HandleOnRegisterFailed
                );
        }

        public AuthenticationValues GetPhotonAuthenticationValues()
        {
            AuthenticationValues authValues = new AuthenticationValues();
            authValues.AuthType = CustomAuthenticationType.Custom;
            authValues.AddAuthParameter("username", loginResult.PlayFabId);
            authValues.AddAuthParameter("token", photonCustomAuthToken);
            return authValues;
        }

        public void ChangeDisplayName(string newName)
        {
            UpdateUserTitleDisplayNameRequest request = new UpdateUserTitleDisplayNameRequest();
            request.DisplayName = newName;
            PlayFabClientAPI.UpdateUserTitleDisplayName(request, HandleOnDisplayNameUpdate, null);
        }
        #endregion

        #region Internal Methods

        #endregion

        #region Protected Methods

        #endregion

        #region Private Methods
        private void RequestPhotonAuthenticationToken()
        {
            GetPhotonAuthenticationTokenRequest request = new GetPhotonAuthenticationTokenRequest();
            request.PhotonApplicationId = PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime;
            PlayFabClientAPI.GetPhotonAuthenticationToken(request, HandleGetPhotonTokenSuccess, HandleGetPhotonTokenFailed);
        }

        private void RequestPlayerProfile(string playfabId, bool isLoginRequest = false)
        {
            GetPlayerProfileRequest request = new GetPlayerProfileRequest();
            request.PlayFabId = playfabId;
            request.ProfileConstraints = new PlayerProfileViewConstraints()
            {
                ShowDisplayName = true
            };

            if (isLoginRequest)
            {

                PlayFabClientAPI.GetPlayerProfile(request, UpdateLocalPlayerProfile, HandleGetPlayerProfilleFailed);
                return;
            }
            PlayFabClientAPI.GetPlayerProfile(request, HandleGetPlayerProfileSuccess, HandleGetPlayerProfilleFailed);
        }
        #endregion

        #region Nested Types

        #endregion
    }
}
