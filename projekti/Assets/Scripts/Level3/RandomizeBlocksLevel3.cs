using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeBlocksLevel3 : MonoBehaviour
{
    public GameObject hotbar;
    public List<GameObject> symbols = new List<GameObject>();
    public GameObject[] prefabs;
    public float secondsUntilBlockIsRemoved = 4.5f;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < hotbar.transform.childCount; i++)
        {
            int index = Random.Range(0, prefabs.Length);
            GameObject go = Instantiate(prefabs[index], hotbar.transform.GetChild(i));
            go.AddComponent<BlockDraggingLevel3>();
            go.GetComponent<BlockDraggingLevel3>().currentBlockSlot = this.gameObject;
            go.GetComponent<BlockDraggingLevel3>().blockType = go.GetComponent<BlockType>().type;
            symbols.Add(go);
        }

        symbols = Shuffle<GameObject>(symbols);
        for (int t = 0; t < symbols.Count; t++)
        {
            Debug.Log(symbols[t]);
        }

        BlockInstantiate();
        StartCoroutine(TimerDelay());
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

    IEnumerator TimerDelay()  //  <-  its a standalone method
    {

        while (true)
        {
            // Rebuild list of available blocks
            symbols.Clear();
            for (int i = 0; i < hotbar.transform.childCount; i++)
            {
                if (hotbar.transform.GetChild(i).childCount != 0)
                {
                    symbols.Add(hotbar.transform.GetChild(i).GetChild(0).gameObject);
                }
            }

            symbols = Shuffle<GameObject>(symbols);

            if (symbols.Count > 0)
            {
                if (this.transform.childCount > 0)
                {
                    Destroy(this.transform.GetChild(0).gameObject);
                    BlockInstantiate();
                    this.transform.GetChild(0).gameObject.transform.localScale = new Vector3(23, 23, 23);
                }
            }

            float v = secondsUntilBlockIsRemoved / 3.0f;
            yield return new WaitForSeconds(v);
            this.transform.GetChild(0).gameObject.transform.localScale *= 0.5f;
            yield return new WaitForSeconds(v);
            this.transform.GetChild(0).gameObject.transform.localScale *= 0.5f;
            yield return new WaitForSeconds(v);
        }
    }

    public int GetBlockType()
    {
        if (this.transform.childCount > 0)
        {
            return this.transform.GetChild(0).GetComponent<BlockDraggingLevel3>().GetBlockType();
        }
        return 0;
    }

    void BlockInstantiate()
    {
        GameObject go1 = Instantiate(symbols[0], this.transform);
        go1.GetComponent<BlockDraggingLevel3>().enabled = false;
        go1.GetComponent<MeshCollider>().enabled = false;
        go1.GetComponent<SoundTrigger>().enabled = false;
    }

}
