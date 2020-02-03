using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerneebGameController : MonoBehaviour
{

    public static string[] suits = new string[] { "H", "C", "D", "S" };
    public static string[] values = new string[] { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };

    public Sprite[] cardFaces;
    public GameObject cardPrefab;
    public List<string> deck;
    
    public GameObject playerPrefab;
    public List<GameObject> players;
    public List<GameObject> allCards;

    public static float WAITINGTIME = 0.01f;

    public int[] scores;


    // Start is called before the first frame update
    void Start()
    {
       
        GameObject.Find("Card").SetActive(false);
        deck = GenerateDeck();
        TarneebDeal();

       // 

        InitPlayers();

        StartCoroutine(StartGame());
        

    }

    public IEnumerator StartGame()
    {
        
        scores = new int[] { 0, 0, 0, 0 };
        bool playing = true;
   
        while (playing)
        {

        
        StartCoroutine(GetComponent<SetController>().StartSet());
        yield return new WaitForSeconds(5);
        yield return new WaitUntil(() => GetComponent<SetController>().canStartSet);

        int winner = -1;
        
        for(int i = 0; i < 4; i++)
        {
          if(this.scores[i] >= 41)
          {
            winner = i;
          }

        }
        if(winner >= 0)
        {
                winGame(players[winner]);
                break;
        }

        //playing = false;
        }
        
    }

    private void winGame(GameObject winnerPlayer)
    {

        throw new NotImplementedException();
    }

    public static List<string> GenerateDeck()
    {
        List<string> newDeck = new List<string>();
        foreach (string s in suits)
        {
            foreach (string v in values)
            {
                newDeck.Add(s + v);
            }
        }
        return newDeck;
    }

    public void Shuffle<T>(List<T> list)
    {
        
        System.Random random = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            int k = random.Next(n);
            n--;
            T temp = list[k];
            list[k] = list[n];
            list[n] = temp;
        }
    }

    public void TarneebDeal()
    {
        float yOffset = 0;
        float zOffset = 0;
        float xOffset = 0;

        foreach(string card in deck)
        {
            
            Vector3 location = new Vector3(xOffset, yOffset, zOffset);
            yOffset = 0 + UnityEngine.Random.Range(-0.5f,0.5f);
            xOffset = 0 + UnityEngine.Random.Range(-0.5f, 0.5f);
         
        

            GameObject newCard = Instantiate(cardPrefab, location, Quaternion.Euler(0, 0, UnityEngine.Random.Range(-10f, 10f)));
            newCard.name = card;
            newCard.GetComponent<CardInfo>().UpdateCardInfo();
            allCards.Add(newCard);
            
            //yield return new WaitForSeconds(0.05f);

            // newCard.GetComponent<UpdateSprite>().faceUp = true;
        }

    }

    void InitPlayers()
    {
        string[] playerLocation = { "Bottom", "Right", "Top", "Left" };

        Vector3[] playerLocations = {
            new Vector3(0,-5f,0),
            new Vector3(9,0,0),
            new Vector3(0,5.15f,0),
            new Vector3(-9,0,0)
        };

        for(int i = 0; i < 4; i ++)
        {
           GameObject player = Instantiate(playerPrefab);
           player.name =  playerLocation[i] + "Player";
           PlayerCards pc = player.GetComponent<PlayerCards>();
           pc.playerLocation = playerLocations[i];
           // pc.SortCards();
           pc.playerId = i;
           players.Add(player);

        }

    }

    //distribute the cards for the Player
    public IEnumerator DealOutCards()
    {
        int index = 0;
        foreach(string card in deck)
        {
            GameObject foundCard = GameObject.Find(card);
            //mod 4 because we have 4 players and we should diestribute the cards for all players
            PlayerCards pc = players[index % 4].GetComponent<PlayerCards>();
            pc.cards.Add(foundCard);
            foundCard.GetComponent<UpdateLocation>().targetPosition = pc.playerLocation;
            yield return new WaitForSeconds(WAITINGTIME * 3);
            if(index % 4 == 0)
            {
                foundCard.GetComponent<UpdateSprite>().faceUp = true;
            }
            index++;
        }

        //locate the cards for each player
        foreach (GameObject go in players)
        {
            go.GetComponent<PlayerCards>().SortCards();
            StartCoroutine(go.GetComponent<PlayerCards>().LocateCards(true));
        }



    }

}
