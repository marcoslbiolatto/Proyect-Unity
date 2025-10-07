using UnityEngine;
using TMPro;

public class StartInstruction : MonoBehaviour
{
    private TMP_Text instructionText;

    void Start()
    {
        instructionText = GetComponent<TMP_Text>();
        instructionText.gameObject.SetActive(true);
    }

    public void HideInstruction()
    {
        instructionText.gameObject.SetActive(false);
    }
}
