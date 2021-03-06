﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvButtons : MonoBehaviour {
    GameObject cell;
    public GameObject prefab;
    string PathPanel;
    string PathPopup;
    GameObject popap;
    Button[] menu;
    string[] menuname = new string[3];
    public bool isEvent,isEndAnimButton;
    Inventory inventorrrrry;

    private void Start()
    {
        menu = new Button[3];
        isEndAnimButton = false;
        inventorrrrry = GameObject.Find("GlobalScripts").GetComponent<Inventory>();
        isEvent = false;
        menuname[0] = "Использовать";
        menuname[1] = "В Хот-Бар";
        menuname[2] = "Выбросить";
        cell = this.gameObject;
        PathPanel = "prefabs/Panel";
        PathPopup = "prefabs/Popup";
    }

    IEnumerator TextSize(int i) {

        for (byte j = 0; j < 250; j += 50)
        {
            menu[i].GetComponentInChildren<Text>().color = new Color32(255, 255, 255, j);
            yield return new WaitForSeconds(0.01f);
        }

    }
    IEnumerator MenuExic()
    {
        for (int i = 0; i < 3; i++)
        {
            menu[i] = Instantiate<Button>(Resources.Load<Button>(PathPopup));
            menu[i].transform.SetParent(popap.transform);
            menu[i].transform.localScale = new Vector2(1f, 1f);
            menu[i].GetComponentInChildren<Text>().color = new Color32(25, 25, 25, 0);
            menu[i].GetComponentInChildren<Text>().text = menuname[i]; 
            yield return StartCoroutine(TextSize(i));
        }
        menu[0].onClick.AddListener(Equip);
        menu[1].onClick.AddListener(HotBar);
        menu[2].onClick.AddListener(Remove);
        isEndAnimButton = true;
    }

    void Remove()
    {
            inventorrrrry.items.Remove(cell.GetComponentInChildren<HelperItems>().it);
            GameObject nobj = Instantiate<GameObject>(Resources.Load<GameObject>(cell.GetComponentInChildren<HelperItems>().helpsprefab));
            nobj.transform.position = new Vector2(inventorrrrry.playerpos.position.x + 5 * inventorrrrry.playerscale, inventorrrrry.playerpos.position.y+5);
            DestroyObject(cell.transform.GetChild(0).gameObject);
            cell.GetComponent<Image>().sprite = Resources.Load<Sprite>("sprites/boxSelect");
            if (isEvent && isEndAnimButton)
            {
               Destroy(popap);
               isEvent = false;
            }
    }

    void HotBar() {
        inventorrrrry.hotbar.Add(cell.GetComponentInChildren<HelperItems>().it);
        {
            Destroy(popap);
            isEvent = false;
        }
        inventorrrrry.HelperBarOpen();
    }
    void HotBar(Item item) {
        inventorrrrry.hotbar.Add(item);
        inventorrrrry.HelperBarOpen();
    }
    void HotBarDel(Item item) {
        inventorrrrry.hotbar.Remove(item);
    }

    void ItemBack(int i) {
        Item temp = inventorrrrry.armor[i - 1];
        inventorrrrry.armor[i - 1] = cell.GetComponentInChildren<HelperItems>().it;
        inventorrrrry.items.Remove(cell.GetComponentInChildren<HelperItems>().it);
        inventorrrrry.items.Add(temp);
        Destroy(inventorrrrry.rhand.GetChild(0).gameObject);
        HandHelper(inventorrrrry.armor[i - 1].sprite);
    }

    private void InCellArmorHelper(int index)
    {
        if (inventorrrrry.armorInv.GetChild(index).childCount == 0)
        {
            inventorrrrry.armor[index - 1] = cell.GetComponentInChildren<HelperItems>().it;
            inventorrrrry.items.Remove(cell.GetComponentInChildren<HelperItems>().it);
            DestroyObject(cell.transform.GetChild(0).gameObject);
            cell.GetComponent<Image>().sprite = Resources.Load<Sprite>("sprites/boxSelect");
            inventorrrrry.HelperArmorOpen();
        }
        else
        {
            ItemBack(index);
            inventorrrrry.HelperInvOpen();
            inventorrrrry.HelperArmorOpen();
        }
        if (isEvent && isEndAnimButton)
        {
            Destroy(popap);
            isEvent = false;
        }

    }

    void HandHelper(string sprite) {
        GameObject newObj = Instantiate(prefab);
        newObj.transform.SetParent(inventorrrrry.rhand);
        newObj.transform.localPosition = new Vector2(0.157f, 0.057f);
        newObj.transform.localRotation = Quaternion.Euler(0f, 0f, 54.5f);
        newObj.transform.localScale = new Vector2(1f, 1f);
        newObj.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(sprite);
        newObj.GetComponent<SpriteRenderer>().sortingLayerName = "RobotLayer";
        newObj.GetComponent<SpriteRenderer>().sortingOrder = 300;
    }

    void Equip()
    {
        if (cell.GetComponentInChildren<HelperItems>().type == "hands")
        {
            HandHelper(cell.GetComponentInChildren<HelperItems>().sprite);
            InCellArmorHelper(5);
        }
        else if (cell.GetComponentInChildren<HelperItems>().type == "legs")
        {
            InCellArmorHelper(3);
        }
        else if (cell.GetComponentInChildren<HelperItems>().type == "head")
        {
            InCellArmorHelper(1);
        }
    }

    public void Enter()
    {       
        transform.localScale = new Vector2(1.05f,1.05f);
        GetComponent<Image>().color = new Color32(184, 177, 169, 164);
    }

    public void Exit()
    {
        transform.localScale = new Vector2(1f, 1f);
        GetComponent<Image>().color = new Color32(210, 205, 198, 154);
    }

    public void PopupAd()
    {
        if (cell.transform.childCount > 0 && !isEvent)
        {
            isEndAnimButton = false;
            popap = Instantiate(Resources.Load<GameObject>(PathPanel));
            popap.transform.SetParent(cell.transform.parent);
            popap.GetComponent<RectTransform>().anchorMin = cell.GetComponent<RectTransform>().anchorMin;
            popap.GetComponent<RectTransform>().anchorMax = cell.GetComponent<RectTransform>().anchorMax;
            popap.GetComponent<RectTransform>().offsetMin = cell.GetComponent<RectTransform>().offsetMin;
            popap.GetComponent<RectTransform>().offsetMax = cell.GetComponent<RectTransform>().offsetMax;
            popap.GetComponent<popHelp>().cel = this.gameObject;
            popap.transform.position = cell.transform.position;
            popap.transform.localScale = new Vector2(1.5f, 1.5f);
            StartCoroutine(MenuExic());

            isEvent = true;
        }
    }

    
   

}
