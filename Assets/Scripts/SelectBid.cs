using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectBid : MonoBehaviour
{
    SetController sc;

    public void Start()
    {
        sc = GameObject.Find("Tarneeb Game").GetComponent<SetController>();
    }

    public void ShowBidSelector()
    {
        sc.bidSelector.SetActive(true);
    }

    public void HideBidSelector()
    {
       sc.bidSelector.SetActive(false);
       
    }
    
    public void onClick(int bid)
    {
        //set the bid for the human player
        GameObject.Find("Tarneeb Game").GetComponent<SetController>().bid[0] = bid;
        HideBidSelector();
       //print(bid);
    }


}
