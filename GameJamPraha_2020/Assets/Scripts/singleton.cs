using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class singleton : MonoBehaviour
{

    //Public variables
    public GameObject wordAsset;
    public string[] words;
    //Private variables
    private int score;
    private GameObject canvas;
    private Text wordPrefab;
    private int aux; //DELETE
    // Start is called before the first frame update
    void Start()
    {
     score = 0;   
     canvas = GameObject.Find("Canvas");
     aux = 0;
    }

    // Update is called once per frame
    void Update()
    {
        while(aux == 0){//DELETE
            CreateWord();
            aux++;
        }
    }


    private void CreateWord(){

        wordPrefab = wordAsset.GetComponent<Text>();
        wordPrefab.text = words[0]; // To make random!
        var rect = canvas.transform as RectTransform;
        Vector2 pos = new Vector2(rect.rect.width/2, rect.rect.height + 20);
        GameObject init_word = Instantiate(wordAsset, pos, Quaternion.identity,canvas.transform);

    }
}
