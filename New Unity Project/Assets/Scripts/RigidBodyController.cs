using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (CapsuleCollider))]

public class RigidBodyController : MonoBehaviour {

		
		public float speed = 10.0f;
		public float gravity = 10.0f;
		public float maxVelocityChange = 10.0f;
		public bool canJump = true;
		public float jumpHeight = 2.0f;
		private bool grounded = false;
		public float grav;
		bool isFlipped = false;
		GameObject player;
		
		
		void Awake () {
			rigidbody.freezeRotation = true;
			//rigidbody.useGravity = false;
		}
		
		void FixedUpdate () {
			if (grounded) {
			// Calculate how fast we should be moving
			Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			targetVelocity = transform.TransformDirection(targetVelocity);
			targetVelocity *= speed;
			
			// Apply a force that attempts to reach our target velocity
			Vector3 velocity = rigidbody.velocity;
			Vector3 velocityChange = (targetVelocity - velocity);
			velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
			velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
			velocityChange.y = 0;
			rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);

			// Jump
			if (canJump && Input.GetButton("Jump")) {
				rigidbody.velocity = new Vector3(velocity.x, CalculateJumpVerticalSpeed(), velocity.z);
				}

				if(Input.GetKeyDown(KeyCode.G))
				{
					if(isFlipped)
					{
					GetComponent("Player");
						Physics.gravity = new Vector3(0,-180,0);
						isFlipped = false;
						Quaternion rotation = Quaternion.Euler(0,180,0) * transform.rotation;
						transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 30f);

					}
					else
					{
					GetComponent("Player");
						Physics.gravity = new Vector3(0,180,0);
						isFlipped = true;
						Quaternion rotation = Quaternion.Euler(0,180,0) * transform.rotation;
						transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 30f);
						
					}



				}
			}
			
			// We apply gravity manually for more tuning control
		rigidbody.AddForce(new Vector3 (0, -gravity * rigidbody.mass, 0));
		grounded = false;
			
		}
		
		void OnCollisionStay () {
			grounded = true;    
		}
		
		float CalculateJumpVerticalSpeed () {
			// From the jump height and gravity we deduce the upwards speed 
			// for the character to reach at the apex.
			return Mathf.Sqrt(2 * jumpHeight * gravity);
		}
}