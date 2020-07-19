using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildEatCake : MonoBehaviour
{
    public bool IsEatingCake = false;
    public bool CanEatCake = true;
    public bool HaveCakeSpot = false;
    [SerializeField] private float _timeEatingCake = 5f;
    [SerializeField] private BirthdayСake _birthdayСake = null;
    private float _curTimeEatingCake = 0f;
    // Start is called before the first frame update
    void Start()
    {
        _birthdayСake = GameObject.Find("birthdayСake").GetComponent<BirthdayСake>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEatingCake();
    }

    public void EatCake()
    {
        IsEatingCake = true;
        _birthdayСake.IsFreeToEat = false;
        Debug.Log("Хрум-хрум");
    }

    public void StopEatingCake()
    {
        IsEatingCake = false;
        CanEatCake = true;
        _birthdayСake.IsFreeToEat = true;
        _curTimeEatingCake = 0f;
    }

    private void UpdateEatingCake()
    {
        if(IsEatingCake)
        {
            if(_curTimeEatingCake>=_timeEatingCake)
            {
                IsEatingCake = false;
                HaveCakeSpot = true;
                Debug.Log("Кусок съеден");
                _birthdayСake.AmountPieceCake--;
                _birthdayСake.IsFreeToEat = true;

            }
            else
            {
                _curTimeEatingCake += Time.deltaTime;
            }
        }
    }
}
