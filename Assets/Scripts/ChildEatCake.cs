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
    [SerializeField] private GameObject _creamSpot = null;
    [SerializeField] private ChildPlayerAnimation _childPlayerAnimation = null;
    private float _curTimeEatingCake = 0f;

    void Start()
    {
        _creamSpot.SetActive(false);
        _birthdayСake = GameObject.Find("birthdayСake").GetComponent<BirthdayСake>();
    }

    void Update()
    {
        UpdateEatingCake();
    }

    public void EatCake()
    {
        IsEatingCake = true;
        _birthdayСake.IsFreeToEat = false;
        _childPlayerAnimation.PlayAnimEat();
        //Debug.Log("Хрум-хрум");
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
            if (_curTimeEatingCake>=_timeEatingCake)
            {
                IsEatingCake = false;
                HaveCakeSpot = true;
                //Debug.Log("Кусок съеден");
                _birthdayСake.AmountPieceCake--;
                _birthdayСake.IsFreeToEat = true;
                GetComponent<ChildAI>().pauseAI = false;
                _creamSpot.SetActive(true);
                _birthdayСake.ChangeSprite();
                _childPlayerAnimation.PlayAnimIdle();
            }
            else
            {
                _curTimeEatingCake += Time.deltaTime;
            }
        }
    }
}
