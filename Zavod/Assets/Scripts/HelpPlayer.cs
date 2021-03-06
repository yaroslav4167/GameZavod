﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HelpPlayer : MonoBehaviour {
    public int maxHealth = 100;
    public int maxEnergy = 100;
    public int energy = 100;
    public int health = 100;
    public int damage = 3;
    public RectTransform barHp;
    public RectTransform barEn;
    bool fullEn = true;
    bool corotin = true;

    private void Start()
    {
        Physics2D.IgnoreLayerCollision(9, 10, true);
    }

    public void UpdateHpBar()
    {
        float with = barHp.rect.width;
        with -= with * 0.3f;

        float value = -with + (with / (float)maxHealth * (float)health);
        barHp.offsetMin = new Vector2(value, 0f);
        barHp.offsetMax = new Vector2(value, 0f);
    }
    public void UpdateEnBar()
    {
        float with = barEn.rect.width;
        with -= with * 0.3f;

        float value = -with +(with/ (float)maxEnergy * (float)energy);
        barEn.offsetMin = new Vector2(value,0f);
        barEn.offsetMax = new Vector2(value, 0f);
    }


    void EnergyAdd(int count)
    {
        energy = Mathf.Clamp(energy + count, 0, maxEnergy);
        UpdateEnBar();
    }

    void HpAdd(int count)
    {
        health = Mathf.Clamp(health + count, 0, maxHealth);
        UpdateHpBar();

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            HpAdd(-10);
        }
    }

    IEnumerator plusEnergy()
    {
        while (energy < maxEnergy)
        {
            EnergyAdd(2);
            UpdateEnBar();
            yield return new WaitForSeconds(0.1f);
        }
        corotin = true;
    }

    private void FixedUpdate()
    {
        if (energy<maxEnergy )
        {

            if (corotin)
            {
                fullEn = false;
                corotin = false;
                if (!fullEn)
                {
                    fullEn = true;
                    StartCoroutine(plusEnergy());
                }

            }

        }        
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "takeDamage")
        {
            Debug.Log("Hit!");
            collision.gameObject.GetComponent<SpriteRenderer>().sortingOrder = -600;
            HpAdd(-10);
        }
    }
}
