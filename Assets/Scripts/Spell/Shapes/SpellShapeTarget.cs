using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellShapeTarget : SpellShape
{
    public GameObject target;
    public SpellShapeTarget()
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
        self.transform.position = target.transform.position;
        self.transform.rotation = target.transform.rotation;
    }

    public override bool Trigger(SpellScript self, GameObject other)
    {
        return true;
    }
}
