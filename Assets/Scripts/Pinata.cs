using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pinata : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem = null;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ShowPinata()
    {
        gameObject.SetActive(true);
    }
    private void OnMouseDowm()
    {
        Debug.Log("Клик");
        _particleSystem.Emit(20);
    }
}
