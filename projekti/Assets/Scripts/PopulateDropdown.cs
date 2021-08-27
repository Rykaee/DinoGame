using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopulateDropdown : MonoBehaviour
{
    public Dropdown dropdown;
    private List<Player> players = null;
    private int selectedPlayerIndex = -1;

    public void Dropdown_IndexChanged(int index)
    {
        Debug.Log(index);
        selectedPlayerIndex = index;
    }

    // Start is called before the first frame update
    void Start()
    {
        players = DatabaseLoader.GetAllPlayers();
        List<string> names = new List<string>();
        foreach(Player p in players)
        {
            names.Add(p.PlayerName);
        }

        // Select first item in list if names-array contains at least
        // one item
        if(names.Count > 0)
        {
            selectedPlayerIndex = 0;
        }

        dropdown.AddOptions(names);
    }

    public void ApplyPlayerChange()
    {
        if (selectedPlayerIndex != -1)
        {
            DatabaseLoader.SetCurrentPlayer(players[selectedPlayerIndex]);
            Application.LoadLevel("Profiles");
        }
    }
}
