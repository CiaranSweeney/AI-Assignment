using UnityEngine;
using System.Collections;

public class KeyPickUp : MonoBehaviour{
    // Use this for initialization
    public bool hasKey = false;
    public KillZone kill;
    public CompleteCameraController ccc;
    public AI ai;
    void Start(){

    }

    // Update is called once per frame
    //take key
    void OnTriggerEnter2D(Collider2D col){
        if (col.gameObject.tag == "Key"){
            hasKey = true;
            Destroy(col.gameObject);
        }
    }
    //kill player if kill mode is on
    void OnCollisionEnter2D(Collision2D col){
        if (col.gameObject.tag == "Player" && kill.entered==true){
            ccc.enabled = false;
            ai.enabled = false;
            Destroy(col.gameObject);
        }
    }
}