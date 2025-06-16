using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CurtainMove : MonoBehaviour
{
    private float targetUpY = 3f;
    private float targetDownY = 0.5f;
    private bool isCurtainUp = false;

    void Start()
    {
        StartCoroutine(CurtainRollUp());
    }

    public void CurtainInteract()
    {
        if(isCurtainUp)
        {
            StartCoroutine(CurtainRollDown());
        }
        else
        {
            StartCoroutine(CurtainRollUp());
        }
    }

    IEnumerator CurtainRollUp()
    {
        while (transform.position.y < targetUpY - 0.01f)
        {
            float newY = Mathf.MoveTowards(transform.position.y, targetUpY, 3f * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            yield return null;
        }
    }

    IEnumerator CurtainRollDown()
    {
        while (transform.position.y > targetUpY + 0.01f)
        {
            float newY = Mathf.MoveTowards(transform.position.y, targetDownY, 3f * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            yield return null;
        }
    }


}
