using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateSprite : MonoBehaviour
{

    public Sprite cardFace;
    public Sprite cardBack;
    public bool faceUp = false;


    TerneebGameController tarneeb;
    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        List<string> deck = TerneebGameController.GenerateDeck();
        tarneeb = FindObjectOfType<TerneebGameController>();
        int i = 0;
        foreach(string card in deck)
        {
            if(this.name == card)
            {
                cardFace = tarneeb.cardFaces[i];
                break;
            }
            i++;
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (faceUp)
        {
            spriteRenderer.sprite = cardFace;
        }
        else
        {
            spriteRenderer.sprite = cardBack;
        }
    }
}
