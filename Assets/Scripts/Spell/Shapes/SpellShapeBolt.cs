using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellShapeBolt : SpellShape
{
    public const float forwardVelocity = 10f;

    public override void Start(SpellScript self)
    {
        base.Start(self);
        // Create rigidbody and move it forward
        Rigidbody boltBody = self.gameObject.AddComponent<Rigidbody>() as Rigidbody;
        boltBody.drag = 0;
        boltBody.useGravity = false;
        boltBody.velocity = self.transform.up * forwardVelocity;



        /*
        // Create bolt collider
        CapsuleCollider boltCollider = self.gameObject.AddComponent<CapsuleCollider>() as CapsuleCollider;
        boltCollider.isTrigger = true;
        boltCollider.radius = 0.25f;
        boltCollider.height = 0.75f;
        //*/

        // Create ball effect
        GameObject sphere = GameObject.Instantiate(Resources.Load<GameObject>("sphere"), self.transform);

        // Decay
        self.StartCoroutine("Decay", 5f);
    }

    public override bool Trigger(SpellScript self, GameObject other)
    {
        // Triggered, just return false
        return false;
    }
}
