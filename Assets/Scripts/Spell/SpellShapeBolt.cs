using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellShapeBolt : SpellShape
{
    public const float forwardVelocity = 10f;
    public override void Run(GameObject self, List<GameObject> others = null)
    {
        // Travel forward
    }

    public override void Start(GameObject self)
    {
        // Create rigidbody and move it forward
        Rigidbody boltBody = self.AddComponent<Rigidbody>() as Rigidbody;
        boltBody.drag = 0;
        boltBody.useGravity = false;
        boltBody.velocity = self.transform.up * forwardVelocity;

        // Create Light Object
        Light boltLight = self.AddComponent<Light>() as Light;
        boltLight.color = Color.yellow;
        boltLight.intensity = 2.0f;


        // Create bolt collider
        CapsuleCollider boltCollider = self.AddComponent<CapsuleCollider>() as CapsuleCollider;
        boltCollider.isTrigger = true;
        boltCollider.radius = 0.25f;
        boltCollider.height = 0.75f;
    }

    public override bool Trigger(GameObject self, GameObject other, List<SpellEffect> effects)
    {
        // Trigger spell effect
        foreach (SpellEffect effect in effects)
            effect.Trigger(self, other);
        return true;
    }
}
