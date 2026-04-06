using UnityEngine;

public class ChapterEndTrigger : MonoBehaviour
{
    public ChapterManager manager;
    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (triggered) return;

        if (other.CompareTag("Player"))
        {
            triggered = true;
            manager.StartChapterEnd();
        }
    }
}