using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public class InteractableObject : WorldObject
{

    private Character interactingCharacter = null;
    private bool waitingForAsk = false;

    private Character giveInteractingCharacter = null;
    private bool waitingForGive = false;


    public GameChat _chat = null;

    private string message = string.Empty;

    public int neededSphere = 0;

    public  int neededCube = 0;
   

    public void StartAsk(Character character)
    {
        if (!waitingForAsk && interactingCharacter == null)
        {
            Debug.Log("Asking Process Started: ");
            this.interactingCharacter = character;
            this.waitingForAsk = true;
        }
        
    }

    public void StartGive(Character character)
    {
        if (!waitingForGive && giveInteractingCharacter == null)
        {
            Debug.Log("Giving Process Started: ");
            this.giveInteractingCharacter = character;
            this.waitingForGive = true;
        }

    }

    public void TryGive()
    {
        if(giveInteractingCharacter.carriedItem == null)
        {
            this.giveInteractingCharacter = null;
            this.waitingForGive = false;
            return;
        }

        if (giveInteractingCharacter.carriedItem.type == "Sphere" && neededSphere > 0)
        {
            Debug.LogWarning(neededSphere);
            this.neededSphere -= 1;
            this._chat.SendMessage("Automata", giveInteractingCharacter.name, "Köszönöm a gömböt, szükségünk van rá!");
            giveInteractingCharacter.giveItem(true, this);
            this.giveInteractingCharacter = null;
            this.waitingForGive = false;
            return;

        }

        if(giveInteractingCharacter.carriedItem.type == "Sphere" && neededSphere <= 0)
        {
            this._chat.SendMessage("Automata", giveInteractingCharacter.name, "Sajnos nincsen szükségünk gömbre.");
            giveInteractingCharacter.giveItem(false, this);
            this.giveInteractingCharacter = null;
            this.waitingForGive = false;
            return;
        }
        
        
        
        if (giveInteractingCharacter.carriedItem.type == "Cube" && neededCube > 0)
        {
            this.neededCube -= 1;
            this._chat.SendMessage("Automata", giveInteractingCharacter.name, "Köszönöm a kockát, szükségünk van rá!");
            giveInteractingCharacter.giveItem(true, this);
            this.giveInteractingCharacter = null;
            this.waitingForGive = false;
            return;

        }

        if (giveInteractingCharacter.carriedItem.type == "Cube" && neededSphere <= 0)
        {
            this._chat.SendMessage("Automata", giveInteractingCharacter.name, "Sajnos nincsen szükségünk kockára.");
            giveInteractingCharacter.giveItem(false, this);
            this.giveInteractingCharacter = null;
            this.waitingForGive = false;
            return;
        }

    }

    public void InteractAsk()
    {
        if (waitingForAsk && interactingCharacter != null)
        {
            this._chat.SendMessage("Automata", interactingCharacter.name, message);
            interactingCharacter.RecieveInteractableMessage(this, this.message);

            this.waitingForAsk = false;
            this.interactingCharacter = null;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.message = $"Spheres:{neededSphere}";

        if (waitingForAsk && interactingCharacter != null)
        {
            float dist = Vector3.Distance(interactingCharacter.transform.position, this.transform.position);
            if(dist <= 1.5f)
            {
                InteractAsk();
            }
        }
        if(waitingForGive && giveInteractingCharacter != null)
        {
            float dist = Vector3.Distance(giveInteractingCharacter.transform.position, this.transform.position);
            if (dist <= 1.5f)
            {
                TryGive();
            }
        }

    }
}
