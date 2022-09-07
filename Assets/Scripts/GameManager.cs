using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager Instance => instance ?? (instance = FindObjectOfType<GameManager>());

    private bool isGameFailed = false;

    [SerializeField] private GameObject tryAgainImage, retryButton;
    [SerializeField] private GameObject levelCompletedImage, nextButton;
    [SerializeField] private GameObject targetYPosition;

    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool IsGameFailed()
    {
        return isGameFailed;
    }

    public void OnGameFailed()
    {
        isGameFailed = true;
        StartCoroutine(OnGameUI(tryAgainImage, retryButton));
    }

    public void OnGameCompleted()
    {
        virtualCamera.Follow = null;
        StartCoroutine(OnGameUI(levelCompletedImage, nextButton));
    }

    private IEnumerator OnGameUI(GameObject image, GameObject button)
    {
        image.transform.DOMoveY(targetYPosition.transform.position.y, 1.5f)
            .SetEase(Ease.OutBack);

        yield return new WaitForSeconds(2f);

        button.transform.DOScale(Vector3.one, 1.5f)
            .SetEase(Ease.OutBack);
    }

    public void RetryButton()
    {
        SceneManager.LoadScene(0);
    }
}
