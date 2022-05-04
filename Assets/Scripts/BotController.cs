using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotController : BotControllerBase
{
    private void Start()
    {
        SetupWeapon();
        botInfos.points = GameObject.FindGameObjectsWithTag("Points");
        botInfos.rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        BehaviorTreeCheckStates();
        BehaviourStatesDefinition();
        Gravity();
    }
    void Gravity()
    {
        Vector2 velocity = Vector2.Lerp(new Vector2(botInfos.rb.velocity.x, botInfos.rb.velocity.y), new Vector2(0, 0), botInfos.propulsionGravity * Time.deltaTime);

        botInfos.rb.velocity = new Vector2(velocity.x, velocity.y);
    }
}
