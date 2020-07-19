using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirthdayСake : MonoBehaviour
{
    public int AmountPieceCake = 5;
    public bool IsFreeToEat = true;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (AmountPieceCake == 0) return;
        if (!IsFreeToEat) return;
        if (collision.tag != "Child") return;
        if (collision.GetComponent<DragDropChild>().IsDraging) return;
        if (collision.GetComponent<ChildEatCake>().HaveCakeSpot) return;
        if (collision.GetComponent<ChildEatCake>().CanEatCake)
        {
            collision.GetComponent<ChildEatCake>().EatCake();
        }


    }
}
