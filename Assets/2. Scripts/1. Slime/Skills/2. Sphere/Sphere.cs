using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;
using UnityEngine.UI;


public class Sphere : MonoBehaviour
{
    public GameObject spherePrefab;
    private GameObject sphereInstance;
    public Image cooldownImage;
    private float cooldown = 3f;

    private bool istrue = true;

    public void StartSkill()
    {
        StartCoroutine(SphereCoroutine());
    }

    private IEnumerator SphereCoroutine()
    {
        while (true)
        {
            if (istrue)
            {
                cooldownImage.fillAmount = 1f;
                sphereInstance = Instantiate(spherePrefab, spherePrefab.transform.position, Quaternion.identity);
                Destroy(sphereInstance, 5f);
            }
            istrue = false;

            if (sphereInstance != null)
            {
                istrue = true;
            }

            yield return StartCoroutine(Cooldown());
        }
    }

    private IEnumerator Cooldown()
    {
        float elapsed = 0f;
        while (elapsed < cooldown)
        {
            elapsed += Time.deltaTime;
            cooldownImage.fillAmount = 1f - (elapsed / cooldown);
            yield return null;
        }
    }

}