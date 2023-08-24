using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : SceneSwitch
{
    public Transform player;
    public float posX;
    public float posY;
    public string previous;


    public override void Start()
    {
        base.Start(); // this calls whats inside the start method from its base script (SceneSwitch) while running its own start method

        if (prevScene == previous)
        { 
            player.position = new Vector2(posX, posY);
        }
    }

}
