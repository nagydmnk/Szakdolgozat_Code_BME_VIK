using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Networking;


[Serializable]
public class RespMessage
{
    public string endConversation;
    public string talkto;
    public string message_private;
    public string message_public;
    public string commands;
    public int new_mood;
    public int new_stress;
    public int new_connectionCharacter1;
    public int new_connectionWork;
}

[Serializable]
public class RespMessageManager
{
    public string endConversation;
    public string talkto;
    public string message_private;
    public string message_public;
    public string commands;
    public int new_mood;
    public int new_stress;
    public int new_connectionCharacter1;
    public int new_connectionWork;
    public string interactableMemoryUpdate;
}

[System.Serializable]
public class Message
{
    public string role;
    public string content;
}

[System.Serializable]
public class Payload
{
    public string model = "gpt-4o-mini";
    public List<Message> messages = new List<Message>();
}

[System.Serializable]
public class OpenAIResponse
{
    public Choice[] choices;
}

[System.Serializable]
public class Choice
{
    public Message message;
}

public class GPTAPI : MonoBehaviour
{
    private readonly string apiUrl = "https://api.openai.com/v1/chat/completions";
    private readonly string apiKey = "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"; 

    public TMP_Text W1text;

    public string ResponseContent { get; private set; }

    public void RequestOpenAIResponse(string userInput, Character agent, string prompt)
    {

         StartCoroutine(PostRequest(userInput, agent, prompt));     

    }

    private IEnumerator PostRequest(string userInput, Character agent, string prompt)
    {
        Payload payload = new Payload();
        payload.messages.Add(new Message
        {
            role = "system",
            content = prompt

        });
        payload.messages.Add(new Message { role = "user", content = userInput });

        string json = JsonUtility.ToJson(payload);

        using (UnityWebRequest request = UnityWebRequest.Put(apiUrl, json))
        {
            request.method = UnityWebRequest.kHttpVerbPOST;
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", $"Bearer {apiKey}");
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(request.error);
                ResponseContent = null; //Clear the response content on error
            }
            else
            {
                //Worker válasza
                if (agent.GetComponent<Manager>() == null && agent.GetComponent<Worker>() != null)
                {
                    string resultText = request.downloadHandler.text;

                    OpenAIResponse response = JsonUtility.FromJson<OpenAIResponse>(resultText);
                    if (response.choices != null && response.choices.Length > 0 && response.choices[0].message != null)
                    {
                        //It jön be a válasz
                        ResponseContent = response.choices[0].message.content;

                        Debug.Log($"Response string: {ResponseContent}");

                        RespMessage message = JsonUtility.FromJson<RespMessage>(ResponseContent);
                        List<string> commandsList = new List<string>(message.commands.Split(','));

                        for (int i = 0; i < commandsList.Count; i++)
                        {
                            commandsList[i] = commandsList[i].TrimStart().TrimEnd();
                        }

                        agent.SendMessage(
                            message.talkto, 
                            message.message_private, 
                            message.message_public, 
                            message.endConversation);

                        agent.ProcessFunctionalResponse(
                            commandsList, message.new_mood, 
                            message.new_stress, 
                            message.new_connectionCharacter1, 
                            message.new_connectionWork
                            );


                        this.W1text.text = $"W1          " +
                            $"{message.new_mood}            " +
                            $"{message.new_stress}              " +
                            $"{message.new_connectionWork}          " +
                            $"{message.new_connectionCharacter1}";
                    }
                    else
                    {
                        Debug.LogWarning("Response JSON structure was not as expected, or was missing data.");
                        ResponseContent = null; 
                    }
                }
                //Manager válasza
                else if(agent.GetComponent<Worker>() == null && agent.GetComponent<Manager>() != null)
                {
                    string resultText = request.downloadHandler.text;

                    OpenAIResponse response = JsonUtility.FromJson<OpenAIResponse>(resultText);
                    if (response.choices != null && response.choices.Length > 0 && response.choices[0].message != null)
                    {
                        //It jön be a válasz
                        ResponseContent = response.choices[0].message.content;

                        Debug.Log($"Response string: {ResponseContent}");

                        RespMessageManager message = JsonUtility.FromJson<RespMessageManager>(ResponseContent);
                        List<string> commandsList = new List<string>(message.commands.Split(','));

                        for (int i = 0; i < commandsList.Count; i++)
                        {
                            commandsList[i] = commandsList[i].TrimStart().TrimEnd();
                        }

                        string[] memoryParts = message.interactableMemoryUpdate.Split(',');

                        foreach (string part in memoryParts)
                        {
                            string[] parameters = part.Trim().Split(' ');
                            if (parameters.Length == 3)
                            {
                                string interactableObjectName = parameters[0];
                                string neededItem = parameters[1];
                                int numberOfNeededItem;

                                if (int.TryParse(parameters[2], out numberOfNeededItem))
                                {

                                }
                                else
                                {
                                    Debug.LogWarning("Error: last param is not a numbar in memory update.");
                                }
                            }
                        }

                        this.W1text.text = $"M1          {message.new_mood}            " +
                            $"{message.new_stress}              " +
                            $"{message.new_connectionWork}          " +
                            $"{message.new_connectionCharacter1}";

                        agent.SendMessage(
                            message.talkto,
                            message.message_private, 
                            message.message_public, 
                            message.endConversation
                            );
                         
                        agent.ProcessFunctionalResponse(
                            commandsList, 
                            message.new_mood, 
                            message.new_stress, 
                            message.new_connectionCharacter1, 
                            message.new_connectionWork
                            );
                    }
                    else
                    {
                        Debug.LogWarning("Response JSON structure was not as expected, or was missing data.");
                        ResponseContent = null; 
                    }
                }
                
            }
        }
    }
}
