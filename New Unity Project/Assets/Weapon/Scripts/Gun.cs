using UnityEngine;
using System.Collections;

public class DamageManager
{
	public static DamageManager Instance;

	void Awake () 
	{
		Instance = this;
	}

    float powerUpTimer;

    void Update()
    {
        powerUpTimer -= Time.deltaTime;
    }

    public int ComputeDamage(Weapon weapon)
    {
        int bonus = 10;
        if (powerUpTimer > 0f)
        {
            bonus *= 2;
        }
        return weapon.damage * bonus; 
    }
}

public class Gun: Weapon 
{
	public Transform muzzleTransform;

    public override void Fire()
    {
        Transform cameraTransform = Camera.main.transform;
        Ray ray = new Ray( cameraTransform.position + cameraTransform.forward,
                          cameraTransform.forward );
        RaycastHit hitInfo = new RaycastHit();
        if( Physics.Raycast( ray, out hitInfo, range ) )
        {
            Health health = hitInfo.collider.GetComponent<Health>();
            if( health )
            {
                hitInfo.rigidbody.AddExplosionForce( 100f, hitInfo.point, 1f );
				health.TakeDamage( DamageManager.Instance.ComputeDamage(this));

                VFXManager.Instance.Spawn( "BloodSplatter", hitInfo.point, Quaternion.identity );
            }
            else
            {
                Quaternion rotation = Quaternion.FromToRotation( Vector3.forward, hitInfo.normal );
                VFXManager.Instance.Spawn( "Dust", hitInfo.point, rotation );
            }
        }
        VFXManager.Instance.Spawn( "Muzzleflare", muzzleTransform.position, muzzleTransform.rotation );
    }
}