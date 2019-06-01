using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellShapeSelf : SpellShape
{
    public SpellShapeSelf()
    {
        manaCost = 0;
        manaMultiplier = 1.0f;
    }

    public override void Start(SpellScript self)
    {
        base.Start(self);
        // Create ball collider
        SphereCollider boltCollider = self.gameObject.AddComponent<SphereCollider>();
        boltCollider.isTrigger = true;
        boltCollider.radius = 0.75f;
        Transform playerTransform = self.parent.transform;
        self.transform.position = playerTransform.position;
        self.transform.rotation = playerTransform.rotation;
    }

    public override bool Trigger(SpellScript self, GameObject other)
    {
        return true;
    }
}
