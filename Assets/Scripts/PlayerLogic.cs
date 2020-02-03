using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerLogic : MonoBehaviour
{



    GameObject tgc;
    public bool humanTurn = false;
    
    // Start is called before the first frame update
    void Start()
    {
        tgc = GameObject.Find("Tarneeb Game");
    }

    public void PlayCard(GameObject card)
    {
        PlayerCards playerCards = GetComponent<PlayerCards>();
        RoundController roundController = tgc.GetComponent<RoundController>();

        card.GetComponent<UpdateLocation>().targetPosition = roundController.tableCardsLocation[playerCards.playerId];

        playerCards.cards.Remove(card);
        roundController.PlayCard(card);
        card.GetComponent<UpdateSprite>().faceUp = true;
        
        StartCoroutine(playerCards.LocateCards(false));



    }

    public IEnumerator Play()
    {
       if(transform.gameObject.name == "BottomPlayer")
        {
            //TODO wait until the human player play
            humanTurn = true;
            //  PlayCard(GetBestCard());
            // print("BEFORE WAITING FOR PLAYER INPUT");
            foreach (GameObject go in GetComponent<PlayerCards>().cards)
            {
                go.GetComponent<SpriteRenderer>().color = (GetComponent<PlayerCards>().isPlayable(go)) ? Color.white: Color.gray ;
            }
            yield return new WaitUntil(() => (!humanTurn));
            foreach(GameObject go in GetComponent<PlayerCards>().cards)
            {
                go.GetComponent<SpriteRenderer>().color = Color.gray;
            }
        }
        else
        {
            PlayCard(GetBestCard());
        }
     
    }


    private GameObject GetBestCard()
    {

        /*
         * كيفية الحصول على افضل ورقة للعب
         * يجب اولا النظر اذا كانت الورقة ممكنة للعب او لا في هذه الحال يجب التقليل من قيمة المتغير المسئول عن تقييم الورقة
         * وجعله سالب
         * الاوراق من لون معين التي لا بوجد عدد كبير منها يجب ان تعطى تقييم عالي للعب للتخلص منها
         * المرحلة التي تليها يجب لعب الاوراق التي يضمن الفوز فيها الى حد معين
         * 
         * 
         */

        List<GameObject> playerCards = GetComponent<PlayerCards>().cards;
        GameObject[] roundCards = tgc.GetComponent<RoundController>().roundCards;

        int[] playerCardsRanking = new int[playerCards.Count()];


        //        يجب اولا النظر اذا كانت الورقة ممكنة للعب او لا في هذه الحال يجب التقليل من قيمة المتغير المسئول عن تقييم الورقة
        //         وجعله سالب  
        if (roundCards[0] != null)
        {
            for (int i = 0; i < playerCards.Count; i++)
            {
                if (playerCards[i].GetComponent<CardInfo>().Suite != roundCards[0].GetComponent<CardInfo>().Suite)
                {
                    playerCardsRanking[i] -= 100;
                }
            }
        }

        //ponus weil es wenig karten von einem Suite gibt
        int[] ponus = { 0, 0, 0, 0 };
        string[] suites = TerneebGameController.suits;
        int[] numberOfSuitecards = { 0, 0, 0, 0 };

        for (int i = 0; i < 4; i++)
        {
            numberOfSuitecards[i] = (from c in playerCards where c.GetComponent<CardInfo>().Suite == suites[i] select c).Count();
            ponus[i] = 3 - numberOfSuitecards[i];
            ponus[i] = (ponus[i] > 0) ? ponus[0] : 0;
        }

        //hier werden die Karten, wovon der Spieler weniger hat besser bewertet als die Anderen
        for (int i = 0; i < playerCards.Count; i++)
        {
            playerCardsRanking[i] += (ponus[Array.IndexOf(suites, playerCards[i].GetComponent<CardInfo>().Suite)] * 10);
        }

        //هنا يجب النظر هل الورقة التي باليد هي اكبر ورقة من لون معين لم يتم لعبها
        for (int i = 0; i < playerCards.Count; i++)
        {
            int cardSetRanking;
            CardInfo playerCardInfo = playerCards[i].GetComponent<CardInfo>();
            cardSetRanking = playerCardInfo.cardRanking;
            if (roundCards[0] != null)
            {
                if (playerCardInfo.Suite != roundCards[0].GetComponent<CardInfo>().Suite)
                {
                    continue;
                }
            }
            foreach (GameObject playedCard in GameObject.Find("Tarneeb Game").GetComponent<SetController>().playedCards)
            {
                CardInfo playedCardInfo = playedCard.GetComponent<CardInfo>();

                if (playedCardInfo.Suite == playerCardInfo.Suite)
                {
                    if (playerCardInfo.cardRanking < playedCardInfo.cardRanking)
                    {
                        cardSetRanking++;
                    }

                }
            }
            if (cardSetRanking == 12)
            {
                playerCardsRanking[i] += 5;
            }


        }


        GameObject roundStrongestCard = getStrongestCard(roundCards);
        if (roundStrongestCard != null)
        {
            for (int i = 0; i < playerCards.Count; i++)
            {
                if (
                    playerCards[i].GetComponent<CardInfo>().cardRanking > roundStrongestCard.GetComponent<CardInfo>().cardRanking
                &
                    playerCards[i].GetComponent<CardInfo>().cardRanking < 10
                )
                {
                    playerCardsRanking[i] += 2;
                }
            }
        }




            //return the card with the highest ranking
            GameObject result = playerCards[0];
        int indexOfResult = 0;
        for (int i =1;i < playerCards.Count; i++)
        {
            if(playerCardsRanking[indexOfResult] < playerCardsRanking[i])
            {
                result = playerCards[i];
                indexOfResult = i;
            }
        }

        return result;
    }


    private GameObject getStrongestCard(GameObject[] roundCards)
    {
        SetController sc = tgc.GetComponent<SetController>();

        GameObject roundStrongestCard = roundCards[0];
        for (int i = 0; i < roundCards.Length; i++)
        {
            if (roundCards[i] != null)
            {
                if (i != 0)
                {
                    if (roundCards[i].GetComponent<CardInfo>().Suite == sc.trump && roundStrongestCard.GetComponent<CardInfo>().Suite != sc.trump)
                    {
                        roundStrongestCard = roundCards[i];
                        continue;
                    }
                    if (roundCards[i].GetComponent<CardInfo>().Suite == roundStrongestCard.GetComponent<CardInfo>().Suite
                     & roundCards[i].GetComponent<CardInfo>().cardRanking > roundStrongestCard.GetComponent<CardInfo>().cardRanking)
                    {
                        roundStrongestCard = roundCards[i];
                    }
                }
                else
                {
                    break;
                }
            }
        }
        
            return roundStrongestCard;
        }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal int GetBid()
    {
        int result = 0;

        List<GameObject> cards = GetComponent<PlayerCards>().cards;
        string[] suites = TerneebGameController.suits;
        string trumpSuite = GameObject.Find("Tarneeb Game").GetComponent<SetController>().trump;

        int numberOfTarneeb = 0;
        int numberOfTahreeb = 0;

        IEnumerable<GameObject>[] cardsSuites = new IEnumerable<GameObject>[5];
        int trumpCardsSuitesIndex = 0;


        //save cardsSuites
        for (int i = 0; i < 4; i++)
        {
            if (suites[i] == trumpSuite)
            {
                trumpCardsSuitesIndex = i;
            }

            List<GameObject> temp = new List<GameObject>();
            foreach (GameObject go in cards)
            {
                if(go.GetComponent<CardInfo>().Suite == suites[i])
                {
                    temp.Add(go);
                }
            }
            cardsSuites[i] = (IEnumerable<GameObject>)temp;
        }

        //count the tarneeb cards
        numberOfTarneeb = cardsSuites[trumpCardsSuitesIndex].Count();

        //calculate the number of possible tahreeb for tarneeb
        for(int i = 0; i < 4; i++)
        {
            if (suites[i] == trumpSuite)
            {
                continue;
            }

            int temp = 2 - cardsSuites.Count();
            temp = (temp >= 0) ? temp : 0;
            numberOfTahreeb += temp;
        }


        //calculate the high cards for all suites except the tarneeb suite
        for(int i = 0; i < 4; i++)
        {
            if (suites[i] != trumpSuite)
            {
                int temp = (from c in cardsSuites[i] where c.GetComponent<CardInfo>().cardRanking > 9 select c).Count();
             
                result += temp;
            }
            else
            {
                int tarneebHighCount = (from c in cardsSuites[i] where c.GetComponent<CardInfo>().cardRanking > 8 select c).Count();
                result += tarneebHighCount;
                int tarneebLowCount = (from c in cardsSuites[i] where c.GetComponent<CardInfo>().cardRanking <= 8 select c).Count();
                int tarneebLowWins = (tarneebLowCount > numberOfTahreeb) ? numberOfTahreeb : tarneebLowCount;
                result += tarneebLowWins;
            }

        }

       if(result < 2)
        {
            return 2;
        }
        else
        {
            return result;
        } 
    }

}
