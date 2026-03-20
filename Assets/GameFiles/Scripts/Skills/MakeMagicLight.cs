using Unity.VisualScripting;
using UnityEngine;

public class MakeMagicLight : MonoBehaviour
{
    [SerializeField] private int torchesCount = 2;
    [SerializeField] private float rayDistance = 3.0f;
    [SerializeField] private GameObject panelAnswer;
    private bool canChangeLight = false;
    private RaycastHit hit;
    private bool hasHit;

    private void Start()
    {
        torchesCount = 2;
        panelAnswer = FindFirstObjectByType<PanelAnswer>(FindObjectsInactive.Include).gameObject;
    }

    private void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        hasHit = Physics.Raycast(ray, out hit, rayDistance);

        if (hasHit)
        {
            if (hit.collider.CompareTag("Torch") && hit.collider.transform.GetChild(1).GetComponent<MagicLight>() == null)
            {
                if (torchesCount > 0)
                {
                    panelAnswer.SetActive(true);
                    canChangeLight = true;
                }
                else
                {
                    canChangeLight = false;
                    panelAnswer.SetActive(false);
                }
                    
            }
            else
            {
                canChangeLight = false;
                panelAnswer.SetActive(false);
            }     
        }
    }

    public void OnInteract()
    {
        if (canChangeLight && hasHit)
        {
            hit.collider.transform.GetChild(1).AddComponent<MagicLight>();
            hit.collider.transform.GetChild(0).gameObject.SetActive(false);
            hit.collider.transform.GetChild(1).gameObject.SetActive(true);
            hit.collider.transform.GetChild(2).gameObject.SetActive(false);
            hit.collider.transform.GetChild(3).gameObject.SetActive(true);
            torchesCount--;
        }
    }
}
