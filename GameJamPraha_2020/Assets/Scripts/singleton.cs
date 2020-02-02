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
    public Text playerScore;
    private GameObject canvas;
    public menu menu;
    public float canvasHeight;
    private Text wordPrefab;
    public bool gameOver;
    public bool[] words_typed; //If they have been spaewned or not
    public int[] w_spawned; //To know the order of spawned words (2-34-5-21-1-45...etc)
    public int[] size_words; //Array with information of the lenght of the words. Useful to control once the word has been completelly typed
    public int wspawned; //Number of words spawned to see if all words have been spawned or not
    public bool selected; //If word has been selected
    public char[] myWord; // The current word selected
    public bool myWord_created;
    public int wsel; //Position of the word selected in the array of words
    public int highScore; //We get the highscore from PlayerPrefs
    private string highScoreKey = "HighScore";
    private bool newHighScore = false;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;   
        canvas = GameObject.Find("Canvas");
        canvasHeight = canvas.GetComponent<RectTransform>().rect.height;
        wspawned = 0;
        selected = false;
        myWord_created = false;
        words_typed = new bool[words.Length]; //Create an array of bool to identify if a word has been spawned or not
        w_spawned = new int[words.Length]; //Create an array of int to identify the order of the words spawned
        size_words = new int[words.Length];
        
        for (int i = 0; i < words_typed.Length; i++)
        {
            words_typed[i] = false; //Set all spawned words to false
            w_spawned[i] = -1; // -1 will be our "null" or "empty"
            size_words[i] = words[i].Length; //Set size of words in the array. Once it gets to 0 it means it's deleted
        }
        if (menu.level1_medium){
            InvokeRepeating("CreateWord", 0.1f, 1.2f); //To create a new word after X seconds (function, initial delay on start, time between invokes)
        }
        if (menu.level1_hard){
            InvokeRepeating("CreateWord", 0.1f, 0.3f); //To create a new word after X seconds (function, initial delay on start, time between invokes)
        }
        gameOver = false;

        highScore = PlayerPrefs.GetInt(highScoreKey, 0);
        GameObject ghs = GameObject.Find("Game_highscore");
        ghs.GetComponent<Text>().text = highScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver){
            Debug.Log("GAME OVER!");
            GameOverToCenter();
            if(score > highScore){
                PlayerPrefs.SetInt(highScoreKey, score);
                PlayerPrefs.Save();
                highScore = score;
                newHighScore = true;
            }
            if (Input.GetKeyDown("space"))
            {
                print("space key was pressed");
            }
        }
        
        listenKeys();
    }

    private void listenKeys() //Key listener (for typing)
    {
        foreach (char letter in Input.inputString)
        {
            checkTyped(letter); //To check it only once key is pressed, not all the time
        }
    }

