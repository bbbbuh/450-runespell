using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragController : MonoBehaviour
{
    private bool isDragActive = false;
    private Vector2 screenPosition;
    private Vector3 worldPosition;
    private Vector3 lastPosition;
    private Spell lastDragged;
    private Camera _camera;
    private SpellSlotManager slotManager;

    private void Awake()
    {
        DragController[] controllers = FindObjectsOfType<DragController>();
        if (controllers.Length > 1)
        {
            Destroy(gameObject);
        }
        _camera = Camera.main;
        slotManager = GameObject.Find("SpellSlotManager").GetComponent<SpellSlotManager>();
        Debug.Log(slotManager);

    }

    void Update()
    {
        if (isDragActive && (Input.GetMouseButtonDown(0) || (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended)))
        {
            Drop();
            return;
        }

        if (isDragActive && screenPosition != null)
        {
            if (screenPosition.x < 0 || screenPosition.x > _camera.pixelWidth || screenPosition.y < 0 || screenPosition.y > _camera.pixelHeight)
            {
                Drop();
                lastDragged.transform.position = new Vector3(lastPosition.x, lastPosition.y, 0);
                return;
            }
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = Input.mousePosition;
            screenPosition = new Vector2(mousePos.x, mousePos.y);
        }
        else if (Input.touchCount > 0)
        {
            screenPosition = Input.GetTouch(0).position;
        }
        else
        {
            return;
        }

        worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);

        if (isDragActive)
        {
            Drag();
        }
        else
        {
            RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);
            if (hit.collider != null)
            {
                Spell draggable = hit.transform.gameObject.GetComponent<Spell>();
                if (draggable != null)
                {
                    lastDragged = draggable;
                    InitDrag();
                }
            }
        }
    }

    void InitDrag() 
    {
        isDragActive = true;
        lastPosition = worldPosition;
    }

    void Drag()
    {
        lastDragged.transform.position = new Vector2(worldPosition.x, worldPosition.y);
        PlaceSpellInSlot();
    }

    void Drop()
    {
        isDragActive = false;
    }

    void PlaceSpellInSlot()
    {
        for (int i = 0; i < 3; i++)
        {
            if (CheckCollision(lastDragged.transform.position, 0.32f, 0.32f, slotManager.Slots[i].transform.position, 0.38f, 0.38f))
            {
                SoundManager.instance.PlaySoundEffect(SoundEffectNames.SpellSlotted);
                slotManager.Slots[i].transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = lastDragged.GetComponent<SpriteRenderer>().sprite;
                slotManager.AddSpellToProjectileManager(lastDragged,i);
                Drop();
                lastDragged.gameObject.SetActive(false);
                lastDragged = null;
                return;
            }
        }
    }

    //Checks collision between any two objects
    bool CheckCollision(Vector2 pos1, float width1, float height1, Vector2 pos2, float width2, float height2)
    {
        pos1.x -= width1 / 2;
        pos1.y -= height1 / 2;
        pos2.x -= width2 / 2;
        pos2.y -= height2 / 2;
        return (pos1.x < pos2.x + width2 && pos1.x + width1 > pos2.x && pos1.y < pos2.y + height2 && pos1.y + height1 > pos2.y);
    }
}
