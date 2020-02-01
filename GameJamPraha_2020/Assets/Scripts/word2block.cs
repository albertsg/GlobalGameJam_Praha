using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class word2block : MonoBehaviour
{
    // public GameObject Word;
    public GameObject Block;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D coll){    
        StartCoroutine(WordToBlock());
    }

    IEnumerator WordToBlock(){
        yield return new WaitForSeconds(5);

        this.gameObject.SetActive(false);
        // Block.Rect = this.gameObject.Rect;
        Instantiate(Block, this.transform.position, this.transform.rotation, GameObject.Find("Canvas").transform);
    }
}
