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
    private AudioSource pop_transform;
    // Start is called before the first frame update
    void Start()
    {
        GameObject g = GameObject.FindGameObjectWithTag("MainCamera");
        infoWorld = g.GetComponent<singleton>();
        pop_transform = infoWorld.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {   
        if (this.transform.position.y > infoWorld.canvasHeight && this.getCollision && this.GetComponent<Rigidbody2D>().velocity.y >= 0){ //GameOver condition
            infoWorld.gameOver = true; //Updates gameOver in singleton
        }
    }

    void OnCollisionEnter2D(Collision2D coll){ //On Collision
        this.getCollision = true;
        StartCoroutine(WordToBlock());
    }

    IEnumerator WordToBlock(){ //Creates a block in the word position
        yield return new WaitForSeconds(5);

        this.gameObject.transform.localScale = new Vector3(0, 0, 0); //Hiding the element by scaling it to 0 makes it available to find
        Instantiate(Block, this.transform.position, this.transform.rotation, GameObject.Find("Canvas").transform);
        pop_transform.Play(0);
        infoWorld.GetComponent<singleton>().DeleteWord(this.GetComponent<Text>().text, true);
    }
}
