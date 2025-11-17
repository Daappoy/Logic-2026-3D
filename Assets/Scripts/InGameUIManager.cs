using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
public class InGameUIManager : MonoBehaviour
{
    public static InGameUIManager Instance;
    public Slider Healthbar;
    public GameObject GameUI;
    public TextMeshProUGUI AmmoText;
    public TextMeshProUGUI ReloadText;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    public void UpdateHealthBar()
    {
        Healthbar.maxValue = PlayerDisplay.Instance.MaxHealth;
        Healthbar.value = PlayerDisplay.Instance.currentHealth;
    }
    public void UpdateAmmoText(int currentAmmo, int maxAmmo)
    {
        AmmoText.text = currentAmmo.ToString() + " / " + maxAmmo.ToString();
    }
    public void ShowReloadText(string message,bool show)
    {
            ReloadText.text = message;
            ReloadText.gameObject.SetActive(show);
    }
}
