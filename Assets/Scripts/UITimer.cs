using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITimer : MonoBehaviour {
    [SerializeField] private Image back;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Pinata _pinata;


    private Action _callback;

    private void Start() {
        //StartTimer(); // TODO: remove
    }

    public void StartTimer(Action callback = null) {
        _callback = callback;
        StartCoroutine(StartTextCoroutine());
    }


    private IEnumerator StartTextCoroutine() {
        text.text = "60";
        back.fillAmount = 1;
        for (int i = 1; i <= 60; i++) {
            yield return new WaitForSeconds(1);
            text.text = (60 - i).ToString();
            back.DOFillAmount((60 - i) / (float) 60, 1);
            transform.DOScale(1.1f, 0.1f).OnComplete(() => {
                transform.DOScale(1, 0.2f);
            });
            if(i > 45)transform.DOShakePosition(0.5f, 20f, 20);
            if(i == 50) _pinata.ShowPinata();
        }
        
        yield return new WaitForSeconds(5f);
        _callback?.Invoke();
    }
}