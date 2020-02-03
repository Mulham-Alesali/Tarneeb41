using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SetController : MonoBehaviour
{
    public List<GameObject> playedCards = new List<GameObject>();

    public TerneebGameController tgc;
    // public GameObject[] players;
    public int trickLeader;

    //الضمان
    //bid index ist die spieler nummer
    public int[] bid;
    
    //الاكول
    public int[] tricks;
    public string trump;

    //لون اللعبة
    public string roundSuite;

    bool isReady = false;

    public GameObject bidSelector;
    public bool canStartSet = false;


    // Start is called before the first frame update
    void Start()
    {

     
        bidSelector = GameObject.Find("BidSelectorCanvas");
        bidSelector.SetActive(false);

        bid = new int[] { 0, 0, 0, 0 };

        tgc = GetComponent<TerneebGameController>();
        //players = GetComponent<TerneebGameController>().players;
        isReady = true;


        int trumpRandom = Random.Range(0, 3);
        trump = TerneebGameController.suits[trumpRandom];


    }
    //start game
    //start set
    //start round
    
    public IEnumerator StartSet()
    {
        yield return new WaitForSeconds(1);

        foreach (GameObject o in tgc.allCards)
        {
            o.GetComponent<UpdateSprite>().faceUp = false;
        }

        tgc.Shuffle<string>(tgc.deck);
        foreach(GameObject card in tgc.allCards)
        {
            UpdateLocation ul = card.GetComponent<UpdateLocation>();
            float yOffset = 0;
            float zOffset = 0;
            float xOffset = 0;
            yOffset = 0 + Random.Range(-0.1f, 0.1f);
            xOffset = 0 + Random.Range(-0.1f, 0.1f);
    
            Vector3 location = new Vector3(xOffset, yOffset, zOffset);

            ul.targetPosition = location;
            ul.transform.position = ul.targetPosition;
        }

        canStartSet = false;
        bid = new int[] { 0, 0, 0, 0 };
        tricks = new int[] { 0, 0, 0, 0 };

        yield return new WaitUntil(()=>isReady);
        yield return new WaitForSeconds(1);

        bid = new int[] { 0, 0, 0, 0 };

        int turn = trickLeader;
        tgc = GetComponent<TerneebGameController>();

        StartCoroutine(tgc.DealOutCards());

        yield return new WaitUntil(() =>UpdateLocation.isAllReady());

        yield return new WaitForSeconds(0f);
        
        //foreach(GameObject p in tgc.players)
        //{
        //    p.GetComponent<PlayerCards>().LocateCards();
        //}
        for (int i = 0; i < 4; i++)
        {
            if(turn == 0)
            {

                bidSelector.SetActive(true);
                yield return new WaitUntil(() => bid[0] > 0);
            }
            else
            {
            bid[turn] = tgc.players[turn].GetComponent<PlayerLogic>().GetBid();
            
            }
   
            turn++; 
            turn %= 4;
        }
        
        for(int i = 0;i < 13; i++)
        {
         
            RoundController rc = GetComponent<RoundController>();
            StartCoroutine(rc.StartRound());
            yield return new WaitWhile(()=> !rc.canStartRound);
        }

        CountScores();
     //   print("new set");
        canStartSet = true;
    }

    //this methode should be call after each set
    public void CountScores()
    {
        trickLeader++;
        trickLeader %= 4;
       

        //calculate the scores after each set
        for (int i = 0; i < 4; i++)
        {
            if (tricks[i] >= bid[i])
            {
                tgc.scores[i] += bid[i];
            }
        }


        //hier mussen scores aus dem round gerechnet

    }

}
