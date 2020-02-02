using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class menu : MonoBehaviour
{
    private GameObject medium;
    private GameObject hard;
    private GameObject globalObject;
    public static bool level1_medium;
    public static bool level1_hard;
    // Start is called before the first frame update
    void Start()
    {
        medium = GameObject.Find("Medium");
        hard = GameObject.Find("Hard");
        level1_medium = true;
        level1_hard = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            SceneManager.LoadScene("Level1", LoadSceneMode.Single);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            medium.GetComponent<Text>().color = Color.red;
            hard.GetComponent<Text>().color = Color.white;
            level1_medium = true;
            level1_hard = false;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            medium.GetComponent<Text>().color = Color.white;
            hard.GetComponent<Text>().color = Color.red;
            level1_hard = true;
            level1_medium = false;
        }   
    }
}
