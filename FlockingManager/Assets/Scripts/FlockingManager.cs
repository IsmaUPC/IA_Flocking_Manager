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
    public bool isLeader = false;
    public Quaternion directionLeader = Quaternion.identity;
    int num = 0;
    public int GetLeader() { return num; }

    [Header("Fish Settings")]
    [Range(0.0f, 5.0f)] public float minSpeed;

    [Range(0.0f, 5.0f)] public float maxSpeed;

    [Range(1.0f, 10.0f)] public float neighbourDistance;

    [Range(0.0f, 5.0f)] public float rotationSpeed;
    private float freq = 0f;

    void Start()
    {
        Vector3 pos;
        allFish = new GameObject[numFish];
        for (int i = 0; i < numFish; i++)
        {
            pos = this.transform.position +
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
        if (freq > 5)
        {
            freq -= 5;
            if (isLeader)
            {
                NewLeader();
                freq -= 5;
            }
            else
            {
                DeleteLeader();
            }

            isLeader = !isLeader;
        }
    }
    void NewLeader()
    {
        num = Random.Range(0, numFish);
        DeleteLeader();

        allFish[num].GetComponent<Flock>().leader = true;
        directionLeader = Random.rotation;
        Debug.Log("I am a Leader");
    }

    void DeleteLeader()
    {
        allFish[num].GetComponent<Flock>().leader = false;
    }

}