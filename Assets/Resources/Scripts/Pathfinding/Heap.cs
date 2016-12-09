using UnityEngine;
using System.Collections;
using System;

public class Heap<T> where T : IHeapItem<T>
{
    T[] items;
    int currentItemCount; // Amount of items currently in the heap

    //=================================================================================
    // Constructor for the Heap class
    //=================================================================================
    public Heap(int maxHeapSize)
    {
        items = new T[maxHeapSize];
    }

    //=================================================================================
    // Function used to add new items to the heap
    //=================================================================================
    public void Add(T item)
    {
        item.HeapIndex = currentItemCount;
        items[currentItemCount] = item; // puts new item at the end of the array
        SortUp(item); // jump to SortUp function to check whether it's position in the array needs changing 
        currentItemCount++; // increase currentItemCount by 1
    }

    //=================================================================================
    // Removes the first item from the heap
    //=================================================================================
    public T RemoveFirst()
    {
        T firstItem = items[0];
        currentItemCount--; // decrease currentItemCount by 1
        items[0] = items[currentItemCount]; //takes item at the end of the heap and put it in the first position
        items[0].HeapIndex = 0;
        SortDown(items[0]); // jumps to sort down function
        return firstItem;
    }

    //=================================================================================
    // Used if the priority of an item needs to be changed due to a node in pathfinding
    // openSet having a change in fCost due to a new path to it.
    //=================================================================================
    public void UpdateItem(T item)
    {
        SortUp(item); // SortUp called, priority will only increase, so SortDown not necessary
    }

    //=================================================================================
    // Accessor to the number of items currently in the heap
    //=================================================================================
    public int Count
    {
        get
        {
            return currentItemCount;
        }
    }

    //=================================================================================
    // Checks if the heap contains a specific item
    //=================================================================================
    public bool Contains(T item)
    {
        return Equals(items[item.HeapIndex], item);
    }

    //=================================================================================
    // Checks parent against its children to see whether it needs to be moved down.
    //=================================================================================
    void SortDown(T item)
    {
        while (true)
        {
            int childIndexLeft = item.HeapIndex * 2 + 1;
            int childIndexRight = item.HeapIndex * 2 + 2;
            int swapIndex = 0;

            // If the parent item has a left child
            if (childIndexLeft < currentItemCount)
            {
                swapIndex = childIndexLeft; // Sets swapIndex to childIndexLeft by default

                // If the parent item also has a right child
                if (childIndexRight < currentItemCount)
                {
                    // Check to see which of the child indicies, left or right is higher (higher one becomes swapIndex)
                    if (items[childIndexLeft].CompareTo(items[childIndexRight]) < 0)
                    {
                        swapIndex = childIndexRight; // Sets swapIndex to childIndexRight if it's higher
                    }
                }

                // Checks if parent has lower priority than it's highest priority child
                if (item.CompareTo(items[swapIndex]) < 0)
                {
                    Swap(item, items[swapIndex]); // jumps to Swap function to swap the parent with it's child
                }
                // If parent's priority is higher, return 
                else
                {
                    return;
                }
            }
            // If parent has no children, return
            else
            {
                return;
            }
        }
    }

    //=================================================================================
    // Compares items with their parent, to see whether it has a higher priority.
    // The parentIndex will equal -1 if the item has a lower priority, 0 if it has the
    // same priority, or 1 if it has a higher priority.
    //=================================================================================
    void SortUp(T item)
    {
        // Calculates parent index to compare item to it's parent
        int parentIndex = (item.HeapIndex - 1) / 2;

        while (true)
        {
            T parentItem = items[parentIndex];

            // If the item has a higher priority than parent item, item needs to be swapped with parent
            if (item.CompareTo(parentItem) > 0)
            {
                Swap(item, parentItem); //Jumps to Swap function to swap item with parent item
            }

            // Once item is no longer of a higher priority, break out of the loop
            else
            {
                break;
            }

            // Otherwise continue calculating the parent index and comparing item to its new parent
            parentIndex = (item.HeapIndex - 1) / 2; 
        }
    }

    //=================================================================================
    // Swap function is used to swap item with its parent
    //=================================================================================
    void Swap(T itemA, T itemB)
    {
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;
        int itemAIndex = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAIndex;
    }
}

//=================================================================================
// Interface used so items can keep track of their index in the heap, and allows
// items to be compared to others to sort priority in the heap.
//=================================================================================
public interface IHeapItem<T> : IComparable<T>
{
    int HeapIndex
    {
        get;
        set;
    }
}
