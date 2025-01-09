using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public interface ICommunication
{
    public void SendMessage(string to, string responseToSender, string responseToPublic, string endConversation);
    public void RecieveMessage(Character from, string message);
    public void RemoveCommunicatingPartner();
    public void ProcessFunctionalResponse(List<string> commands, int newMood, int newStress, int newConnectionC1, int newConnectionWork);

}
