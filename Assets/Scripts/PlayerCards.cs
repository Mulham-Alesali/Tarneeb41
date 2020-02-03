using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerCards : MonoBehaviour
{
    public int playerId;
    public List<GameObject> cards;
    public Vector3 playerLocation;

    //عدد الاكول لكل لاعب
    public int tricks;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
   public void SortCards()
    {
        List<GameObject> sortedCards = new List<GameObject>();
        List<string> deck = TerneebGameController.GenerateDeck();

        foreach (string cardName in deck)
        {
         foreach(GameObject card in cards)
            {
                if (card.name == cardName)
                {
                    
                    sortedCards.Add(card);
                    //break;
                }
            }
        }

        int sortingLayer = 0;
        foreach(GameObject go in sortedCards)
        {
            go.GetComponent<SpriteRenderer>().sortingOrder = sortingLayer;
            
            sortingLayer++;
            
        }
        cards = sortedCards;
       
        
    }

   public IEnumerator LocateCards(bool movement)
    {
        int middleIndex = (cards.Count()) / 2;
        int i = 0;
        foreach(GameObject go in cards)
        {
            UpdateLocation ul = go.GetComponent<UpdateLocation>();
            Vector3 loc = ul.targetPosition;
                            
            loc.z = 0;  
           

            //go.GetComponent<SpriteRenderer>().sortingOrder = i;
           
            if (playerId % 2 == 0)
            {
                int sign = (cards.IndexOf(go) - middleIndex) > 0 ? 1: -1;
                loc.y = playerLocation.y + (cards.IndexOf(go) - middleIndex) * -0.04f * sign;
                loc.x = playerLocation.x + ((cards.IndexOf(go) - middleIndex) * 0.70f);

               
                if (playerId == 0)
                {

                    //loc.x = playerLocation.x + (cards.IndexOf(go) - middleIndex) * 0.70f;
                    ul.targetEulerAngles = new Vector3(0f, 0f, 0f - (cards.IndexOf(go) - middleIndex) * 1.5f);
                  //  print(loc.ToString());
                }
                else
                {
                    loc.x = playerLocation.x + (cards.IndexOf(go) - middleIndex) * 0.30f;
                    ul.targetEulerAngles = new Vector3(0f, 0f, 0f + (cards.IndexOf(go) - middleIndex) * 1.5f);
                }
            }
            else
            {
                loc.x = playerLocation.x;
                loc.y = playerLocation.y + (cards.IndexOf(go) - middleIndex) * 0.25f;

                if (playerId == 1)
                {
                ul.targetEulerAngles  = new Vector3(0f, 0f, 90f - (cards.IndexOf(go) - middleIndex) * 1.5f);
                }
                else
                {
                    ul.targetEulerAngles = new Vector3(0f, 0f, 90f + (cards.IndexOf(go) - middleIndex) * 1.5f);
                }
            }
            ul.targetPosition = loc;
            if (!movement)
            {
                ul.gameObject.transform.position = ul.targetPosition;
            }
            i++;
            if (movement)
            {
                yield return new WaitForSeconds(TerneebGameController.WAITINGTIME * 2);

            }
        }
   

    }
  
   public bool hasCard(GameObject card)
    {
        return cards.Contains(card);
    }

   public bool isPlayable(GameObject card)
   {
        
        // card.GetComponent<SpriteRenderer>().color = new Color(0xFF, 0xFF, 0xFF);
        RoundController rc = GameObject.Find("Tarneeb Game").GetComponent<RoundController>();
        //if it is the first round
        try
        {
            rc.roundCards[0].ToString();
        }
        catch (System.Exception)
        {
            return true;
        }

        //if it is played in the first round
        if(rc.roundCards[0].GetComponent<CardInfo>().Suite == card.GetComponent<CardInfo>().Suite)
        {
            return true;
        }

        //if the player has no cards from the suite in the first round
        foreach(GameObject go in cards)
        {
            if(go.GetComponent<CardInfo>().Suite == rc.roundCards[0].GetComponent<CardInfo>().Suite)
            {
                return false;
            }
        }
        return true;
   }
}
