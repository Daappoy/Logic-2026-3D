using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class radarPing : MonoBehaviour
{
    private Image pingImage;
    public float dissapearTimer;
    public float dissapearTimerMax;
    private Color color;

    void Start()
    {
        pingImage = GetComponent<Image>();
        color = new Color(1, 1, 1, 1);
    }

    void Update()
    {
        dissapearTimer += Time.deltaTime;
        color.a = Mathf.Lerp(dissapearTimerMax, 0, dissapearTimer / dissapearTimerMax);
        pingImage.color = color;

        if(dissapearTimer >= dissapearTimerMax)
        {
            Destroy(gameObject);
        }
    }

    public void SetPingColor(Color newColor)
    {
        this.color = newColor;
    }

    public void SetDissapearTimerMax(float newDissapearTimerMax)
    {
        this.dissapearTimerMax = newDissapearTimerMax;
        dissapearTimer = 0;
    }
}
