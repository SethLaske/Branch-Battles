using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MagicButtons : MonoBehaviour
{
    private float buttonCooldownTime;
    public Button thisButton;
    public TextMeshProUGUI MagicName;
    public TextMeshProUGUI MagicCost;
    // Start is called before the first frame update

    public void SetMagicType(Magic magic)
    {
        MagicName.text = magic.magicName;
        MagicCost.text = magic.soulCost.ToString();
        GetComponent<Image>().sprite = magic.buttonUI;
        buttonCooldownTime = magic.cooldownTime;
        magic.magicButton = this;
    }

    public void ShowCost()
    {
        if (thisButton.interactable == true)
        {
            MagicName.gameObject.SetActive(false);
            MagicCost.gameObject.SetActive(true);
        }
    }

    public void ShowName()
    {
        MagicName.gameObject.SetActive(true);
        MagicCost.gameObject.SetActive(false);
    }

    public void MagicActivated() {
        StartCoroutine(ButtonCooldown());
    }

    IEnumerator ButtonCooldown() {
        thisButton.interactable = false;

        yield return new WaitForSeconds(buttonCooldownTime);

        thisButton.interactable = true;
    }
}
