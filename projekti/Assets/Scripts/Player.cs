using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    private int id;
    private string playerName;
    private int score;
    private int avatarId;
    private int skipTutorialLevel1;
    private int skipTutorialLevel2;
    private int skipTutorialLevel3;

    public int Id
    {
        get { return id;  }
        set { id = value; }
    }
    
    public string PlayerName
    {
        get { return playerName; }
        set { playerName = value; }
    }

    public int AvatarId
    {
        get { return avatarId; }
        set { avatarId = value; }
    }

    public int Score
    {
        get { return score;  }
        set { score = value; }
    }

    public int SkipTutorialLevel1
    {
        get { return skipTutorialLevel1;  }
        set { skipTutorialLevel1 = value; }
    }
    public int SkipTutorialLevel2
    {
        get { return skipTutorialLevel2; }
        set { skipTutorialLevel2 = value; }
    }
    public int SkipTutorialLevel3
    {
        get { return skipTutorialLevel3; }
        set { skipTutorialLevel3 = value; }
    }


    public Player()
    {
        this.id = 0;
        this.playerName = "";
        this.avatarId = 0;
        this.score = 0;
        this.skipTutorialLevel1 = 0;
        this.skipTutorialLevel2 = 0;
        this.skipTutorialLevel3 = 0;
    }

    public Player(int id, string playerName, int score, int avatarId)
    {
        this.id = id;
        this.playerName = playerName;
        this.avatarId = avatarId;
        this.score = score;
        this.skipTutorialLevel1 = 0;
        this.skipTutorialLevel2 = 0;
        this.skipTutorialLevel3 = 0;
    }

    public string GetAsString()
    {
        string s = "" + id + "#" + playerName + "#" + avatarId + "#" + score + "#" + skipTutorialLevel1 + "#" + skipTutorialLevel2 + "#" + skipTutorialLevel3;
        return s;
    }
}
