using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SwitchCreditsLanguage : MonoBehaviour
{
    bool isTextFinnish = true;
    public GameObject kylttiFI;
    public GameObject kylttiEN;

    // Start is called before the first frame update
    void Start()
    {
        kylttiFI.SetActive(true);
        kylttiEN.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchLanguage()
    {
        isTextFinnish = !isTextFinnish;
        if(isTextFinnish)
        {
            // Show Finnish text
            kylttiFI.SetActive(true);
            kylttiEN.SetActive(false);
        }
        else
        {
            // Show English text
            kylttiFI.SetActive(false);
            kylttiEN.SetActive(true);
        }
    }
}
