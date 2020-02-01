using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class word2block : MonoBehaviour
{
    // public GameObject Word;
    public GameObject Block;
    public singleton infoWorld;
    public bool getCollision;
    // Start is called before the first frame update
    void Start()
    {
        GameObject g = GameObject.FindGameObjectWithTag("MainCamera");
        infoWorld = g.GetComponent<singleton>();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.position.y > infoWorld.canvasHeight && this.getCollision){
            infoWorld.gameOver = true;
            Debug.Log(infoWorld.canvasHeight);
            Debug.Log(this.transform.position.y);
        }
    }

    void OnCollisionEnter2D(Collision2D coll){
        this.getCollision = true;
        StartCoroutine(WordToBlock());
    }

    IEnumerator WordToBlock(){
        yield return new WaitForSeconds(5);

        this.gameObject.SetActive(false);
        // Block.Rect = this.gameObject.Rect;
        Instantiate(Block, this.transform.position, this.transform.rotation, GameObject.Find("Canvas").transform);
    }
}
