using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Game : MonoBehaviour
{
    public GameObject shipPrefab;//object to be cloned and spawned
    public List<GameObject> objectPool;//list of inactive objects
    public List<GameObject> spawnedObjects;//objects currently active
    public Text textBox;//reference to the Canvas element
    public GameObject camera;//reference to the camera
    public Vector3 spawnPosition = new Vector3(0,0,0);//the starting position of all ships
    float timerSeconds = 1;//time between de-spawns
    int fibonacciCounter = 0;//where are we in the fibonacci sequence

    int spawnedCounter = 0;//total number of ships shown

    bool addObjectsToPool(int n)
    {
        //this is to be used when there are no availiable inactive ships in the object pool
        //this funcion instantiates a new copy of the prefab, de-activates it and adds it to the object pool
        //this is a recursive function
        GameObject shipToAdd =Instantiate(shipPrefab);
        shipToAdd.SetActive(false);
        objectPool.Add(shipToAdd);
        n = n -1;
        if(n == 0){ return true;}
        else{return addObjectsToPool(n);}
        
       
    }
    void spawnObjects(int n)
    {
        //checks if there are enough objects in the inactive object pool (spoiler: there never are)
        //if not, it creates the amount needed in the first pass (this is also a recursive function)

        if(objectPool.Count < n)
        {
            addObjectsToPool(n - objectPool.Count);
        }
       if(n > 0)
       {
           //activates and sets the position and rotation of the ship object
            objectPool[0].SetActive(true);
            objectPool[0].GetComponent<Transform>().position = spawnPosition;
            objectPool[0].GetComponent<Transform>().rotation = Quaternion.Euler(n*-10,-90,90);
            //here we communicate with the ship code to set the speed
            objectPool[0].GetComponent<ShipCode>().SetSpeed(0.5f+(n*0.2f),0.5f);
            //add object to active objects list
            spawnedObjects.Add(objectPool[0]);
            //remove the object from the inactive objects list
            objectPool.RemoveAt(0);
            n = n -1;
            if(n > 0)
            {
                spawnObjects(n);
            }
       }
    }

    void DespawnObjects(int n)
    {
        //checks if there are actually any objects to de-spawn
        if(spawnedObjects.Count > 0)
        {
            //if there are, set it to inactive
            spawnedObjects[0].SetActive(false);
            //remove it from the active object pool and add it to the inactive object pool
            objectPool.Add(spawnedObjects[0]);
            spawnedObjects.RemoveAt(0);
            //this is also a recursive function
            n = n -1;
            if(n > 0)
            {
                DespawnObjects(n);
            }
        }
       
    }

    //returns the result from a certain position of the fibonacci sequence
    //also a recursive function
    int Fib(int n)
    {
        if(n <= 0)
        {
            return 0;
        }
        
        if(n == 1)
        {
           return 1;         
        }
        return Fib (n-1) + Fib (n-2);
    }


    // Start is called before the first frame update
    void Start()
    {

        //spawnObjects(Fib(fibonacciCounter));
        fibonacciCounter ++;
        //timerSeconds = 1;
    }

    // Update is called once per frame
    void Update()
    {
        //this is the timer for the de-spawning
        timerSeconds = timerSeconds - Time.deltaTime;
        //if it reaches 0, we de-spawn a object
        if(timerSeconds < 0)
        {
            DespawnObjects(1);
            if(spawnedObjects.Count == 0)
            {
                //if all objects are de-spawned, it's time for the new spawn wave
                
                spawnedCounter = spawnedCounter + Fib(fibonacciCounter);
                //number of ships is the fibonacci result of the current step

                spawnObjects(Fib(fibonacciCounter));
                //pass to the next step in the fibonacci sequence
                fibonacciCounter ++;
                
            }
            //reset timer
            timerSeconds = 1;
        }
        //sets camera further away depending on the quantity of ships (it kind of works)
        camera.transform.position = Vector3.Lerp(camera.transform.position,new Vector3(0,0,-10-Fib(fibonacciCounter)),Time.deltaTime);
        //sets the text to the textbox
        textBox.text = 
        "Active Objects: "+spawnedObjects.Count+"\n"+
        "Inactive Objects: "+objectPool.Count+"\n"+
        "Next Despawn: "+timerSeconds+"s\n"+
        "Fibonacci Step: "+fibonacciCounter+"\n"+
        "Fibonacci Step Result: "+Fib(fibonacciCounter)+"\n"+
        "Total Ships Shown: "+spawnedCounter+"\n"+
        "Total Ships Used: "+(spawnedObjects.Count+objectPool.Count)+"\n";

        
    }
}
