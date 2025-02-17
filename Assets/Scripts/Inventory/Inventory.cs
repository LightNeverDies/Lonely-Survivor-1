﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    private const int SLOTS = 9;
    private List<InventorySlot> mSlots = new List<InventorySlot>();
    public event EventHandler<InventoryEventArgs> ItemAdded;
    public event EventHandler<InventoryEventArgs> ItemRemoved;
    public event EventHandler<InventoryEventArgs> ItemUsed;

    public Inventory()
    {
        for(int i =0; i<SLOTS; i++)
        {
            mSlots.Add(new InventorySlot(i));
        }
    }

    private InventorySlot FindStackableSlot(InventoryItemCollection item)
    {
        foreach(InventorySlot slot in mSlots)
        {
          if (slot.IsStackable(item))
          {
             return slot;
          }
        }
        return null;
    }

    private InventorySlot FindNextEmptySlot()
    {
        foreach(InventorySlot slot in mSlots)
        {
            if (slot.IsEmpty)
                return slot;
        }
        return null;
    }

    public void AddItem(InventoryItemCollection item)
    {
        InventorySlot freeSlot = FindStackableSlot(item);
        if(freeSlot == null)
        {
            freeSlot = FindNextEmptySlot();
        }
        if(freeSlot != null)
        {
            freeSlot.AddItem(item);
               
            if (ItemAdded != null)
            {
              ItemAdded(this, new InventoryEventArgs(item));
            }    
        }
        
    }

    internal void UseItem(InventoryItemCollection item)
    {
        if (ItemUsed != null)
        {
            ItemUsed(this, new InventoryEventArgs(item));
        }
    }

    public void RemoveItem(InventoryItemCollection item)
    {
        foreach(InventorySlot slot in mSlots)
        {
            if(slot.Remove(item))
            {
                if (ItemRemoved != null)
                {
                    ItemRemoved(this, new InventoryEventArgs(item));
                }
                break;
            }
        }
        
    }

    public void DropRemovedItem(InventoryItemCollection item)
    {
        foreach(InventorySlot slot in mSlots)
        {
            if (slot.Remove(item))
            {
                if (ItemRemoved != null)
                {
                    ItemRemoved(this, new InventoryEventArgs(item));
                }
                break;
            }
        }
    }

}
