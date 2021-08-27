using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System;
using UnityEngine.UI;
using System.IO;

public class DatabaseLoader : MonoBehaviour
{
    private static List<Player> players = null;
    private static Player currentPlayer = null;
    private static string databaseName = "Userdata_text";

    void Start()
    {
        players = new List<Player>();

        // Check if database exists
        if (System.IO.File.Exists(GetDatabaseURI()))
        {
            LoadPlayersFromFile();
        }
        else
        {
            Player newPlayer = new Player();
            newPlayer.Id = 1;
            newPlayer.PlayerName = "Pelaaja";
            newPlayer.AvatarId = 1;
            newPlayer.Score = 0;
            newPlayer.SkipTutorialLevel1 = 0;
            newPlayer.SkipTutorialLevel2 = 0;
            newPlayer.SkipTutorialLevel3 = 0;
            players.Add(newPlayer);

            // Write default entry for player profile database
            string entry = newPlayer.GetAsString();
            StreamWriter sw = new StreamWriter(GetDatabaseURI());
            sw.Write(entry);
            sw.Write("\n");
            sw.Close();
            PlayerPrefs.SetInt("lastId", 1);
            SetCurrentPlayer(newPlayer);
        }
    }

    public static List<Player> GetAllPlayers()
    {
        // Late database loading if editor is started from anywhere else than
        // from MainMenu-scene
        if(players == null)
        {
            players = new List<Player>();
            LoadPlayersFromFile();
        }

        return players;
    }

    public static void CreateNewPlayer(string name, int avatarId)
    {
        // Get new id for player from database
        Player newPlayer = new Player();
        newPlayer.Id = GetNewPlayerId();
        newPlayer.PlayerName = name;
        newPlayer.AvatarId = avatarId;
        newPlayer.Score = 0;
        newPlayer.SkipTutorialLevel1 = 0;
        newPlayer.SkipTutorialLevel2 = 0;
        newPlayer.SkipTutorialLevel3 = 0;
        players.Add(newPlayer);
        SetCurrentPlayer(newPlayer);
        SaveCurrentPlayer();
    }

    public static void DeletePlayer(Player player)
    {
        // Find out player by id and remove from list
        bool done = false;
        int indexToRemove = -1;
        for (int i = 0; i < players.Count && !done; i++)
        {
            if (players[i].Id == player.Id)
            {
                indexToRemove = i;
                done = true;
            }
        }

        if (indexToRemove >= 0)
        {
            players.RemoveAt(indexToRemove);
        }

        // Save database to file.
        SaveToFile();
    }

    public static Player GetCurrentPlayer()
    {
        return currentPlayer;
    }

    public static void SaveCurrentPlayer()
    {
        SaveToFile();
    }

    public static void SetCurrentPlayer(Player player)
    {
        currentPlayer = player;
        PlayerPrefs.SetInt("lastId", player.Id);
    }

    private static string GetDatabaseURI()
    {
        string s = Application.persistentDataPath + "/" + databaseName;
        return s;
    }

    private static int GetNewPlayerId()
    {
        int maxId = 0;
        foreach (Player p in players)
        {
            if (p.Id > maxId)
            {
                maxId = p.Id;
            }
        }

        int newId = maxId + 1;
        return newId;
    }

    private static void SaveToFile()
    {
        // Save all players to database
        FileStream fs = new FileStream(GetDatabaseURI(), FileMode.Truncate);
        StreamWriter sw = new StreamWriter(fs);
        foreach (Player p in players)
        {
            string entry = p.GetAsString();
            sw.Write(entry);
            sw.Write("\n");
        }
        sw.Close();
        fs.Close();
    }

