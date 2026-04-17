using UnityEngine;

public static class HUDHider
{
    private static int hideCount = 0;
    private static GameObject hud;

    private static void ResolveHUD()
    {
        if (hud == null)
        {
            hud = GameObject.FindGameObjectWithTag("HUD");
        }
    }

    public static void Hide()
    {
        ResolveHUD();
        if (hud == null)
        {
            return;
        }

        hideCount++;
        if (hideCount == 1)
        {
            hud.SetActive(false);
        }
    }

    public static void Show()
    {
        ResolveHUD();
        if (hud == null)
        {
            return;
        }

        if (hideCount > 0)
        {
            hideCount--;
        }

        if (hideCount == 0)
        {
            hud.SetActive(true);
        }
    }

    public static void Reset()
    {
        ResolveHUD();
        if (hud == null)
        {
            return;
        }

        hideCount = 0;
        hud.SetActive(true);
    }
}
