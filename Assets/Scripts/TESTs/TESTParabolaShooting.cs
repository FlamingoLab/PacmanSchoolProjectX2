using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Flamingo;
using Voidless;

[RequireComponent(typeof(ShootParabolaProjectile))]
public class TESTParabolaShooting : MonoBehaviour
{
	[SerializeField] private Vector3 target; 					/// <summary>Target.</summary>
	[SerializeField] private FloatRange timeRange; 				/// <summary>Time Range.</summary>
	[SerializeField] private FloatRange projectionRange; 		/// <summary>Time Projection's Range.</summary>
	private ShootParabolaProjectile shootParabolaProjectile;
	private float time;
	private float current;

	/// <summary>Draws Gizmos on Editor mode.</summary>
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(target, 0.2f);
	}

	/// <summary>TESTParabolaShooting's instance initialization when loaded [Before scene loads].</summary>
	private void Awake()
	{
		shootParabolaProjectile = GetComponent<ShootParabolaProjectile>();
		ResetTime();
	}

	/// <summary>Updates TESTParabolaShooting's instance at each frame.</summary>
	private void Update()
	{
		if(current < time) current += Time.deltaTime;
		else
		{
			shootParabolaProjectile.Shoot(target, Random.Range(projectionRange.Min(), projectionRange.Max()));
			ResetTime();
		}
	}

	private void ResetTime()
	{
		time = Random.Range(timeRange.Min(), timeRange.Max());
		//time = 2f;
		current = 0.0f;
	}
}