using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController
{
    [SerializeField] private MessageManager messageManager;
    private Character character;


    public AIController(MessageManager injectedMessageManager, Character assignedCharacter) 
    {
        messageManager = injectedMessageManager;
        character = assignedCharacter;
    }

    public void ProcessMessage(string message)
    {
        // For now, simulate a response to the message
        string gptResponse = message;
        AnalyzeResponse(gptResponse);
    }

    private void AnalyzeResponse(string response)
    {
        Debug.Log("Analyzed response: " + response);

        if (character != null)
        {
            // Split the commands by comma, handling cases with only one command
            string[] commands = response.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
            List<string> commandList = new List<string>();

            foreach (string command in commands)
            {
                if (!string.IsNullOrWhiteSpace(command))
                {
                    commandList.Add(command.Trim());
                }
            }

            // Pass the command list to the character for execution
            character.AddCommandsToQueue(commandList);
        }
    }


    void getChatGPTResponse(string message)
    {
        //TODO
    }

    void initiateConversation(Character character)
    {
        //TODO
    }
    
}
