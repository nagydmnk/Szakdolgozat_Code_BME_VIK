     using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour, IObservable
{
    [SerializeField] private MessageManager messageManager;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button sendButton;

    private List<IObserver> observers = new List<IObserver>();


    void Awake()
    {
        messageManager = Camera.main.GetComponent<MessageManager>();
    }

    void StartGame()
    {
        
    }


    // Start is called before the first frame update
    void Start()
    {
        sendButton.onClick.AddListener(OnSendButtonClicked);
    }

    void OnSendButtonClicked()
    {
        string command = inputField.text;
        ProcessCommand(command);
    }

    void ProcessCommand(string command)
    {
        // Notify the MessageManager to handle the command
        messageManager.HandleCommand(command);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void addObserver(IObserver obs)
    {
        observers.Add(obs);
    }

    public void notifyObservers()
    {
        foreach (IObserver obs in observers)
        {
            obs.update();
        }
    }

    public void removeObserver(IObserver obs)
    {
        observers.Remove(obs);
    }
}
