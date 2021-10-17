using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    Vector3 cohesion = Vector3.zero;
    Vector3 align = Vector3.zero;
    Vector3 vecLeader = Vector3.zero;
    Vector3 separation = Vector3.zero;

    public FlockingManager myManager;
    float speed;
    Vector3 direction = Vector3.zero;

    public bool leader = false;

    private float freq = 0f;
    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        freq += Time.deltaTime;
        if (freq > 0.5)
        {
            freq -= 0.5f;
            ApplyRules();
        }
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,
                                      Quaternion.LookRotation(direction),
                                      myManager.rotationSpeed * Time.deltaTime);
        }

        transform.Translate(0, 0, Time.deltaTime * speed);
    }
    void ApplyRules()
    {
        if (!myManager.isLeader) FlockSwim();
        else FollowLeader();
    }

    private void FlockSwim()
    {
        cohesion = Vector3.zero;
        align = Vector3.zero;
        vecLeader = Vector3.zero;
        separation = Vector3.zero;

        float distance;
        int groupSize = 0;

        float distancePredator = Vector3.Distance(myManager.predator.transform.position, transform.position);
        if (distancePredator < 3)
        {
            direction = (transform.position - myManager.predator.transform.position).normalized * speed;
            direction *= 5;
        }
        else
        {
            foreach (GameObject go in myManager.allFish)
            {
                //Check that the object is different from the current one  
                if (go != this.gameObject)
                {
                    //Calculate the distance to the neighbor 
                    distance = Vector3.Distance(go.transform.position, transform.position);
                    //If the distance is less than the marked limit
                    if (distance <= myManager.neighbourDistance)
                    {
                        // Calcualte cohesion (the total amount of the positions)
                        cohesion += go.transform.position;

                        // Calculate align (the total amount of the directions)
                        align += go.GetComponent<Flock>().direction;

                        // Avoid crowding neighbours
                        separation -= (transform.position - go.transform.position) / (distance * distance);
                        // Increase group size
                        groupSize++;
                    }
                }
            }
            cohesion = (cohesion / groupSize - transform.position).normalized * speed;
            align /= groupSize;
            speed = Mathf.Clamp(align.magnitude, myManager.minSpeed, myManager.maxSpeed);
            // Combination for calculate new direction
            direction = (cohesion + align + separation + vecLeader).normalized * speed;
            Debug.Log(direction);
        }
    }

    // The leader has more influence over the other entities 
    void FollowLeader()
    {
        direction = ((myManager.allFish[myManager.GetLeader()].transform.position) - transform.position).normalized * speed;
    }

}