using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
public class PlayerInfo : MonoBehaviour
{
    public PlayerProfileModel profile;

    public static PlayerInfo instance;
    private void Awake() { instance = this;  }

    public void OnLoggedIn()
    {
        GetPlayerProfileRequest getPlayerProfile = new GetPlayerProfileRequest
        {
            PlayFabId = LoginRegister.instance.playFabID,
            ProfileConstraints = new PlayerProfileViewConstraints
            {
                ShowDisplayName = true
            }
        };
        PlayFabClientAPI.GetPlayerProfile(getPlayerProfile,
            result =>
            {
                profile = result.PlayerProfile;
                Debug.Log("Loaded in player:" + profile.DisplayName);
            },
            error => Debug.Log(error.ErrorMessage)
        );
    }
}
