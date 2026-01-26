using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Deleter : MonoBehaviour
{
    [SerializeField] private int SecToDestroy = 5;
    private void Awake()
    {
        StartCoroutine(waiter());
    }

    IEnumerator waiter()
    {
        yield return new WaitForSeconds(SecToDestroy);
        Object.Destroy(this.gameObject);
    }
}
