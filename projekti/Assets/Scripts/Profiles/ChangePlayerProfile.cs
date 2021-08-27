using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangePlayerProfile : MonoBehaviour
{
    public Dropdown dropdown;
    public Sprite[] sprites;
    private List<Player> players = null;
    private int selectedPlayerIndex = -1;

    // Start is called before the first frame update
    void Start()
    {
        dropdown.ClearOptions();
        players = DatabaseLoader.GetAllPlayers();
        List<Dropdown.OptionData> items = new List<Dropdown.OptionData>();        
        foreach(Player p in players)
        {
            int avatarId = p.AvatarId;
            string text = p.PlayerName + ", " + p.Score + " pistettä";
            Dropdown.OptionData newOption = new Dropdown.OptionData(text, sprites[avatarId]);
            items.Add(newOption);
        }
        dropdown.AddOptions(items);

        // Select first item in list if names - array contains at least
        // one item
        if (items.Count > 0)
        {
            // Find current player and set that as selected
            Player currentPlayer = DatabaseLoader.GetCurrentPlayer();
            int currentPlayerId = currentPlayer.Id;
            bool found = false;
            for (int i = 0; i < players.Count && !found; i++)
            {
                if (players[i].Id == currentPlayerId)
                {
                    found = true;
                    dropdown.value = i;
                    selectedPlayerIndex = i;
                }
            }
        }
    }

    public void UpdateSelectedPlayerIndex(int index)
    {
        selectedPlayerIndex = index;
    }

    public void ApplyPlayerChange()
    {
        if (selectedPlayerIndex != -1)
        {
            DatabaseLoader.SetCurrentPlayer(players[selectedPlayerIndex]);
            SceneManager.LoadScene("Profiles");
        }
    }
}
