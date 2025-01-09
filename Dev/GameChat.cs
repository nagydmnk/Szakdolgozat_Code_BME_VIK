using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameChat : MonoBehaviour
{
    public TMP_Text _messages;

    public void SendMessage(string from, string to, string message)
    {
        _messages.text += $"<color=#00ff00>{from} -> {to}</color> : {message}\n";
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
