using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSpawner : MonoBehaviour
{
    private static ArrowSpawner instance;

    public static ArrowSpawner Instance => instance ?? (instance = FindObjectOfType<ArrowSpawner>());

    [SerializeField] private List<GameObject> stackedArrows = new List<GameObject>(); 
    [SerializeField] private GameObject arrowPrefab;

    [SerializeField] private float arrowDistance, arrowDistanceXPosition, arrowDistanceSpeed;
    [SerializeField] private float minimumArrowDistance, maximumArrowDistance;

    [SerializeField] private float arrowsSortDistance, arrowSortSpeed;

    private bool isHitSortWall = false;

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

    void Update()
    {
        SortArrowsSideBySide(arrowsSortDistance);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Gate"))
        {
            string sign = other.GetComponent<Gate>().GetSign();
            int number = other.GetComponent<Gate>().GetNumber();

            Calculate(sign, number);
        }

        if (other.CompareTag("SortWall"))
        {
            isHitSortWall = true;
        }
    }

    private void Calculate(string sign, int number)
    {
        int newArrowCount = 0;
        int currentArrowCount = stackedArrows.Count;

        switch (sign)
        {
            case "+":
                CreateArrow(number);
                break;

            case "-":
                DestroyArrow(number);
                break;

            case "x":
                newArrowCount = currentArrowCount * number;
                int addingArrowCount = newArrowCount - currentArrowCount;
                CreateArrow(addingArrowCount);
                break;

            case "÷":
                newArrowCount = currentArrowCount / number;
                int removingArrowCount = currentArrowCount - newArrowCount;
                DestroyArrow(removingArrowCount);
                break;
        }
    }

    private void CreateArrow(int arrowCount)
    {
        for(int i = 0; i < arrowCount; i++)
        {
            GameObject arrow = Instantiate(arrowPrefab, transform);
            stackedArrows.Add(arrow);
        }

        Sort();
    }

    private void DestroyArrow(int arrowCount)
    {
        if(arrowCount < stackedArrows.Count)
        {
            for (int i = 0; i < arrowCount; i++)
            {
                GameObject arrow = stackedArrows[0];
                stackedArrows.Remove(arrow);
                Destroy(arrow);
            }

            Sort();
        }
        else
        {
            GameManager.Instance.OnGameFailed();
        }
    }

    private void Sort()
    {
        float angle = 1f;
        angle = 360 / stackedArrows.Count;

        if(stackedArrows.Count > 1)
        {
            for (int i = 0; i < stackedArrows.Count; i++)
            {
                MoveArrows(stackedArrows[i].transform, i * angle);
            }
        }
    }

    private void MoveArrows(Transform arrowTransform, float degree)
    {
        Vector3 position = Vector3.zero;

        position.x = Mathf.Cos(degree * Mathf.Deg2Rad);
        position.y = Mathf.Sin(degree * Mathf.Deg2Rad);

        arrowTransform.localPosition = position * arrowDistance;
    }

    private void SortArrowsSideBySide(float arrowsXDistance)
    {
        if(isHitSortWall == true)
        {
            for (int i = 0; i < stackedArrows.Count; i++)
            {
                stackedArrows[i].transform.localPosition = new Vector3(stackedArrows[i].transform.localPosition.x, 0f, 0f);
            }

            float xDistanceFromCenter = ((stackedArrows.Count - 1) / 2) * arrowsXDistance;

            var position = stackedArrows[0].transform.position;
            position.x = Mathf.Lerp(position.x, transform.position.x - xDistanceFromCenter, arrowSortSpeed);
            stackedArrows[0].transform.position = position;

            for (int i = 1; i < stackedArrows.Count; i++)
            {
                var position2 = stackedArrows[i].transform.position;
                position2.x = Mathf.Lerp(position2.x, stackedArrows[i - 1].transform.position.x + arrowsXDistance, arrowSortSpeed);
                stackedArrows[i].transform.position = position2;
            }
        }    
    }

    public void CheckArrowDistance(Vector3 sideMovementRoot)
    {
        if (isHitSortWall == false)
        {
            if (sideMovementRoot.x <= -arrowDistanceXPosition || sideMovementRoot.x >= arrowDistanceXPosition)
            {
                if (arrowDistance >= minimumArrowDistance)
                {
                    arrowDistance -= Time.deltaTime * arrowDistanceSpeed;
                    Sort();
                }
            }
            else
            {
                if (arrowDistance <= maximumArrowDistance)
                {
                    arrowDistance += Time.deltaTime * arrowDistanceSpeed;
                    Sort();
                }
            }
        }
    }
}
