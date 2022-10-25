using System;
using System.Collections;
using System.Collections.Generic;
using CodeBase.Infrastructure.Events;
using UnityEngine;
using Zenject;

public class ButtonClick : MonoBehaviour
{
    private EventReferer eventReferer;
    private int buttonIndex;

    [Inject]
    public void Construct(EventReferer eventReferer)
    {
        this.eventReferer = eventReferer;
    }

    private void Awake()
    {
        buttonIndex = gameObject.name == "NextButton" ? 0 : 1;
        gameObject.SetActive(false);
    }

    public void OnButtonClick() =>
        eventReferer.ButtonClick();
}