using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class AnimatedStars : MonoBehaviour
{
	public Vector2 velocityMultiplier = Vector2.one;
	NimMove nimMove;
	new ParticleSystem particleSystem;
	ParticleSystem.VelocityOverLifetimeModule velocity;

	void Start()
	{
		particleSystem = GetComponent<ParticleSystem>();
		velocity = particleSystem.velocityOverLifetime;
	}

	void LateUpdate()
	{
		if(nimMove == null)
		{
			nimMove = FindObjectOfType<NimMove>();
		}
		else
		{
			ParticleSystem.MinMaxCurve rateX = new ParticleSystem.MinMaxCurve();
			ParticleSystem.MinMaxCurve rateY = new ParticleSystem.MinMaxCurve();
			rateX.constantMax = nimMove.GetInput().x * velocityMultiplier.x;
			rateY.constantMax = nimMove.GetInput().y * velocityMultiplier.y;
			velocity.z = rateX;
			velocity.y = rateY;
		}
	}
}
