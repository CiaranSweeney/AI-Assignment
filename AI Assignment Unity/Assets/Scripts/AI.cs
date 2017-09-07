using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI : MonoBehaviour {

    // Use this for initialization
    public KillZone kill; 
    public float maxSpeed;
    public GameObject[] ai;
    public Rigidbody2D[] rbs;
    public GameObject player;
    public List<GameObject> points;
    public bool update=false;

    public List<Vector3> goapOrder = new List<Vector3>();

    public int[] aiJobs; 

    Vector3 desiredVelocity;
    Vector3 keyUsed;
    Vector3[] keysPlacement;

    int count = 0;
    bool exit = false;
    //Rigidbody2D rb;
    void Start () {
        //rb = GetComponent<Rigidbody2D>();
        keysPlacement = new Vector3[3];
        //Keys positions
        keysPlacement[0] = new Vector3(-32, -32, 0);
        keysPlacement[1] = new Vector3(-32, 32, 0);
        keysPlacement[2] = new Vector3(32, 32, 0);
        aiJobs = new int[ai.Length];
        resetJobs();
    }
	// Update is called once per frame
	void FixedUpdate () {
        //update jobs
        if (update==false){
            resetJobs();
            goapOrder.Clear();
            count = 0;
            goToKey();
            chasePlayer();
            pointGuard();
            for (int i = 0; i < ai.Length; i++){
                if (aiJobs[i]==1){
                    goapForKill(ai[i].transform.position);
                }
            }
            update = true;
        }
        //run jobs
        else if (update == true){
            for (int i = 0; i < ai.Length; i++){
                if (aiJobs[i] == 1){
                    moveForKill(ai[i].transform.position,i);
                    //rbs[i].AddForce(seek(goapOrder[0], ai[i].transform.position, rbs[i]));
                    if (aiJobs[i] == 1 && exit==false) {
                        rbs[i].AddForce(seek(goapOrder[0], ai[i].transform.position, rbs[i]));
                    }
                    exit = false;
                }
                else if(aiJobs[i] == 2){
                    rbs[i].AddForce(seek(player.transform.position, ai[i].transform.position, rbs[i]));
                }
               else if(aiJobs[i] == 3){
                    pointGuarding(ai[i].transform.position);
                    rbs[i].AddForce(seek(points[count].transform.position, ai[i].transform.position, rbs[i]));
                }
            }
        }

	}
    //chase
    Vector3 seek(Vector3 seekTarget,Vector3 aiPos,Rigidbody2D rb){
        desiredVelocity = seekTarget - aiPos;
        desiredVelocity.Normalize();
        desiredVelocity *= maxSpeed;
        if (rb.velocity.magnitude > maxSpeed){
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
        transform.up = desiredVelocity;
        return desiredVelocity - (Vector3)rb.velocity;
    }
    //get to kill mode
    void goapForKill(Vector3 aiPos){
        float lowestCost= float.MaxValue;
        float cost;
        for (int i=0; i<keysPlacement.Length; i++){
            cost= Vector3.Distance(aiPos, keysPlacement[i]);
            cost+= Vector3.Distance(keysPlacement[i], kill.transform.position);
            if (cost<lowestCost){
                lowestCost = cost;
                keyUsed = keysPlacement[i];
            }
        }
        goapOrder.Add(keyUsed);
        goapOrder.Add(kill.transform.position);
    }
    //check to see which ai gets the kill mode job
    void goToKey(){
        //furterest AI from player
        float highestCost = 0;
        float cost = 0;
        int index=0;
        for (int i=0;  i<ai.Length; i++) {
            cost = Vector3.Distance(ai[i].transform.position, player.transform.position);
            if (cost > highestCost){
                highestCost = cost;
                index = i;
            }
        }
        aiJobs[index]=1;
    }
    //checks to see which ai get the chases the ai job
    void chasePlayer(){
        float lowestCost = float.MaxValue;
        float cost = 0;
        int index = 0;
        for (int i = 0; i < ai.Length; i++){
            cost = Vector3.Distance(ai[i].transform.position, player.transform.position);
            if (cost < lowestCost  && aiJobs[i]==0){
                lowestCost = cost;
                index = i;
            }
        }
        aiJobs[index] = 2;
    }
    //checks to see which ai get the point guard the ai job
    void pointGuard(){
        int index = 0;
        for (int i = 0; i < ai.Length; i++){
            if (aiJobs[i] == 0){
                index = i;
            }
        }
        aiJobs[index] = 3;
    }
    // checks to see if the agent reached the lists position
    void moveForKill(Vector3 aiPos, int i){
        if (goapOrder.Count==0 && points.Count > 10){
            aiJobs[i]=2;
        }
        else if (goapOrder.Count == 0 && points.Count < 10){
            aiJobs[i] = 3;
        }
        else if (Vector3.Distance(aiPos, goapOrder[0]) < 1f){
            goapOrder.RemoveAt(0);
        }
        if (goapOrder.Count==0 && aiJobs[i]== 1){
            exit = true;
        }

    }
    // checks for amount of points left
    void pointGuarding(Vector3 aiPos){
        float cost = Vector3.Distance(aiPos, points[count].transform.position);
        if (cost < 1f && count<points.Count){
            count++;
        }
        else if (count == points.Count){
            count = 0;
        }
    }
    void resetJobs(){
        for (int i = 0; i < aiJobs.Length; i++){
            aiJobs[i] = 0;
        }
    }

}
