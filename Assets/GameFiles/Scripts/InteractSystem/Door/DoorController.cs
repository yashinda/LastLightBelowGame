/*using UnityEngine;

public class DoorController : MonoBehaviour, IInteractable
{
    [SerializeField] private bool needKey = false;
    [SerializeField] private Animator animator;

    private bool isOpen = false;

    public string GetInteractionDescription()
    {
        if (needKey & !HasKey())
            return "Нужен ключ";
        return isOpen ? "Закрыть дверь" : "Открыть дверь";
    }

    public void Interact()
    {
        if (needKey && !HasKey())
        {
            Debug.Log("Дверь заперта, нужен ключ");
            return;
        }

        if (needKey && HasKey())
        {
            KeyInventory.Instance.UseKey();
            needKey = false;
            isOpen = !isOpen;
            animator.SetBool("isOpen", isOpen);
            Debug.Log(isOpen ? "Дверь открыта" : "Дверь закрыта");
        }    

        isOpen = !isOpen;
        animator.SetBool("isOpen", isOpen);
        Debug.Log(isOpen ? "Дверь открыта" : "Дверь закрыта");
    }

    public bool HasKey()
    {
        return KeyInventory.Instance.HasKey;
    }

    public void Open()
    {
        Debug.Log($"{name} открывается");
        if (animator != null)
        {
            animator.SetBool("isOpen", true);
        }
    }

    public void Close()
    {
        Debug.Log($"{name} закрывается");
        if (animator != null)
        {
            animator.SetBool("isOpen", false);
        }
    }
}
*/