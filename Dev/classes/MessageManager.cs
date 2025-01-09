using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageManager : MonoBehaviour
{

    private MessageManager() { }

    private List<Character> characters = new List<Character>();
    private List<InteractableObject> interactableObjects = new List<InteractableObject>();

    public void RegisterCharacter(Character character)
    {
        if (!characters.Contains(character))
        {
            characters.Add(character);
        }
    }

    public void RegisterInteractableObject(InteractableObject intObj)
    {
        if (!interactableObjects.Contains(intObj))
        {
            interactableObjects.Add(intObj);
        }
    }

    public Character tmpChar = null;

    public void HandleCommand(string command, Character targetCharacter = null)
    {
        targetCharacter = targetCharacter ?? tmpChar;
        if (targetCharacter != null && characters.Contains(targetCharacter))
        {
            targetCharacter.Controller.ProcessMessage(command);
        }
        else
        {
            Debug.LogWarning("Target character not found or not specified.");
        }
    }



}
