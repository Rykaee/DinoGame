using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockType : MonoBehaviour
{
    public int type = 0;
    public int Type
    {
        get
        {
            return type;
        }
        set
        {
            type = value;
        }
    }

    public BlockType()
    {
        type = 0;
    }

    public BlockType(int t)
    {
        type = t;
    }
}
