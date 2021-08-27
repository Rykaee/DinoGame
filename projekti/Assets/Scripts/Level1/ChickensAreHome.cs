using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickensAreHome : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // NOTE: Muuta nuo scoren suorat attribuuttikutsut
        // metodeiksi
        if (Score.numChickensInHome >= Score.numChickensInGame)
        {
            Application.LoadLevel("GameOver");
        }
    }
}