/*
Bugs:
- It takes the first word of the list with that letter, not in order of spawn!
*/
    private void checkTyped(char rkey){
        char[] aux;

        if (!selected) //If a word has not been selected
        {
           FindWordList(rkey);
        }
        else //If the word has been selected
        {
            if(rkey == myWord[0]){ //If next typed letter is from the list, update it
                aux = new char[myWord.Length];
                for (int i = 0; i < myWord.Length - 1; i++) //Copy all the letters except the first one into an aux char array
                {
                    aux[i] = myWord[i + 1];
                }
                myWord = aux;
                string retstring = words[wsel]; //Will return the string before the update so we can find it in the gameobjects
                string auxs = new string(myWord); //Convert to string
                size_words[wsel]--; //Update the size of the word
                if (size_words[wsel] <= 0)
                { //If words has been completed, delete
                    DeleteWord(retstring, false);
                    //w_spawned[wsel] = -1;
                    //And then we add points to our score
                    if(!gameOver){
                        AddScore();
                    }
                }
                else
                { //Otherwise update word in gameobject
                    words[wsel] = auxs; //Replace word in the words list
                    UpdateGameObjectWithWord(retstring, wsel); //Update the gameobject word
                }
            }else{//If the typed word is from another word
                FindWordList(rkey);
            }
        }
    }



    private string FindWordList(char rkey){ //Will find the word in the list and will update it. Also will call for a function to update the gameobject
        int xword = 0;
        char[] saux;
        char[] aux;
        bool found = false;
        string retstring = "";

        for (int i = 0; i < wspawned; i++)
        {
            if (!found)
            {
                if (w_spawned[i] >= 0)
                { //To check only the spawned words
                    saux = words[w_spawned[i]].ToCharArray(); //We find the words by spawning order
                    if (saux[0] == rkey)
                    {
                        found = true;
                        selected = true;
                        wsel = w_spawned[i];
                        myWord = words[wsel].ToCharArray();
                        retstring = words[w_spawned[i]]; //Will return the string before the update so we can find it in the gameobjects
                        aux = new char[myWord.Length];
                        for (int x = 0; x < myWord.Length - 1; x++) //Copy all the letters except the first one into an aux char array
                        {
                            aux[x] = myWord[x + 1];
                        }
                        myWord = aux;
                        string auxs = new string(myWord); //Convert to string
                        size_words[w_spawned[i]]--; //Update the size of the word
                        if (size_words[w_spawned[i]] <= 0)
                        { //If words has been completed, delete
                            DeleteWord(retstring, false);
                            if(!gameOver){
                                AddScore();
                            }   
                        }
                        else
                        { //Otherwise update word in gameobject
                            words[w_spawned[i]] = auxs; //Replace word in the words list
                            UpdateGameObjectWithWord(retstring, w_spawned[i]); //Update the gameobject with the new string
                        }
                    }
                }
            }
            else
            {
                break;
            }
        }

        return retstring;
    }

    private void UpdateGameObjectWithWord(string findstring, int pos){//This will update the word in the gameobject
        string saux = words[pos];
        Text go_text;
        GameObject[] respawns = GameObject.FindGameObjectsWithTag("WordPrefab"); //Gets array of words gameobject in the scene
        bool found = false;

        foreach (GameObject respawn in respawns)
        {
            go_text = respawn.GetComponent<Text>();
            if(!found){ //If we didn't find the correct gameobject
                if(go_text.text == findstring){ //In order to find the gameobject we compare the string before deleting a word with the current string of the gameobject
                    found = true;
                    go_text.text = saux; //Update the component text with the new text already updated
                    go_text.color = Color.green;
                    break;//We break the loop once found to avoid more unnecesary iterations
                }
            }

        }
    }

    public void DeleteWord(string findstring, bool fromObj){ //Delete the gameobject
        Text go_text;
        GameObject[] respawns = GameObject.FindGameObjectsWithTag("WordPrefab"); //Gets array of words gameobject in the scene
        foreach (GameObject respawn in respawns)
        {
            go_text = respawn.GetComponent<Text>();
            if (go_text.text == findstring)
            { //In order to find the gameobject we compare the string before deleting a word with the current string of the gameobject
                selected = false; 
                if(fromObj){
                    //We need to find the position of the word since it's not selected anymore
                    int loc_pos = FindPositionInList(findstring);
                    if(loc_pos > -1){ //POZOR this can cause ISSUES when -1! What should it remove?-------------------------------
                        Debug.Log("Word can be found in position: " + loc_pos);
                        char[] del = new char[words[loc_pos].ToCharArray().Length];
                        del = words[loc_pos].ToCharArray();
                        for (int i = 0; i < del.Length; i++)
                        {
                            del[i] = char.MinValue;
                        }
                        string auxs = new string(del); //Convert to string
                        words[loc_pos] = auxs;
                        size_words[loc_pos] = 0;
                        Destroy(respawn); //Remove the gameobject
                        break;//We break the loop once found to avoid more unnecesary iterations
                    }
                }else //It has been deleted by the user
                {
                    char[] del = new char[words[wsel].ToCharArray().Length];
                    del = words[wsel].ToCharArray();
                    for (int i = 0; i < del.Length; i++)
                    {
                        del[i] = char.MinValue;
                    }
                    string auxs = new string(del); //Convert to string
                    words[wsel] = auxs;
                    size_words[wsel] = 0; 
                    Destroy(respawn); //Remove the gameobject
                    break;//We break the loop once found to avoid more unnecesary iterations
                }
            }
        }
    }

    private void CreateWord(){ //Generates new words into the Canvas at a random position (but same Y)
        if(!gameOver){
            if (!AllWordsSpawned())
            {//Checks if all words have been spawned
                wordPrefab = wordAsset.GetComponent<Text>();
                int rand;
                do
                {
                    rand = Random.Range(0, words.Length);
                } while (words_typed[rand]);
                wordPrefab.text = words[rand]; // To make random!
                var rect = canvas.transform as RectTransform;
                Vector2 pos = new Vector2(Random.Range(300, rect.rect.width - 300), rect.rect.height + 200);
                GameObject init_word = Instantiate(wordAsset, pos, Quaternion.identity, canvas.transform);
                words_typed[rand] = true;
                w_spawned[wspawned] = rand; //This will indicate what word (rand) has been spawned and in what order (w_spawned[order])
                if (!myWord_created)
                { //Only gets here once, when the first object/word is created
                    myWord = words[rand].ToCharArray();
                    myWord_created = true;
                    wsel = rand;
                }
                wspawned++;//Increase the number of spawned words
            }
        }

    }

    private bool AllWordsSpawned(){
        return wspawned == words.Length;
    }

    private void AddScore(){
        score += 100;
        playerScore.text = score.ToString();
    }

    private void UpdateBeforeChange(){
        words[wsel] = myWord.ToString();
        size_words[wsel] = myWord.Length;
    }

    private int FindPositionInList(string palabra){
        int iaux = 0;
        int iret = -1;
        foreach (string item in words)
        {
            if(palabra == item){
                iret = iaux;
            }else{
                iaux++;
            }
        }
        return iret;
    }

    private void GameOverScreen(){
        Time.timeScale = 0; //Stops everything
        CancelInvoke(); //Cancel invoke
        GameOverToCenter();
    }


   

    private void GameOverToCenter(){
        GameObject go1 = GameObject.Find("GameOver");
        GameObject go2 = GameObject.Find("GameOverBack");
        GameObject hs1 = GameObject.Find("HishScore");
        GameObject hs2 = GameObject.Find("HighScore_score");
        GameObject ret = GameObject.Find("Return");


        RectTransform assign_text_2RT = go2.GetComponent<RectTransform>();
        assign_text_2RT.anchoredPosition = new Vector3(12f, -3f, 0f);
        go2.transform.SetAsLastSibling();

        RectTransform assign_text_1RT = go1.GetComponent<RectTransform>();
        assign_text_1RT.anchoredPosition = new Vector3(6f, 0f, 0f);
        go1.transform.SetAsLastSibling();

        if(newHighScore){
            RectTransform assign_text_3RT = hs1.GetComponent<RectTransform>();
            assign_text_3RT.anchoredPosition = new Vector3(0f, -114f, 0f);
            hs1.transform.SetAsLastSibling();

            hs2.GetComponent<Text>().text = highScore.ToString();
            RectTransform assign_text_4RT = hs2.GetComponent<RectTransform>();
            assign_text_4RT.anchoredPosition = new Vector3(0f, -171f, 0f);
            hs2.transform.SetAsLastSibling();
        }

        RectTransform assign_text_5RT = ret.GetComponent<RectTransform>();
        assign_text_5RT.anchoredPosition = new Vector3(0f, -286f, 0f);
        ret.transform.SetAsLastSibling();

    }


}
