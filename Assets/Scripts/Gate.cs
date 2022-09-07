using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Gate : MonoBehaviour
{
    [SerializeField] private string sign;
    [SerializeField] private int number;

    void Start()
    {
        GetComponentInChildren<TMP_Text>().text = sign + number;
    }

    public string GetSign()
    {
        return sign;
    }

    public int GetNumber()
    {
        return number;
    }
}
