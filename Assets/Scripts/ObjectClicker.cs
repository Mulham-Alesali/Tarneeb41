using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectClicker : MonoBehaviour
{
    UpdateLocation ul;
    // Start is called before the first frame update
    void Start()
    {
        ul = GetComponent<UpdateLocation>();
    }

    private void Update()
    {
     
    }

    private void OnMouseUp()
    {
        GameObject humanPlayer = GameObject.Find("BottomPlayer");
        if (humanPlayer.GetComponent<PlayerCards>().isPlayable(transform.gameObject))
        {
            PlayerLogic playerLogic = humanPlayer.GetComponent<PlayerLogic>();
            GameObject thisCard = transform.gameObject;
            if (playerLogic.humanTurn)
            {
                playerLogic.PlayCard(thisCard);
                playerLogic.humanTurn = false;
                thisCard.GetComponent<AudioSource>().Play();
            }
        }
    }
    private void OnMouseEnter()
    {

        GameObject humanPlayer = GameObject.Find("BottomPlayer");

        if (humanPlayer.GetComponent<PlayerCards>().isPlayable(transform.gameObject))
        {
            PlayerLogic playerLogic = humanPlayer.GetComponent<PlayerLogic>();
            GameObject thisCard = transform.gameObject;
            if (playerLogic.humanTurn)
            {
                ul.SelectCard();
            }
        }
    }
    
    private void OnMouseExit()
    {
        GameObject humanPlayer = GameObject.Find("BottomPlayer");
        if (humanPlayer.GetComponent<PlayerCards>().isPlayable(transform.gameObject))
        {
            PlayerLogic playerLogic = humanPlayer.GetComponent<PlayerLogic>();
            GameObject thisCard = transform.gameObject;
                if (playerLogic.humanTurn)
                {
                    ul.DeselectCard();
                }
        }
    }
    
    private void OnMouseOver()
    {

    }


    private void OnMouseDown()
    {

  
    }

    private void PrintName(GameObject go)
    {
        print(go.name);
    }
}
