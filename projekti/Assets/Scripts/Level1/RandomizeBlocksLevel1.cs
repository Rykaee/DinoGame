using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeBlocksLevel1 : MonoBehaviour
{
    public GameObject hotbar;
    public List<GameObject> symbols = new List<GameObject>();
    public GameObject[] prefabs;
    public GameObject blockRed;
    public GameObject blockGreen;
    public GameObject blockYellow;
    public GameObject blockBlue;

    // Start is called before the first frame update
    void Start()
    {
        symbols.Add(blockBlue);
        symbols.Add(blockYellow);
        symbols.Add(blockRed);
        symbols.Add(blockGreen);

        while (symbols.Count != 10)
        {
            int index = Random.Range(0, prefabs.Length);
            symbols.Add(prefabs[index]);
        }

        symbols = Shuffle<GameObject>(symbols);

        for (int i = 0; i < hotbar.transform.childCount; i++)
        {
            GameObject go = Instantiate(symbols[i], hotbar.transform.GetChild(i));
            go.transform.position = hotbar.transform.GetChild(i).position;
            go.GetComponent<DragAndDrop>().canvas = FindObjectOfType<Canvas>();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Shuffle the list when this method called.
    public static List<T> Shuffle<T>(List<T> _list)
    {
        for (int i = 0; i < _list.Count; i++)
        {
            T temp = _list[i];
            int randomIndex = Random.Range(i, _list.Count);
            _list[i] = _list[randomIndex];
            _list[randomIndex] = temp;
        }
        return _list;
    }
}
