﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileObject : MapObject
{
	void Start ()
    {
	}
	
	void Update ()
    {
	}
    
    /*
    private void OnMouseDown()
    {
        Debug.Log("Check " + Input.mousePosition);
        gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
    }
    */


    // Init

    public void Init(Sprite sprite)
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
    }
}