    private static void LoadPlayersFromFile()
    {
        // Load all players from database
        string[] lines = File.ReadAllLines(GetDatabaseURI());
        foreach (string s in lines)
        {
            string[] tokens = s.Split('#');
            int id = Int32.Parse(tokens[0]);
            string name = tokens[1];
            int avatarId = Int32.Parse(tokens[2]);
            int score = Int32.Parse(tokens[3]);
            int skipTutorialLevel1 = Int32.Parse(tokens[4]);
            int skipTutorialLevel2 = Int32.Parse(tokens[5]);
            int skipTutorialLevel3 = Int32.Parse(tokens[6]);
            Player newPlayer = new Player(id, name, score, avatarId);
            newPlayer.SkipTutorialLevel1 = skipTutorialLevel1;
            newPlayer.SkipTutorialLevel2 = skipTutorialLevel2;
            newPlayer.SkipTutorialLevel3 = skipTutorialLevel3;
            players.Add(newPlayer);
        }

        // Check if last user id exists
        int lastId = PlayerPrefs.GetInt("lastId");
        if (lastId != 0)
        {
            // Find and set active last player id
            bool found = false;
            for (int i = 0; i < players.Count && !found; i++)
            {
                if (players[i].Id == lastId)
                {
                    found = true;
                    SetCurrentPlayer(players[i]);
                }
            }
        }
        else
        {
            // No previous lastId set, use first one in list
            SetCurrentPlayer(players[0]);
        }
    }

    //    // Start is called before the first frame update
    //    void Start()
    //    {
    //        // Allocate new static Player-instance
    //        currentPlayer = new Player();

    //        // Load binary file from Resources-folder that is packed within APK.
    //        // Binary asset loading is done with Resources.Load()-method

    //        // Check if database file already exists?
    //        if (System.IO.File.Exists(GetDatabaseURI()))
    //        {
    //            // Open existing database file
    //            text.text = "Tiedosto on olemassa";

    //            int lastId = PlayerPrefs.GetInt("lastId");
    //            if (lastId != 0)
    //            {
    //                // Reload player with last played id
    //                Player lastActivePlayer = GetPlayerWithId(lastId);
    //                SetCurrentPlayer(lastActivePlayer);
    //                Debug.Log("Pelaaja, id: " + lastActivePlayer.Id + ", nimi: " + lastActivePlayer.PlayerName + ", avatarId: " + lastActivePlayer.AvatarId + ", score: " + lastActivePlayer.Score);
    //                text.text = "lastid : " + lastId;
    //            }
    //        }
    //        else
    //        {
    //            // Create new database file
    //            TextAsset textAsset = Resources.Load("Userdata") as TextAsset;
    //            Stream s = new MemoryStream(textAsset.bytes);
    //            System.IO.File.WriteAllBytes(GetDatabaseURI(), textAsset.bytes);

    //            // Create tables for database and default player.
    //            //PlayerPrefs.SetInt("lastId", newPlayerId);
    //            currentPlayer = GetPlayerWithId(1);
    //            text.text = "" + currentPlayer.Id;
    //        }

    //        IDbConnection dbconn = new SqliteConnection(GetConnectionString());
    //        dbconn.Open();
    //        IDbCommand cmd = CreateCommand("SELECT id FROM Pelaajat", dbconn);
    //        IDataReader reader = cmd.ExecuteReader();
    //        while (reader.Read())
    //        {
    //            int i = reader.GetInt32(0);
    //        }
    //        dbconn.Close();
    //    }
}


//public class DatabaseLoader : MonoBehaviour
//{
//    public Text text;
//    private static Player currentPlayer = null;
//    private static readonly string tableName = "Pelaajat";
//    private static readonly string databaseName = "Userdata";
//    private static readonly string textDatabaseName = "Userdata_text";

//    // Create new player to database and set it as current player
//    public static void CreateNewPlayer(string name, int avatarIndex)
//    {
//        Player newPlayer = new Player();

//        // Get new player id from database
//        int newPlayerId = -1;
//        IDbConnection dbconn = new SqliteConnection(GetConnectionString());
//        dbconn.Open();
//        IDbCommand cmdNewId = dbconn.CreateCommand();
//        cmdNewId.CommandText = "SELECT MAX(id) FROM " + tableName;
//        try
//        {
//            IDataReader reader = cmdNewId.ExecuteReader();
//            while (reader.Read())
//            {
//                newPlayerId = reader.GetInt32(0);
//                newPlayerId++;
//            }
//            reader.Dispose();
//        }
//        catch
//        {
//            // No players exist in table, start with id = 1
//            newPlayerId = 1;
//        }
//        cmdNewId.Dispose();

//        IDbCommand cmdNewPlayer = dbconn.CreateCommand();
//        cmdNewPlayer.CommandText = "INSERT INTO " + tableName + "(id, nimi, avatarId, kokonaispisteet) VALUES(" + newPlayerId + ", '" + name + "', " + avatarIndex + ", 0)";
//        cmdNewPlayer.ExecuteNonQuery();
//        cmdNewPlayer.Dispose();
//        dbconn.Close();

