using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
public class UpdateLocation : MonoBehaviour
{

    // Adjust the speed for the application.
    public float speed = 0.0001f;
    public float rotationSpeed = 0.1f;
    // The target (cylinder) position.
    public Vector3 targetPosition;
    public Vector3 targetEulerAngles;

    bool selected = false;
    Vector3 selecting = new Vector3(0f, 1f, 0f);

    private GameObject bottomPlayer;
    // Start is called before the first frame update
    void Start()
    {

        bottomPlayer = GameObject.Find("BottomPlayer");

    }

    // Update is called once per frame
    void Update()
    {
        // Move our position a step closer to the target.
        //float step = (float)(speed * (double)Time.deltaTime); // calculate distance to move
        float step = speed;
        float rotationStep = rotationSpeed;

        if (Vector3.Distance(transform.position, targetPosition) > 0.01)
        {
            AudioSource audioSource = gameObject.GetComponent<AudioSource>();
            if (!audioSource.isPlaying && Vector3.Distance(transform.position, targetPosition) > 2)
            {
            audioSource.Play();
            }

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
        }
        else
        {

        }


        //TODO test this code
        transform.eulerAngles = targetEulerAngles;
        //float direction = targetEulerAngles.z - transform.eulerAngles.z;
        //int sign = (direction > 0) ? 1 : -1;
        //transform.eulerAngles =  Vector3.RotateTowards(transform.eulerAngles, targetEulerAngles, 0f, 5f * sign);

        

        


    }
    
    public void SelectCard()
    {

        if (bottomPlayer.GetComponent<PlayerCards>().hasCard(transform.gameObject) & !selected)
        {
            targetPosition += selecting;
            selected = true;
            transform.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(3f, 7.156424f);
        }
    }

    public void DeselectCard()
    {
        if (bottomPlayer.GetComponent<PlayerCards>().hasCard(transform.gameObject) & selected)
        {
       if((targetPosition - selecting).y > -6)
        targetPosition -= selecting;
        selected = false;
        transform.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(1.94f, 7.156424f);
        }
    }

    public bool isReady()
    {
        bool ready = (Vector3.Distance(transform.position, targetPosition) <= 0.01);
        return ready;
    }

    public static bool isAllReady()
    {
        IEnumerable<UpdateLocation> ul; 
        while (true)
        {
        try
        {
            ul = (from UpdateLocation i in GameObject.FindObjectsOfType<UpdateLocation>()
                where !i.isReady()
                select i);
       
            break;
        }
        catch (InvalidOperationException)
        {

        }

        }

        if(ul.Count<UpdateLocation>() > 0)
        {
            return false;
        }

        return true;
    }
    
}
