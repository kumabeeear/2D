using UnityEngine;
using System.Collections;
using Fungus;

public class ChapterManager : MonoBehaviour
{
    [System.Serializable]
    public class ChapterData
    {
        [Header("章节名")]
        public string chapterName;

        [Header("点位")]
        public Transform camStart;      // 本章开场镜头位置
        public Transform camTarget;     // 本章开场对话后，镜头平移到玩家的位置
        public Transform playerSpawn;   // 玩家在本章开始时站的位置

        [Header("Fungus Block")]
        public string introBlockName;   // 本章开场对话
        public string endBlockName;     // 本章结束前的小对话（可空）

        [Header("本章结束时的音效")]
        public AudioClip transitionSound;
        public float transitionWaitTime = 1f; // 没有音效时等待多久
    }

    [Header("章节配置")]
    public ChapterData[] chapters;
    public int currentChapterIndex = 0;

    [Header("Player")]
    public GameObject player;
    public MonoBehaviour playerMoveScript;
    public Rigidbody2D playerRb;

    [Header("Camera")]
    public Camera mainCamera;                  // 剧情镜头
    public GameObject cinemachineCamera;      // 玩家跟随镜头
    public float cameraMoveSpeed = 3f;

    [Header("Transition")]
    public GameObject blackScreen;
    public AudioSource audioSource;

    [Header("Fungus")]
    public Flowchart flowchart;

    private bool isBusy = false;
    private bool waitingForEndDialogue = false;

    void Start()
    {
        if (playerMoveScript != null)
            playerMoveScript.enabled = false;

        if (cinemachineCamera != null)
            cinemachineCamera.SetActive(false);

        if (mainCamera != null)
            mainCamera.gameObject.SetActive(true);

        if (blackScreen != null)
            blackScreen.SetActive(false);

        PlaceMainCameraAtCurrentChapterStart();
    }

    // ===== 开场 =====

    public void PlaceMainCameraAtCurrentChapterStart()
    {
        if (!IsChapterValid(currentChapterIndex)) return;

        Transform camStart = chapters[currentChapterIndex].camStart;
        if (mainCamera != null && camStart != null)
        {
            mainCamera.transform.position = new Vector3(
                camStart.position.x,
                camStart.position.y,
                mainCamera.transform.position.z
            );
        }
    }

    // 开场对话结束后，调用这个
    public void MoveCameraToCurrentChapterTarget()
    {
        if (isBusy || !IsChapterValid(currentChapterIndex)) return;

        Transform target = chapters[currentChapterIndex].camTarget;
        if (target == null)
        {
            Debug.LogWarning("当前章节没有设置 camTarget");
            return;
        }

        StartCoroutine(MoveCameraRoutine(target));
    }

    private IEnumerator MoveCameraRoutine(Transform target)
    {
        isBusy = true;

        if (mainCamera == null || target == null)
        {
            isBusy = false;
            yield break;
        }

        Vector3 targetPos = new Vector3(
            target.position.x,
            target.position.y,
            mainCamera.transform.position.z
        );

        while (Vector3.Distance(mainCamera.transform.position, targetPos) > 0.05f)
        {
            mainCamera.transform.position = Vector3.MoveTowards(
                mainCamera.transform.position,
                targetPos,
                cameraMoveSpeed * Time.deltaTime
            );

            yield return null;
        }

        mainCamera.transform.position = targetPos;

        if (cinemachineCamera != null)
            cinemachineCamera.SetActive(true);

        if (mainCamera != null)
            mainCamera.gameObject.SetActive(false);

        if (playerMoveScript != null)
            playerMoveScript.enabled = true;

        isBusy = false;
    }

    // ===== 章节结尾 =====

    // Trigger碰到后调用这个
    public void StartChapterEnd()
    {
        if (isBusy || !IsChapterValid(currentChapterIndex)) return;

        ChapterData currentChapter = chapters[currentChapterIndex];

        // 先锁玩家
        if (playerMoveScript != null)
            playerMoveScript.enabled = false;

        if (playerRb != null)
        {
            // 新版Unity常用 linearVelocity；旧版可换成 velocity
            playerRb.linearVelocity = Vector2.zero;
        }

        // 如果这一章有结尾对话，就先播对话
        if (flowchart != null && !string.IsNullOrEmpty(currentChapter.endBlockName))
        {
            waitingForEndDialogue = true;
            flowchart.ExecuteBlock(currentChapter.endBlockName);
        }
        else
        {
            StartCoroutine(GoToNextChapterRoutine());
        }
    }

    // 结尾对话播完后，在 Fungus 里调用这个
    public void ContinueAfterEndDialogue()
    {
        if (!waitingForEndDialogue) return;

        waitingForEndDialogue = false;
        StartCoroutine(GoToNextChapterRoutine());
    }

    private IEnumerator GoToNextChapterRoutine()
    {
        if (!IsChapterValid(currentChapterIndex))
            yield break;

        isBusy = true;

        ChapterData currentChapter = chapters[currentChapterIndex];

        // 切回剧情镜头
        if (cinemachineCamera != null)
            cinemachineCamera.SetActive(false);

        if (mainCamera != null)
            mainCamera.gameObject.SetActive(true);

        // 黑屏
        if (blackScreen != null)
            blackScreen.SetActive(true);

        // 播当前章节结束音效
        if (audioSource != null && currentChapter.transitionSound != null)
        {
            audioSource.PlayOneShot(currentChapter.transitionSound);
            yield return new WaitForSeconds(currentChapter.transitionSound.length);
        }
        else
        {
            yield return new WaitForSeconds(currentChapter.transitionWaitTime);
        }

        // 没有下一章了：到这里结束
        if (!IsChapterValid(currentChapterIndex + 1))
        {
            Debug.Log("已经是最后一章");
            isBusy = false;
            yield break;
        }

        // 进入下一章
        currentChapterIndex++;
        ChapterData nextChapter = chapters[currentChapterIndex];

        // 传送玩家
        if (player != null && nextChapter.playerSpawn != null)
        {
            player.transform.position = nextChapter.playerSpawn.position;
        }

        if (playerRb != null)
        {
            playerRb.linearVelocity = Vector2.zero;
        }

        // 镜头跳到下一章开场点
        if (mainCamera != null && nextChapter.camStart != null)
        {
            mainCamera.transform.position = new Vector3(
                nextChapter.camStart.position.x,
                nextChapter.camStart.position.y,
                mainCamera.transform.position.z
            );
        }

        yield return null;

        // 关黑屏
        if (blackScreen != null)
            blackScreen.SetActive(false);

        // 播下一章开场对话
        if (flowchart != null && !string.IsNullOrEmpty(nextChapter.introBlockName))
        {
            flowchart.ExecuteBlock(nextChapter.introBlockName);
        }

        isBusy = false;
    }

    // 给 Trigger 直接调用
    public void GoToNextChapterDirect()
    {
        StartChapterEnd();
    }

    private bool IsChapterValid(int index)
    {
        return chapters != null && index >= 0 && index < chapters.Length;
    }
}