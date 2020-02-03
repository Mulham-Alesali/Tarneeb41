using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInfo : MonoBehaviour
{

    public int cardRanking;
    public string Suite;
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void UpdateCardInfo()
    {
        string name = transform.gameObject.name;
        Suite = name[0].ToString();

        string rank = name.Replace(Suite,"");
        rank = rank.Replace("J", "11");
        rank = rank.Replace("Q", "12");
        rank = rank.Replace("K", "13");
        rank = rank.Replace("A", "14");

        cardRanking = int.Parse(rank) - 2;

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
