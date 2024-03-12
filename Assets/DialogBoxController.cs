using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class DialogBoxController : MonoBehaviour
{
    [SerializeField]
    GameObject dialogPanel;

    [SerializeField]
    Button optionButton;

    void Start()
    {
        ShowDialog(true);
        optionButton.onClick.AddListener(OnOptionButtonClicked);
    }

    void Update()
    {
        if (Gamepad.current.buttonSouth.isPressed || Input.GetKey(KeyCode.Alpha1))
        {
            ShowDialog(false);
        }

    }

    private void ShowDialog(bool show)
    {
        dialogPanel.SetActive(show);
    }

    private void OnOptionButtonClicked() {
        ShowDialog(false);
    }
}

