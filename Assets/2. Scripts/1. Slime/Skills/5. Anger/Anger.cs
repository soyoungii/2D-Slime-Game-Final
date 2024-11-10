using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Anger : MonoBehaviour
{
    public GameObject angerPrefab;
    public Image cooldownImage;
    private float cooldown = 20f;
    private Slime slime;
    private GameObject currentAngerEffect;

    private void Awake()
    {
        slime = FindObjectOfType<Slime>();
    }

    public void StartSkill()
    {
        StartCoroutine(AngerCoroutine());
    }


    private IEnumerator AngerCoroutine()
    {
        while (true)
        {
            cooldownImage.fillAmount = 1f;
            currentAngerEffect = Instantiate(angerPrefab, angerPrefab.transform.position, Quaternion.identity);
            float originalDamage = slime.damage;
            slime.damage *= 2f;
            yield return new WaitForSeconds(10f);

            if (currentAngerEffect != null)
            {
                Destroy(currentAngerEffect);
            }
            slime.damage = originalDamage;
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