using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellShapeBall : SpellShape
{
    private float maxSize = 2.0f;
    private float fullTime = .6f;
    private float increment;

    public SpellShapeBall()
    {
        increment = (maxSize - 1.0f) / fullTime;
        manaCost = 0;
        manaMultiplier = 1.75f;
    }

    public override void Start(SpellScript self)
    {
        base.Start(self);
        // Create ball collider
        SphereCollider boltCollider = self.gameObject.AddComponent<SphereCollider>();
        boltCollider.isTrigger = true;
        boltCollider.radius = 0.75f;
    }

    public override void Update(SpellScript self, List<GameObject> others = null)
    {
        Vector3 scale = self.transform.localScale;
        float inc = increment * Time.deltaTime;
        scale += new Vector3(inc, inc, inc);
        self.transform.localScale = scale;
        if (scale.x >= maxSize)
            DestroyAndStartChildren(self.gameObject);
    }

    public override bool Trigger(SpellScript self, GameObject other)
    {
        self.effectMagnitudeScale = Time.deltaTime/fullTime;
        return true;
    }
}
