using UnityEngine;

public class WaveDisabled : MonoBehaviour
{
    public GetUpgrade upgradeSystem;
    public LevelStateController gameManager;
    private void Update()
    {
        if (transform.childCount <= 0)
        {
            if (upgradeSystem != null && gameManager != null)
            {
                upgradeSystem.ShowPanel();
                gameManager.PlayerChoosesUpgrade();
            }
            Destroy(gameObject);
        }
            
    }
}
