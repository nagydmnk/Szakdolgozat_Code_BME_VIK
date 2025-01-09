using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;




public class GPTAPI_Manager : MonoBehaviour
{
    private readonly string apiUrl = "https://api.openai.com/v1/chat/completions";
    private readonly string apiKey = "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";

    public TMP_Text text;

    public string ResponseContent { get; private set; }

    public void RequestOpenAIResponse(string userInput, Character agent, string prompt)
    {

        Debug.LogWarning(userInput);

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
                string resultText = request.downloadHandler.text;
                Debug.Log("Raw response: " + resultText);

                OpenAIResponse response = JsonUtility.FromJson<OpenAIResponse>(resultText);
                if (response.choices != null && response.choices.Length > 0 && response.choices[0].message != null)
                {
                    //It jön be a válasz
                    ResponseContent = response.choices[0].message.content;

                    RespMessageManager message = JsonUtility.FromJson<RespMessageManager>(ResponseContent);
                    List<string> commandsList = new List<string>(message.commands.Split(','));

                    for (int i = 0; i < commandsList.Count; i++)
                    {
                        commandsList[i] = commandsList[i].TrimStart().TrimEnd();
                    }

                    string[] memoryParts = message.interactableMemoryUpdate.Split(',');

                    foreach(string part in memoryParts)
                    {
                        string[] parameters = part.Trim().Split(' ');
                        if(parameters.Length == 3)
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


                    Debug.Log("API Obj : Extracted content: " + ResponseContent);

                    agent.SendMessage(message.message_private, message.message_public);
                    agent.ProcessFunctionalResponse(commandsList, message.new_mood, message.new_stress, message.new_connectionCharacter1, message.new_connectionWork);

                    Debug.Log($"Response string: {ResponseContent}");
                }
                else
                {
                    Debug.LogWarning("Response JSON structure was not as expected, or was missing data.");
                    ResponseContent = null; //Clear the response content if parsing fails
                }
            }
        }
    }
}
