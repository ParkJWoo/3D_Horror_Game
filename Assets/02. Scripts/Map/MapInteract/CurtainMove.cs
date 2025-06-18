using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CurtainMove : MonoBehaviour, IInteractable
{
    private float targetUpY = 3f;
    private float targetDownY = 0.5f;
    private bool isCurtainUp = false;

    public event Action<IInteractable> OnInteracted;


    public void CurtainInteract()
    {
        if(isCurtainUp)
        {
            StartCoroutine(CurtainRollDown());
            isCurtainUp = false;
            return;
        }
        else
        {
            StartCoroutine(CurtainRollUp());
            isCurtainUp = true;
            return;
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
        yield break;
    }

    IEnumerator CurtainRollDown()
    {
        while (transform.position.y > targetDownY + 0.01f)
        {
            float newY = Mathf.MoveTowards(transform.position.y, targetDownY, 3f * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            yield return null;
        }
        yield break;
    }

    public void SetInterface(bool active)
    {
 
    }

    public void OnInteraction()
    {
        CurtainInteract();
    }
}
