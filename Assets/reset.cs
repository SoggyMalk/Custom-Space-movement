using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetOnDoubleEsc : MonoBehaviour
{
    private float escTimer = 0f;
    private int escCount = 0;
    public float escResetTime = 1f; // time window to detect double press

    void Update()
    {
        CheckEscReload();
    }

    void CheckEscReload()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            escCount++;
            if (escCount == 2)
            {
                // Reload current scene
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            escTimer = escResetTime;
        }

        // Countdown timer
        if (escTimer > 0)
        {
            escTimer -= Time.deltaTime;
            if (escTimer <= 0)
                escCount = 0; // reset if timer runs out
        }
    }
}
