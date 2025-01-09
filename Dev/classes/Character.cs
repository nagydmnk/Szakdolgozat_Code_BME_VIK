using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class Character : MonoBehaviour, ICommunication
{


    public WorldManager WorldManager;
    



    private string Name { get; set; }
    private Vector3 Position { get; set; }
    private CharacterState State;
    public AIController Controller;
    public NavMeshAgent agent;
    public GameObject target;
    public GameChat _chat;

    private Queue<string> commandQueue = new Queue<string>();
    private bool isExecutingCommand = false;

    public MoveableItem carriedItem = null;



    // Start is called before the first frame update
    void Start()
    {
        

        agent = GetComponent<NavMeshAgent>();
        MessageManager messageManager = Camera.main.GetComponent<MessageManager>();
        Controller = new AIController(messageManager, this);
        messageManager.RegisterCharacter(this);
        

    }

    public void AddCommandsToQueue(List<string> commands)
    {
        foreach (string command in commands)
        {
            commandQueue.Enqueue(command);
        }
        if (!isExecutingCommand)
        {
            ExecuteNextCommand();
        }
    }

    //####################################
    //Kommandok feldolgozása és kezelése #
    //####################################

    public void ExecuteNextCommand()
    {
        if (commandQueue.Count == 0)
        {
            Debug.LogWarning("No commands to execute.");
            return;
        }

        string command = commandQueue.Dequeue();
        Debug.Log($"Comm: {command}");

    
        string[] commandParts = command.Split(' ', 3);

        for (int i = 0; i < commandParts.Length; i++)
        {
            commandParts[i] = commandParts[i].Trim();
        }

        if (commandParts.Length < 2)
        {
            HandleSingleWordCommand(commandParts[0]);
            return;
        }

        string action = commandParts[0].ToLower();
        string targetName = commandParts[1];
        string message = commandParts.Length == 3 ? commandParts[2] : string.Empty; 

        switch (action)
        {
            case "move":
                move(targetName);
                break;
            case "pick":
                move(targetName);
                pick(targetName);
                break;
            case "ask":
                move(targetName);
                ask(targetName);
                break;
            case "give":
                move(targetName);
                give(targetName);
                break;
            default:
                Debug.LogWarning($"Unknown command: {command}");
                break;
        }
    }


    //Egyszavas kommandok kezelése
    private void HandleSingleWordCommand(string command)
    {
        if (command.Equals("putdown", StringComparison.OrdinalIgnoreCase))
        {
            if (carriedItem != null)
            {
                carriedItem.putDown();
                carriedItem = null;
                //Debug.Log("Character put down the item.");
            }
            else
            {
                //Debug.LogWarning("No item to put down.");
            }
        }
        else
        {
            Debug.LogWarning($"Unknown command: {command}");
        }
    }

    //#############################
    //Tevékenységek megvalósítása #
    //#############################

    //Mozgás egy GameObject-hez
    public void move(string targetName)
    {

        if (string.IsNullOrWhiteSpace(targetName))
        {
            //Debug.LogWarning("Target name is null or empty.");
            return;
        }

        GameObject targetObject = GameObject.Find(targetName);
        if (targetObject == null)
        {
            //Debug.LogWarning($"Target object '{targetName}' not found.");
            return;
        }

        target = targetObject;

        if (agent == null)
        {
            //Debug.LogError("NavMeshAgent component is missing on the character.");
            return;
        }

        agent.SetDestination(target.transform.position);
        isExecutingCommand = true;

        Debug.Log($"Character is moving to: {targetName}");
    }

    //MoveableItem felvétele
    public void pick(string targetName)
    {
        if (string.IsNullOrWhiteSpace(targetName))
        {
           // Debug.LogWarning("Target name is null or empty.");
            return;
        }

        GameObject targetObject = GameObject.Find(targetName);
        if (targetObject == null)
        {
            //Debug.LogWarning($"Target object '{targetName}' not found.");
            return;
        }

        MoveableItem item = targetObject.GetComponent<MoveableItem>();
        if (item == null)
        {
            //Debug.LogWarning($"Target object '{targetName}' is not a MoveableItem.");
            return;
        }

        item.StartPickupProcess(this);
        carriedItem = item;
        //Debug.Log($"Pickup process started for: {targetName}");
    }

    //InteractebleObject megkérdezse a befogadóképességrõl
    public void ask(string targetName)
    {
        if (string.IsNullOrWhiteSpace(targetName))
        {
            //Debug.LogWarning("Target name is null or empty.");
            return;
        }

        GameObject targetObject = GameObject.Find(targetName);
        if (targetObject == null)
        {
           // Debug.LogWarning($"Target object '{targetName}' not found.");
            return;
        }

        InteractableObject interactObject = targetObject.GetComponent<InteractableObject>();
        if (interactObject == null)
        {
            //Debug.LogWarning($"Target object '{targetName}' is not an InteractableObject.");
            return;
        }

        interactObject.StartAsk(this);
    }


    // MoveableItem leadása az InteractableItem-nek.
    public void give(string targetName)
    {
        if (string.IsNullOrWhiteSpace(targetName))
        {
            //Debug.LogWarning("Target name is null or empty.");
            return;
        }

        if (carriedItem == null)
        {
            //Debug.LogWarning("No item is currently being carried to give.");
            return;
        }

        GameObject targetObject = GameObject.Find(targetName);
        if (targetObject == null)
        {
           // Debug.LogWarning($"Target object '{targetName}' not found.");
            return;
        }

        InteractableObject interactObject = targetObject.GetComponent<InteractableObject>();
        if (interactObject == null)
        {
            //Debug.LogWarning($"Target object '{targetName}' is not an InteractableObject.");
            return;
        }

        //Debug.Log("Giving...");
        interactObject.StartGive(this);
    }

    public void giveItem(bool success, InteractableObject from)
    {
        if (success)
        {
            if (carriedItem != null)
            {
                carriedItem.Hide();
                carriedItem = null;

                //Debug.Log("Character put down the item and now it's hidden.");
                _chat.SendMessage("Character 1", from.name, "Örülök, hogy segíthettem!");
            }
            else
            {
               // Debug.LogWarning("Wrong item: No item to put down.");
                _chat.SendMessage("Character 1", from.name, "Sajnálom, hogy rossz tárgyat hoztam.");
            }
        }
    }


  



    public void NextCommand()
    {
        isExecutingCommand = false;
        if (!isExecutingCommand)
        {
            ExecuteNextCommand();
        }
    }



    // Manage timer for self message
 
    void Update()
    {
        
    }

 


    //#################################################################
    //#  Communication with other Characters using the GPTAPI object  #
    //#################################################################

    //Main variables forthe prompt

    public TMP_InputField tstMessageInput = null;

    public GPTAPI gptAPI;


    protected int mood = 5;
    protected int stress = 5;
    protected int connectionCharacter1 = 5;
    protected int connectionWork = 5;


    protected Character activeConversationPartner = null;

    public void tstInputTrigger()
    {
        RecieveMessageFromUser(null, this.tstMessageInput.text);
    }

    public void RecieveMessage(Character sender, string message)
    {
        Debug.LogWarning($"ÉN : {this.name} MOST KAPTAM MEG TÕLE: {sender.name}, {message}");
        if(this.activeConversationPartner == null || sender == this.activeConversationPartner)
        {
            this.activeConversationPartner = sender;
            message += $" | Az utolsó üzenetet küldõ karakter neve: {sender.name}";
            this.gptAPI.RequestOpenAIResponse(message, this, constructPrompt());
        }
        else
        {
            Debug.LogWarning($"{sender.name} közbeszólt {this.name} beszélgetésébe.");
        }
    }

    public void RecieveInteractableMessage(InteractableObject sender, string message)
    {
        Debug.LogWarning($"ÉN : {this.name} MOST KAPTAM MEG TÕLE: {sender.name}, {message}");

        message += $" | Az utolsó üzenetet küldõ lerakóhely neve: {sender.name}";
        this.gptAPI.RequestOpenAIResponse(message, this, constructPrompt());

    }

    public void RecieveMessageFromUser(Character sender, string message)
    {
        if (this.activeConversationPartner == null && sender == null)
        {
            //this.activeConversationPartner = sender;
            message += $" | Az utolsó üzenetet küldõ játékos neve: Játékkal játszó játékos (FÕNÖK.)";
            //Elküldi a saját referenciáját az API-nak, hogy vissza tudja hívni. A hívó karakter csak helyileg van tárolva.
            this.gptAPI.RequestOpenAIResponse(message, this, constructPrompt());
        }
        else
        {
            Debug.LogWarning($"JÁTÉKOS közbeszólt {this.name} beszélgetésébe.");
        }
    }

  
    //kommunikáció kezdeményezése.


    public void SendMessage(string toCharacterName, string responseToSender,string responseToPublic, string endConversation)
    {

       
        if (responseToSender != null && activeConversationPartner != null)
        {
            Debug.LogError("C");
            this._chat.SendMessage(this.name, activeConversationPartner.name, responseToSender);
            this.activeConversationPartner.RecieveMessage(this, responseToSender);
        }
        if (responseToSender != null && this.activeConversationPartner == null)
        {

            GameObject targetObject = GameObject.Find(toCharacterName);
             if (targetObject != null)
             {
                Debug.LogError("B");
                Character character = targetObject.GetComponent<Character>();

                this._chat.SendMessage(this.name, character.name, responseToSender);

                character.RecieveMessage(this, responseToSender);

                this.activeConversationPartner = character;
             }
             else
             {
                Debug.LogWarning("Target object is not an InteractableObject or not found: " + toCharacterName);
             }
        }
        if (endConversation == "Igen")
        {
            Debug.LogError("A");


            if (this.activeConversationPartner != null)
            {;
                this.activeConversationPartner.RemoveCommunicatingPartner();
            }
            RemoveCommunicatingPartner();
        }

        if (responseToPublic != "")
        {

            this._chat.SendMessage(this.name,"all", responseToPublic);
            
        }

        

    }

    public void RemoveCommunicatingPartner()
    {
        this.activeConversationPartner = null;
    }
    public void ProcessFunctionalResponse(List<string> commands, int newMood, int newStress, int newConnectionC1, int newConnectionWork)
    {
        if (commands != null)
        {
            AddCommandsToQueue(commands);
        }
        
        this.mood = newMood;
        this.stress = newStress;
        this.connectionCharacter1 = newConnectionC1;
        this.connectionWork = newConnectionWork;
    }

    public virtual string constructPrompt()
    {
        Debug.LogWarning("This is the Character classes construnctPrompt(). Use Worker or Manager classes.");
        return null;
    }

  
}
