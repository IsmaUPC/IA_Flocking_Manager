using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingManager : MonoBehaviour
{
    public GameObject fishPrefab;
    public GameObject predator;
    public int numFish = 20;
    public GameObject[] allFish;
    public Vector3 swimLimits = new Vector3(5, 5, 5);

    [Header("Fish Settings")]
    [Range(0.0f, 5.0f)]
    public float minSpeed;
    [Range(0.0f, 5.0f)]
    public float maxSpeed;

    [Range(1.0f, 10.0f)]
    public float neighbourDistance;
    [Range(0.0f, 5.0f)]
    public float rotationSpeed;
    private float freq = 0f;

    void Start () {

        allFish = new GameObject[numFish];
        for (int i = 0; i < numFish; i++)
        {
            Vector3 pos = this.transform.position +
                new Vector3(Random.Range(-swimLimits.x, swimLimits.x),
                Random.Range(-swimLimits.y, swimLimits.y),
                Random.Range(-swimLimits.z, swimLimits.z));
            allFish[i] = (GameObject)Instantiate(fishPrefab, pos, Quaternion.identity);
            allFish[i].GetComponent<Flock>().myManager = this;
        }

    }

    // Update is called once per frame
    void Update()
    {
        // Assign a new leader
        freq += Time.deltaTime;
        if (freq > 0.5f)
        {
            freq -= 0.5f;
            NewLeader();
        }
    }
    void NewLeader()
    {
        int num = Random.Range(0, numFish);
        for (int i = 0; i < numFish; i++)
        {
            allFish[i].GetComponent<Flock>().leader = false;
        }
        allFish[num].GetComponent<Flock>().leader = true;
        allFish[num].GetComponent<Flock>().directionLeader = Random.rotation;
        Debug.Log("I am a Leader");
    }
}