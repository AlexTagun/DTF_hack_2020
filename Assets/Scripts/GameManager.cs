using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private UITimer _UITimer;
    [SerializeField] private Pinata _pinata;
    [SerializeField] GamePhoto _resultPhoto = null;
    [SerializeField] ResultPhotoUIObject _resultUI = null;

    void Start()
    {
        _UITimer.StartTimer(() => {
            _pinata.ShowPinata();
            _resultUI.show(_resultPhoto.bestPhotoTexture, _resultPhoto.bestPhotoScores);
        });
    }
}
