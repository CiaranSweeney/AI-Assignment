using UnityEngine;
using System.Collections;

public class KillZone : MonoBehaviour {

    // Use this for initialization
    public Renderer[] aiColour;
    public bool entered = false;
    public GameObject skeletonKey;
    public AI ai;
    public Key key;
    float time = 0;
    Renderer rend;
    KeyPickUp keyPickUp;
    void Start() {
        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update() {
        if (entered==true){
            //kill mode is turned on
            rend.material.color=Color.red;
            for (int j = 0; j < aiColour.Length; j++){
                aiColour[j].material.color = Color.red;
            }
            time += Time.deltaTime;
        }
        if (time > 10){
            // kill mode is turned off
            ai.update = false;
            entered = false;
            time = 0f;
            rend.material.color = Color.white;
            key.resetkey();
            Instantiate(skeletonKey, new Vector3(-32, -32, 0), Quaternion.identity);
            Instantiate(skeletonKey, new Vector3(-32, 32, 0), Quaternion.identity);
            Instantiate(skeletonKey, new Vector3(32, 32, 0), Quaternion.identity);
            for (int j = 0; j < aiColour.Length; j++){
                aiColour[j].material.color = Color.white;
            }
        }
    }
    void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "AI"){
            keyPickUp= col.gameObject.GetComponent<KeyPickUp>();
            // If the ai has the key then kill zone is on
            if (keyPickUp.hasKey==true) {
                entered = true;
            }     
        }
    }
}
