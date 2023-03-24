using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Cell
{
    public enum Type { // Type of cell
        Invalid, // Out of bounds squares
        Initial, // First clicked square and those adjacent to it
        Empty, // Not numbered squares
        Mine, // Mine
        Number // Numbered squares
    }
    
    public Type type;
    public Vector3Int position;
    public int number;
    public bool revealed;
    public bool flagged;
    public bool exploded;
}