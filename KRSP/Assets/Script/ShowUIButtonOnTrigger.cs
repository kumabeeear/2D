using UnityEngine;

public class ShowUIButtonOnTrigger : MonoBehaviour
{
    public GameObject targetButton; // 按钮

    private void Start()
    {
        if (targetButton != null)
        {
            targetButton.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (targetButton != null)
            {
                targetButton.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (targetButton != null)
            {
                targetButton.SetActive(false);
            }
        }
    }
}