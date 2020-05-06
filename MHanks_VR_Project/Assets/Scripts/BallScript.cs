using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BallScript : GrabbableObject
{
    public enum ToyStates
    {
        Thrown,
        Retrievable,
        Retrieved,
        Held
    }

    public ToyStates currentState;
    public Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        //dog won't come after ball
        currentState = ToyStates.Retrieved;
        startPosition = gameObject.transform.position;
    }

    void FixedUpdate()
    {
        switch (currentState)
        {
            case ToyStates.Retrieved:
                RetrievedBall();
                break;
            case ToyStates.Thrown:
                ThrownBall();
                break;
            case ToyStates.Retrievable:
                RetrievableBall();
                break;
            case ToyStates.Held:
                HeldBall();
                break;
        }
    }

    public void RetrievedBall()     //Dog will not chase ball
    {
        Debug.Log("I came back!");
    }

    public void ThrownBall()    //Ball is thrown, but dog will not chase ball (dog may look at ball if I have time to add that)
    {
        Debug.Log("I'm being thrown!");
    }

    public void RetrievableBall()   //Ball is retrievable, dog gets ball
    {
        Debug.Log("Come get me, doggy!");
    }

    public void HeldBall()  //Ball is being held, Dog doesn't chase Ball
    {
        Debug.Log("I'm being held!");
    }
}


