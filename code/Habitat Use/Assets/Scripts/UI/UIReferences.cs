using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UIReferences : MonoBehaviour
{
    public Queue<IUIPopup> popupqueue = new Queue<IUIPopup>();
    public GameObject eventTitle, eventDescription, eventUI;
    public GameObject lawTitle, lawDescription, lawReward, lawRequirement, lawRequirementNotMet, lawUI;
    public GameObject questTitle, questDescription, questReward, questRequirement, questRequirementNotMet, questUI;

    public Material red, green, brown;

    public GameObject trippleLaw1Title,
        trippleLaw1Description,
        trippleLaw1Reward,
        trippleLaw1Requirement,
        trippleLawUI;

    public GameObject trippleLaw2Title,
        trippleLaw2Description,
        trippleLaw2Reward,
        trippleLaw2Requirement;

    public GameObject trippleLaw3Title,
        trippleLaw3Description,
        trippleLaw3Reward,
        trippleLaw3Requirement;
    
    public void enqueuePopup(IUIPopup popup)
    {
        popupqueue.Enqueue(popup);
    }

    public void nextPopup()
    {
        if (popupqueue.Any())
        {
            IUIPopup popup = popupqueue.Dequeue();
            popup.Popup();
        }
    }
}