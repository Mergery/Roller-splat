using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody rb;
    public float speed= 15.0f;
    private bool isTravelling ;
    private Vector3 travelDirection;
    private Vector3 nextCollisionDirection;
    private AudioSource playerAudio;
    public AudioClip slideSound;
    

    public int minSwipeRecognition = 500; // dictates by how much the swipe should a user make to be considered as input 
    private Vector2 swipePosLastFrame ;
    private Vector2 swipePosCurrentFrame ;
    private Vector2 currentSwipe ;

    private Color solveColor;

    // Start is called before the first frame update
    private void Start()
    {
       solveColor= Random.ColorHSV(0.5f, 1);
       GetComponent<MeshRenderer>().material.color= solveColor; // 
       playerAudio= GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        if(isTravelling)
        {
            rb.velocity = speed* travelDirection;

        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position - (Vector3.up/2), 0.05f); 
        
        int i =0;
        while(i< hitColliders.Length)
        {
            GroundPiece ground =hitColliders[i].transform.GetComponent<GroundPiece>(); // if we collide with a tile store it in the ground variable 
            if (ground && !ground.isColored) // iif we collide with atile and the tile is not colored we should color the ground 
            {
                ground.ChangeColor(solveColor);
            }
            i++;
        }
         if(nextCollisionDirection != Vector3.zero) // we can only swipe the ball once it has reached its destination 
        {
            if(Vector3.Distance(transform.position, nextCollisionDirection)< 1)// is the 3d distance of the position of the ball and compare to next position collision should be less than 1
            {
                isTravelling= false;        
                travelDirection = Vector3.zero;
                nextCollisionDirection= Vector3.zero;
            }
        }

        if(isTravelling)
        {
            return;
        }
        if (Input.GetMouseButton(0)) // swipe mechanisimm
        {
            
            swipePosCurrentFrame = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            if(swipePosLastFrame != Vector2.zero)
            {
                currentSwipe= swipePosCurrentFrame- swipePosLastFrame;

                if(currentSwipe.sqrMagnitude < minSwipeRecognition) // creates the distance our swipe has travelled and compares it to see if its a valid swipe 
                {
                    return;
                }

                currentSwipe.Normalize();// we are lookig only to get the input ot the distance 

                // Up/Dow
                if(currentSwipe.x>-0.5f && currentSwipe.x< 0.5)
                {
                    // go up and down
                    playerAudio.PlayOneShot(slideSound,0.5f);
                    SetDestination(currentSwipe.y > 0? Vector3.forward : Vector3.back) ;// if the swipe goes up then go up if the swipee is down go down 
                }

                if(currentSwipe.y>-0.5f && currentSwipe.y< 0.5)
                {
                    // go left or right
                    playerAudio.PlayOneShot(slideSound,0.5f);
                    SetDestination(currentSwipe.x> 0? Vector3.right:Vector3.left);
                    // why are we using it interchangably  
                }
            }
            swipePosLastFrame = swipePosCurrentFrame; // so now the swipe we have made is now a past swipe 
        } 

        if(Input.GetMouseButtonUp(0))
        {
            swipePosLastFrame = Vector2.zero;
            currentSwipe= Vector2.zero;
        }
        
    }
    private void SetDestination(Vector3 direction)
    {
        travelDirection= direction;

        RaycastHit hit; // checks which object it will collide with 
        if(Physics.Raycast(transform.position , direction, out hit, 100f))
        {
            nextCollisionDirection = hit.point;

        }

        isTravelling = true; 
    }
}