//        newPlayer.Id = newPlayerId;
//        newPlayer.PlayerName = name;
//        newPlayer.AvatarId = avatarIndex;
//        newPlayer.Score = 0;

//        SetCurrentPlayer(newPlayer);
//    }

//    // Get current loaded Player
//    public static Player GetCurrentPlayer()
//    {
//        return currentPlayer;
//    }

//    // Set current active player
//    public static void SetCurrentPlayer(Player player)
//    {
//        currentPlayer = player;
//        PlayerPrefs.SetInt("lastId", player.Id);
//    }

//    public static void SaveCurrentPlayer()
//    {
//        Player player = GetCurrentPlayer();
//        IDbConnection dbconn = new SqliteConnection(GetConnectionString());
//        dbconn.Open();
//        IDbCommand cmd = dbconn.CreateCommand();
//        cmd.CommandText = "UPDATE " + tableName + " SET kokonaispisteet = " + player.Score + " WHERE id = " + player.Id;
//        cmd.ExecuteNonQuery();
//        cmd.Dispose();
//        dbconn.Close();
//    }

//    public static List<Player> GetAllPlayers()
//    {
//        List<Player> players= new List<Player>();
//        IDbConnection dbconn = new SqliteConnection(GetConnectionString());
//        dbconn.Open();

//        IDbCommand cmd = dbconn.CreateCommand();
//        cmd.CommandText = "SELECT id, nimi, avatarId, kokonaispisteet FROM " + tableName;
//        IDataReader reader = cmd.ExecuteReader();
//        while (reader.Read())
//        {
//            int id = reader.GetInt32(0);
//            string name = reader.GetString(1);
//            int avatarId = reader.GetInt32(2);
//            int score = reader.GetInt32(3);
//            players.Add(new Player(id, name, score, avatarId));
//        }

//        reader.Close();
//        cmd.Dispose();
//        dbconn.Close();
//        return players;
//    }

//    public static void DeletePlayer(Player player)
//    {
//        IDbConnection dbconn = new SqliteConnection(GetConnectionString());
//        dbconn.Open();
//        try
//        {
//            IDbCommand cmd = CreateCommand("DELETE FROM " + tableName + " WHERE id = " + player.Id, dbconn);
//            cmd.ExecuteNonQuery();
//            cmd.Dispose();
//            dbconn.Close();
//        }
//        catch
//        {
//            Debug.Log("Ei onnistunut");
//        }
//    }

//    // Start is called before the first frame update
//    void Start()
//    {
//        // Allocate new static Player-instance
//        currentPlayer = new Player();

//        // Load binary file from Resources-folder that is packed within APK.
//        // Binary asset loading is done with Resources.Load()-method

//        // Check if database file already exists?
//        if (System.IO.File.Exists(GetDatabaseURI()))
//        {
//            // Open existing database file
//            text.text = "Tiedosto on olemassa";

//            int lastId = PlayerPrefs.GetInt("lastId");
//            if (lastId != 0)
//            {
//                // Reload player with last played id
//                Player lastActivePlayer = GetPlayerWithId(lastId);
//                SetCurrentPlayer(lastActivePlayer);
//                Debug.Log("Pelaaja, id: " + lastActivePlayer.Id + ", nimi: " + lastActivePlayer.PlayerName + ", avatarId: " + lastActivePlayer.AvatarId + ", score: " + lastActivePlayer.Score);
//                text.text = "lastid : " + lastId;
//            }
//        }
//        else
//        {
//            // Create new database file
//            TextAsset textAsset = Resources.Load("Userdata") as TextAsset;
//            Stream s = new MemoryStream(textAsset.bytes);
//            System.IO.File.WriteAllBytes(GetDatabaseURI(), textAsset.bytes);

//            // Create tables for database and default player.
//            //PlayerPrefs.SetInt("lastId", newPlayerId);
//            currentPlayer = GetPlayerWithId(1);
//            text.text = "" + currentPlayer.Id;
//        }

