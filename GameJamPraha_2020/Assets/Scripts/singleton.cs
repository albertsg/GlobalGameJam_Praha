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


    // Start is called before the first frame update
    void Start()
    {
     score = 0;   
     canvas = GameObject.Find("Canvas");

        InvokeRepeating("CreateWord", 1.0f, 1.0f); //To create a new word after X seconds (function, initial delay on start, time between invokes)
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void CreateWord(){ //Generates new words into the Canvas at a random position (but same Y)

        wordPrefab = wordAsset.GetComponent<Text>();
        wordPrefab.text = words[Random.Range(0, words.Length)]; // To make random!
        var rect = canvas.transform as RectTransform;
        Vector2 pos = new Vector2(Random.Range(50, rect.rect.width - 50), rect.rect.height + 200);//50 is a treshold
        GameObject init_word = Instantiate(wordAsset, pos, Quaternion.identity,canvas.transform);

    }
}
