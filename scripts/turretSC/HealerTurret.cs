using System.Collections;
using UnityEngine;

public class HeallerTurret : MonoBehaviour
{
    public Animator HealerAnim;
    public GameObject rangeHighlighter;

    private bool rangeActive = false;

    private void Update()
    {
    }

    public void StopAnim()
    {
        StartCoroutine(StopAnimation());
    }

    IEnumerator StopAnimation()
    {
        yield return new WaitForSeconds(0.5f);
        HealerAnim.SetBool("Healing", false);
            rangeHighlighter.SetActive(false);
    }
}
