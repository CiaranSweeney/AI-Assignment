using UnityEngine;
using System.Collections;

public class Key : MonoBehaviour{
    // Use this for initialization
    public KeyPickUp[] keys;
    public void resetkey(){
        for(int i=0; i<keys.Length; i++){
            keys[i].hasKey = false;
        }
    }
}
