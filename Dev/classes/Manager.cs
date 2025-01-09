using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Manager : Character
{
    public Worker worker;
    private List<CharacterMemory> neededItems = new List<CharacterMemory>();

    // Start is called before the first frame update
    void Start()
    {
        this.neededItems.Add(new CharacterMemory(this.WorldManager.getInteractByIndex(0)));
        _chat.SendMessage(this.name, "all", "'Started'");
    }

   // Manage timer for self message
    private float timer = 0f;
    private float interval = 10f;
    void Update()
    { /*
        timer += Time.deltaTime;
        if (timer >= interval)
        {
            if(this.activeConversationPartner == null) RecieveMessage(this, "Gondold �t a j�t�kmenetet.");

            timer = 0f;
        }
        */
    }


    public void UpdateInteractableMemory(string name, string neededObject, int numberOfObjects)
    {
        GameObject interactableObject = GameObject.Find(name);

        if (interactableObject == null)
        {
            Debug.LogWarning($"GameObject with name '{name}' not found.");
            return;
        }

        CharacterMemory existingMemory = neededItems.FirstOrDefault(memory => interactableObject == memory.getInteractableObject());

        if (existingMemory != null)
        {
            UpdateMemory(existingMemory, neededObject, numberOfObjects);
        }
        else
        {
            CharacterMemory newMemory = new CharacterMemory(interactableObject.GetComponent<InteractableObject>());
            if (UpdateMemory(newMemory, neededObject, numberOfObjects))
            {
                neededItems.Add(newMemory);
            }
        }
    }

    private bool UpdateMemory(CharacterMemory memory, string neededObject, int numberOfObjects)
    {
        switch (neededObject)
        {
            case "Cube":
                memory.setNeededCubes(numberOfObjects);
                return true;
            case "Sphere":
                memory.setNeededSpheares(numberOfObjects);
                return true;
            default:
                Debug.LogError("Invalid neededObject name for memory update.");
                return false;
        }
    }

    
    public override string constructPrompt()
    {
        string filePath = "Assets/Dev/classes/AI-API/manager_prompt.txt"; // Adjust path as needed
        string staticPrompt;

        // Reading the static prompt from the file
        try
        {
            staticPrompt = System.IO.File.ReadAllText(filePath);
        }
        catch (Exception e)
        {
            // Handle file read errors
            return "Error reading prompt template: " + e.Message;
        }

        // Appending dynamic content to the prompt
        return staticPrompt +
               "Aktu�lis j�t�k�llapot:" +
               "P�ly�n l�v� lerak�helyek: " + this.WorldManager.getInteractableNames() +
               "Lerak�helyek ismert �llapota: " + getAllMemoryString() +
               "El�rhet� t�rgyak:" + this.WorldManager.getMoveableNames() +
               "Aktu�lis �llapotod:" +
               "\r\nKedv:" + this.mood +
               "\r\nStressz: " + this.stress +
               "\r\nHozz��ll�s a Munk�s j�t�koshoz:" + this.connectionCharacter1 +
               "\r\nMunk�hoz val� hozz��ll�s:" + this.connectionWork +
               "Munk�s j�t�kos neve: " + this.worker.name;
    }

    public string getAllMemoryString()
    {
        string data = "";
        foreach(var mem in this.neededItems)
        {
            data += mem.getMemoryString();
        }

        return data;
    }
}
