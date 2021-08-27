using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class AddProfile : MonoBehaviour
{
    private string playerName = null;
    private int selectedAvatarIndex = 0;

    public void SetSelectedAvatar(int avatarIndex)
    {
        selectedAvatarIndex = avatarIndex;
    }

    public void SetPlayerName(string s)
    {
        if(s.Length > 0)
        {
            playerName = s;
        }
    }

    public void AddNewProfile()
    {
        bool isNameSet = (playerName != null);
        bool isAvatarIndexSet = (selectedAvatarIndex != 0);
        if (isNameSet && isAvatarIndexSet)
        {
            Debug.Log(playerName);
            DatabaseLoader.CreateNewPlayer(playerName, selectedAvatarIndex);
            SceneManager.LoadScene("Profiles");
        }
    }
}
