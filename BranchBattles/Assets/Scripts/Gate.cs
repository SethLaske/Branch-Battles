using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : Building
{
    public bool open;
    public Collider2D enterance;
    public SpriteRenderer sprite;
    // Start is called before the first frame update
    void Start()
    {
        enterance = GetComponent<Collider2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (open == true)
        {
            enterance.enabled = false;
            sprite.enabled = false;
        }
        else {
            enterance.enabled = true;
            sprite.enabled = true;
        }
    }
}
