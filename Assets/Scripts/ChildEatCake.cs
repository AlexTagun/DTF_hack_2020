using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildEatCake : MonoBehaviour
{
    public bool IsEatingCake = false;
    public bool CanEatCake = true;
    public bool HaveCakeSpot = false;
    [SerializeField] private float _timeEatingCake = 5f;
    public BirthdayСake BirthdayСake = null;
    [SerializeField] private GameObject _creamSpot = null;
    [SerializeField] private ChildPlayerAnimation _childPlayerAnimation = null;
    [SerializeField] private AudioSource _audioSource = null;
    private float _curTimeEatingCake = 0f;

    void Start()
    {
        _creamSpot.SetActive(false);
        BirthdayСake = GameObject.Find("birthdayСake").GetComponent<BirthdayСake>();
    }

    void Update()
    {
        UpdateEatingCake();
    }

    public void EatCake()
    {
        IsEatingCake = true;
        BirthdayСake.IsFreeToEat = false;
        _childPlayerAnimation.PlayAnimEat();
        _audioSource.Play();
        //Debug.Log("Хрум-хрум");
    }

    public void StopEatingCake()
    {
        IsEatingCake = false;
        CanEatCake = true;
        BirthdayСake.IsFreeToEat = true;
        _curTimeEatingCake = 0f;
        _audioSource.Stop();
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
                BirthdayСake.AmountPieceCake--;
                BirthdayСake.IsFreeToEat = true;
                GetComponent<ChildAI>().pauseAI = false;
                _creamSpot.SetActive(true);
                BirthdayСake.ChangeSprite();
                _childPlayerAnimation.PlayAnimIdle();
                _audioSource.Stop();
            }
            else
            {
                _curTimeEatingCake += Time.deltaTime;
            }
        }
    }
}
