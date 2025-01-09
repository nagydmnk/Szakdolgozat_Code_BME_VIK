using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worker : Character
{

    public Manager manager;
    // Start is called before the first frame update
    void Start()
    {
        _chat.SendMessage(this.name, "all", "'Started'");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   

    public override string constructPrompt()
    {
        string filePath = "Assets/Dev/classes/AI-API/worker_prompt.txt"; // Adjust path as needed
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
               "Aktuális játékállapot:" +
               "Pályán lévõ lerakóhelyek: " + this.WorldManager.getInteractableNames() +
               "Elérhetõ tárgyak:" + this.WorldManager.getMoveableNames() +
               "Aktuális állapotod:" +
               "Kedv:" + this.mood +
               "Stressz: " + this.stress +
               "Hozzáállás a manager játékoshoz:" + this.connectionCharacter1 +
               "Munkához való hozzáállás:" + this.connectionWork +
                "Manager játékos neve: " + this.manager.name;
    }
}
