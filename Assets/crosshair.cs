using UnityEngine;
using UnityEngine.UI;

public class CrosshairManager : MonoBehaviour
{
    public Image crosshairUI;
    public Sprite defaultCrosshair;
    public Sprite grappleCrosshair;

    void Start()
    {
        SetCrosshair(defaultCrosshair);
    }

    public void SetCrosshair(Sprite newCrosshair)
    {
        if (crosshairUI != null)
            crosshairUI.sprite = newCrosshair;
    }
}
