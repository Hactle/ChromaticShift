using System;
using System.Collections;
using UnityEngine;

public class LevelCompletePoint : MonoBehaviour
{   
    private Coroutine _finishCoroutine;

    public static event Action OnLevelComplete;

    private bool _isPlayerInPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _isPlayerInPoint = true;
            Debug.Log("Player Enter Complete Point");

            _finishCoroutine ??= StartCoroutine(CheckFinishCondition());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _isPlayerInPoint = false;
        }
    }

    private IEnumerator CheckFinishCondition()
    {

        float timer = 0f;
        float requiredTime = 2f;

        while (timer < requiredTime)
        {
            if (!_isPlayerInPoint)
            {
                Debug.Log("Отсчет прерван - игрок вышел из зоны");
                _finishCoroutine = null;
                yield break; 
            }

            timer += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }

        OnLevelComplete?.Invoke();

        _finishCoroutine = null;
    }
}
