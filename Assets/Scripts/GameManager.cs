using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UITimer _UITimer;
    [SerializeField] private Pinata _pinata;

    // Start is called before the first frame update
    void Start()
    {
        _UITimer.StartTimer(() => {
            _pinata.ShowPinata();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
