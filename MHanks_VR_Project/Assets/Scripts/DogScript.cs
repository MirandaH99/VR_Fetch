using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

enum DogStates
{
    Wait,
    Chase,
    Retrieve
}
public class DogScript : MonoBehaviour
{
    public GameObject ball;
    public GameObject grabPoint;

    BallScript bs;
    DogStates currentState;

    public NavMeshAgent agent;

    public float distance;
    public float HowCloseisClose = 1.0f;
    Vector3 startpos;

    public float closeToPlayer = 3.0f;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        agent = gameObject.GetComponentInParent<NavMeshAgent>();

        bs = ball.GetComponent<BallScript>();
        currentState = DogStates.Wait;
    }

    private void FixedUpdate()
    {
        switch (currentState)
        {
            case DogStates.Wait:
                DogWait();
                break;
            case DogStates.Chase:
                DogChase();
                break;
            case DogStates.Retrieve:
                DogRetrieve();
                break;
        }
    }

    public void DogWait()
    {
        if(bs.currentState == BallScript.ToyStates.Retrievable)
        {
            currentState = DogStates.Chase;
        }
    }

    public void DogChase()
    {

        distance = GetDistanceTo(ball);
        if (distance <= agent.speed * Time.deltaTime || distance <= HowCloseisClose)        //got close to toy
        {
            ball.GetComponent<Rigidbody>().useGravity = false;
            //ball.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            ball.GetComponent<Rigidbody>().isKinematic = true;

            ball.GetComponent<Collider>().isTrigger = true;

            ball.transform.position = grabPoint.transform.position;
            ball.transform.rotation = grabPoint.transform.rotation;
            ball.transform.parent = grabPoint.transform;

            currentState = DogStates.Retrieve;
        }
        else                    //haven't gotten close to toy yet
        {
            agent.isStopped = false;
            agent.SetDestination(ball.transform.position);
        }
    }

    public void DogRetrieve()
    {
        distance = GetDistanceTo(Player.Instance.gameObject);
        if (distance <= agent.speed * Time.deltaTime || distance <= closeToPlayer)        //got close to toy
        {
            ball.GetComponent<Rigidbody>().useGravity = true;
            //ball.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            ball.GetComponent<Rigidbody>().isKinematic = false;

            ball.GetComponent<Collider>().isTrigger = false;

            ball.transform.parent = null;

            bs.currentState = BallScript.ToyStates.Retrieved;
            agent.isStopped = true;

            currentState = DogStates.Wait;
        }
        else                    //haven't gotten close to toy yet
        {
            agent.SetDestination(Player.Instance.gameObject.transform.position);
        }
    }

    public float GetDistanceTo(Vector3 Other)
    {
        Vector3 DirectionVector = Other - transform.position;
        return DirectionVector.magnitude;
    }

    public float GetDistanceTo(GameObject Other)
    {
        return GetDistanceTo(Other.transform.position);
    }
}