//        IDbConnection dbconn = new SqliteConnection(GetConnectionString());
//        dbconn.Open();
//        IDbCommand cmd = CreateCommand("SELECT id FROM Pelaajat", dbconn);
//        IDataReader reader = cmd.ExecuteReader();
//        while (reader.Read())
//        {
//            int i = reader.GetInt32(0);
//        }
//        dbconn.Close();
//    }


//    private void CreateTable()
//    {
//        IDbConnection dbconn = new SqliteConnection(GetConnectionString());
//        dbconn.Open();
//        IDbCommand cmd = CreateCommand("CREATE TABLE IF NOT EXISTS " + tableName + " (id INTEGER PRIMARY KEY, nimi TEXT, avatarId INTEGER, kokonaispisteet INTEGER )", dbconn);
//        IDataReader reader = cmd.ExecuteReader();
//        dbconn.Close();
//    }

//    private int GetNewPlayerId()
//    {
//        int newPlayerId = -1;
//        IDbConnection dbconn = new SqliteConnection(GetConnectionString());
//        dbconn.Open();
//        IDbCommand cmd = CreateCommand("SELECT MAX(id) FROM " + tableName, dbconn);
//        try
//        {
//            IDataReader reader = cmd.ExecuteReader();
//            while (reader.Read())
//            {
//                newPlayerId = reader.GetInt32(0);
//                newPlayerId++;
//            }
//            reader.Dispose();
//        }
//        catch
//        {
//            // No players exist in table, start with id = 1
//            newPlayerId = 1;
//        }

//        cmd.Dispose();
//        dbconn.Close();
//        return newPlayerId;
//    }

//    // Add new player to datase.
//    // Return id for new player.
//    private int AddNewPlayer(string name, int avatarId, int score)
//    {
//        int newPlayerId = GetNewPlayerId();
//        IDbConnection dbconn = new SqliteConnection(GetConnectionString());
//        dbconn.Open();
//        IDbCommand cmd = CreateCommand("INSERT INTO " + tableName + "(id, nimi, avatarId, kokonaispisteet) VALUES(" + newPlayerId + ", '" + name + "', " + avatarId + ", " + score + ")", dbconn);
//        cmd.ExecuteNonQuery();
//        cmd.Dispose();
//        dbconn.Close();
//        return newPlayerId;
//    }

//    private Player GetPlayerWithId(int id)
//    {
//        Player player = new Player();
//        IDbConnection dbconn = new SqliteConnection(GetConnectionString());
//        dbconn.Open();
//        IDbCommand cmd = CreateCommand("SELECT nimi, avatarId, kokonaispisteet FROM " + tableName + " WHERE id = " + id, dbconn);
//        IDataReader reader = cmd.ExecuteReader();
//        text.text = GetConnectionString();
//        while (reader.Read())
//        {
//            string name = reader.GetString(0);
//            int avatarId = reader.GetInt32(1);
//            int score = reader.GetInt32(2);
//            player.Id = id;
//            player.PlayerName = name;
//            player.AvatarId = avatarId;
//            player.Score = score;
//        }

//        reader.Close();
//        cmd.Dispose();
//        dbconn.Close();
//        return player;
//    }

//    void PrintPlayers()
//    {
//        IDbConnection dbconn = new SqliteConnection(GetConnectionString());
//        dbconn.Open();
//        IDbCommand cmd = CreateCommand("SELECT id, nimi, avatarId, kokonaispisteet FROM " + tableName, dbconn);
//        IDataReader reader = cmd.ExecuteReader();
//        while (reader.Read())
//        {
//            int id = reader.GetInt32(0);
//            string name = reader.GetString(1);
//            int avatarId = reader.GetInt32(2);
//            int score = reader.GetInt32(3);
//            Debug.Log("Id: " + id + ", name: " + name + ", avatarId: " + avatarId + ", kokonaispisteet: " + score);
//        }

//        reader.Close();
//        cmd.Dispose();
//        dbconn.Close();
//    }

//    public static string GetConnectionString()
//    {
//        string s = "URI=file:" + GetDatabaseURI();
//        return s;
//    }

//    public static string GetDatabaseURI()
//    {
//        string s = Application.persistentDataPath + "/" + databaseName;
//        return s;
//    }

//    public static IDbCommand CreateCommand(string commandText, IDbConnection connection)
//    {
//        IDbCommand newCommand = connection.CreateCommand();
//        newCommand.CommandText = commandText;
//        return newCommand;
//    }
//}