using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class RoundController : MonoBehaviour
{

    public GameObject[] roundCards;
    public SetController sc;
    public TerneebGameController tgc;

    public bool canStartRound = true;

    public Vector3[] tableCardsLocation;
    public Vector3[] tricksCardsLocation;
    int index = 0;
    int turn;

    // Start is called before the first frame update
    void Start()
    {
        tableCardsLocation = new Vector3[] {
        new Vector3(0f,-0.5f,0f),
        new Vector3(0.5f,0f,0f),
        new Vector3(0f,0.5f,0f),
        new Vector3(-0.5f,0f,0f)
        };

        tricksCardsLocation = new Vector3[] {
            new Vector3(0,-10f,0),
            new Vector3(15,0,0),
            new Vector3(0,10f,0),
            new Vector3(-15,0,0)
        };

        sc = GetComponent<SetController>();
        tgc = GetComponent<TerneebGameController>();

    }

    public IEnumerator StartRound()
    {
        canStartRound = false;
        roundCards = new GameObject[4];
        turn = sc.trickLeader;

        for (int i = 0; i < 4; i++)
        {

            StartCoroutine(tgc.players[turn].GetComponent<PlayerLogic>().Play());
            if(turn == 0)
            {
                yield return new WaitUntil(() => (!tgc.players[turn].GetComponent<PlayerLogic>().humanTurn));
            }
            yield return new WaitForSeconds(1f);
            turn++;
            turn %= 4;
        }





    }

    //لطش
    //take trick and decide which player take it 
    public IEnumerator WinTrick()
    {

        GameObject winCard;
        int winCardIndex = sc.trickLeader;
        winCard = roundCards[winCardIndex];


        for (int i = 0; i < 4; i++)
        {

            //wenn die karte tarneeb ist 
            if (roundCards[i].GetComponent<CardInfo>().Suite == sc.trump && winCard.GetComponent<CardInfo>().Suite != sc.trump)
            {
                winCard = roundCards[i];
                continue;
            }

            if (roundCards[i].GetComponent<CardInfo>().Suite == winCard.GetComponent<CardInfo>().Suite
                & roundCards[i].GetComponent<CardInfo>().cardRanking > winCard.GetComponent<CardInfo>().cardRanking)
            {
                winCard = roundCards[i];
            }

        }

        //the winner is the owner of the win card

        yield return new WaitForSeconds(1);

        int indexOfWinner = (Array.IndexOf(roundCards, winCard) + sc.trickLeader) % 4;
        //if (indexOfWinner < 0) indexOfWinner += 4;

        //print("winCard: " + winCard.name);
        //print("trickLeader" + sc.trickLeader);
        //print("turn" + turn);
        //print("index" + index);
        //print("index of winner" + indexOfWinner);
        //foreach(GameObject g in roundCards)
        //{
        //    print("roundCard: " + g.name);
        //}


        sc.trickLeader = indexOfWinner;
        for (int i = 0; i < 4; i++)
        {
            roundCards[i].GetComponent<UpdateLocation>().targetPosition = tricksCardsLocation[indexOfWinner];
           
            //adding the played card to the list
            sc.playedCards.Add(roundCards[i]);
        }
        

        //adding 1 for the trick taker
        sc.tricks[indexOfWinner]++;
        refreshTricks();
        yield return new WaitForSeconds(1);

        canStartRound = true;
        //return tgc.players[indexOfWinner];

       // Debug.Log("Can not find a Winnder");
        //throw new System.Exception("Can not find a Winnder");


    }

    //wenn ein spieler eine Karte auf dem Tisch liegt
    public void PlayCard(GameObject card)
    {

        roundCards[index] = card;
        card.GetComponent<SpriteRenderer>().sortingOrder = index;
        index++;
        if (index > 3)
        {
            index %= 4;
            StartCoroutine(WinTrick());
        }
    }


    private void refreshTricks()
    {
        List<GameObject> tricks = new List<GameObject>();
        tricks.Add(GameObject.Find("Trick0"));
        tricks.Add(GameObject.Find("Trick1"));
        tricks.Add(GameObject.Find("Trick2"));
        tricks.Add(GameObject.Find("Trick3"));
        for(int i =0; i<4;i++)
        {
            tricks[i].GetComponent<Text>().text = sc.tricks[i].ToString();
        }

    }
}
