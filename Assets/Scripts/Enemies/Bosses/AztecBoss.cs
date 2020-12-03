using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
[RequireComponent(typeof(Tackle))]
[RequireComponent(typeof(KeepOffsetFromPlayer))]
[RequireComponent(typeof(Stomp))]
//[RequireComponent(typeof(SteeringVehicle))]
public class AztecBoss : Boss
{
	[Space(5f)]
	[Header("Aztec Boss' Attributes:")]
	[SerializeField] private Renderer _mainRenderer; 							/// <summary>Boss' main Renderer.</summary>
	[Space(5f)]
	[Header("Gem's Attributes:")]
	[SerializeField] private DischargeLaserBeam _dischargeLaserBeam; 			/// <summary>DischargeLaserBeam's Component.</summary>
	[SerializeField] private Renderer _gemRenderer; 							/// <summary>Gem's Renderer.</summary>
	[SerializeField] private MaterialTag _colorTag; 							/// <summary>Color's Tag fot Material.</summary>
	[SerializeField] private MaterialTag _emissionColorTag; 					/// <summary>Color's Tag fot Material.</summary>
	[SerializeField] private Color _stage1Color; 								/// <summary>Stage 1's Color for the Gem.</summary>
	[SerializeField] private Color _stage1EmissionColor; 						/// <summary>Stage 1's Emission Color for the Gem.</summary>
	[SerializeField] private Color _stage2Color; 								/// <summary>Stage 2's Color for the Gem.</summary>
	[SerializeField] private Color _stage2EmissionColor; 						/// <summary>Stage 1's Emission Color for the Gem.</summary>
	[SerializeField] private Color _stage3Color; 								/// <summary>Stage 3's Color for the Gem.</summary>
	[SerializeField] private Color _stage3EmissionColor; 						/// <summary>Stage 1's Emission Color for the Gem.</summary>
	[Space(5f)]
	[Header("Eyes' Attributes:")]
	[SerializeField] private Health _leftEyeHealth; 							/// <summary>Left Eye's Health.</summary>
	[SerializeField] private Health _rightEyeHealth; 							/// <summary>Right Eye's Health.</summary>
	[SerializeField] private EulerRotation _targetRotation; 					/// <summary>Eyes' Target Rotation.</summary>
	[SerializeField] private Renderer _leftEyeRenderer; 						/// <summary>Left Eye's Renderer.</summary>
	[SerializeField] private Renderer _rightEyeRenderer; 						/// <summary>Right Eye's Renderer.</summary>
	[SerializeField] private Texture2D _stage1And2EyeTexture; 					/// <summary>Eye Texture for stages 1 and 2.</summary>
	[SerializeField] private Texture2D _stage2And3EyeTexture; 					/// <summary>Eye Texture for stages 2 and 3.</summary>
	[SerializeField] private MaterialTag _albedoTag; 							/// <summary>Albedo's Tag for the Material.</summary>
	[Space(5f)]
	[Header("Rubble Shooting's Attributes:")]
	[SerializeField] private ShootParabolaProjectile _shootParabolaProjectile; 	/// <summary>ShootProjectile's Ability.</summary>
	[SerializeField] private FloatRange _radiusMargin; 							/// <summary>Radius' Margin of Error when shooting the Rubbles.</summary>
	[SerializeField] private FloatRange _rubbleShotInterval; 					/// <summary>Shoot interval wait between each rubble.</summary>
	[SerializeField] private float _parabolaTime; 								/// <summary>Rubble's Trajectory Time.</summary>
	[Space(5f)]
	[SerializeField] private ShootProjectile _shootProjectile; 					/// <summary>ShootProjectile's Ability for the normal projectile's.</summary>
	[SerializeField] private ShootProjectile _shootMandalas; 					/// <summary>ShootProjectile's Ability for the Mandalas.</summary>
	/*[Space(5f)]
	[Header("Stomp's Attributes:")]*/
	private SteeringVehicle _vehicle; 											/// <summary>SteeringVehicle's Component.</summary>
	private KeepOffsetFromPlayer _keepOffsetFromPlayer; 						/// <summary>Keep Offset from Player's Component.</summary>
	private Tackle _tackle; 													/// <summary>Tackle's Component.</summary>
	private Stomp _stomp; 														/// <summary>Stomp's Component.</summary>
	private Coroutine coroutine; 												/// <summary>Coroutine's Reference.</summary>

#region Getters/Setters:
	/// <summary>Gets mainRenderer property.</summary>
	public Renderer mainRenderer { get { return _mainRenderer; } }

	/// <summary>Gets gemRenderer property.</summary>
	public Renderer gemRenderer { get { return _gemRenderer; } }

	/// <summary>Gets leftEyeRenderer property.</summary>
	public Renderer leftEyeRenderer { get { return _leftEyeRenderer; } }

	/// <summary>Gets rightEyeRenderer property.</summary>
	public Renderer rightEyeRenderer { get { return _rightEyeRenderer; } }

	/// <summary>Gets dischargeLaserBeam property.</summary>
	public DischargeLaserBeam dischargeLaserBeam { get { return _dischargeLaserBeam; } }

	/// <summary>Gets colorTag property.</summary>
	public MaterialTag colorTag { get { return _colorTag; } }

	/// <summary>Gets emissionColorTag property.</summary>
	public MaterialTag emissionColorTag { get { return _emissionColorTag; } }

	/// <summary>Gets albedoTag property.</summary>
	public MaterialTag albedoTag { get { return _albedoTag; } }

	/// <summary>Gets stage1Color property.</summary>
	public Color stage1Color { get { return _stage1Color; } }

	/// <summary>Gets stage1EmissionColor property.</summary>
	public Color stage1EmissionColor { get { return _stage1EmissionColor; } }

	/// <summary>Gets stage2Color property.</summary>
	public Color stage2Color { get { return _stage2Color; } }

	/// <summary>Gets stage2EmissionColor property.</summary>
	public Color stage2EmissionColor { get { return _stage2EmissionColor; } }

	/// <summary>Gets stage3Color property.</summary>
	public Color stage3Color { get { return _stage3Color; } }

	/// <summary>Gets stage3EmissionColor property.</summary>
	public Color stage3EmissionColor { get { return _stage3EmissionColor; } }

	/// <summary>Gets leftEyeHealth property.</summary>
	public Health leftEyeHealth { get { return _leftEyeHealth; } }

	/// <summary>Gets rightEyeHealth property.</summary>
	public Health rightEyeHealth { get { return _rightEyeHealth; } }

	/// <summary>Gets targetRotation property.</summary>
	public EulerRotation targetRotation { get { return _targetRotation; } }

	/// <summary>Gets stage1And2EyeTexture property.</summary>
	public Texture2D stage1And2EyeTexture { get { return _stage1And2EyeTexture; } }

	/// <summary>Gets stage2And3EyeTexture property.</summary>
	public Texture2D stage2And3EyeTexture { get { return _stage2And3EyeTexture; } }

	/// <summary>Gets shootProjectile property.</summary>
	public ShootProjectile shootProjectile { get { return _shootProjectile; } }

	/// <summary>Gets shootMandalas property.</summary>
	public ShootProjectile shootMandalas { get { return _shootMandalas; } }

	/// <summary>Gets shootParabolaProjectile property.</summary>
	public ShootParabolaProjectile shootParabolaProjectile { get { return _shootParabolaProjectile; } }

	/// <summary>Gets radiusMargin property.</summary>
	public FloatRange radiusMargin { get { return _radiusMargin; } }

	/// <summary>Gets rubbleShotInterval property.</summary>
	public FloatRange rubbleShotInterval { get { return _rubbleShotInterval; } }

	/// <summary>Gets parabolaTime property.</summary>
	public float parabolaTime { get { return _parabolaTime; } }

	/// <summary>Gets vehicle Component.</summary>
	public SteeringVehicle vehicle
	{ 
		get
		{
			if(_vehicle == null) _vehicle = GetComponent<SteeringVehicle>();
			return _vehicle;
		}
	}

	/// <summary>Gets keepOffsetFromPlayer Component.</summary>
	public KeepOffsetFromPlayer keepOffsetFromPlayer
	{ 
		get
		{
			if(_keepOffsetFromPlayer == null) _keepOffsetFromPlayer = GetComponent<KeepOffsetFromPlayer>();
			return _keepOffsetFromPlayer;
		}
	}

	/// <summary>Gets tackle Component.</summary>
	public Tackle tackle
	{ 
		get
		{
			if(_tackle == null) _tackle = GetComponent<Tackle>();
			return _tackle;
		}
	}

	/// <summary>Gets stomp Component.</summary>
	public Stomp stomp
	{ 
		get
		{
			if(_stomp == null) _stomp = GetComponent<Stomp>();
			return _stomp;
		}
	}
#endregion

	/// <summary>Callback internally called right after Awake.</summary>
	protected override void OnAwake()
	{
		base.OnAwake();

		leftEyeHealth.onHealthInstanceEvent += OnHealthInstanceEvent;
		rightEyeHealth.onHealthInstanceEvent += OnHealthInstanceEvent;
		stomp.onStateChange += OnStompStateChange;
		tackle.onStateChange += OnTackleStateChange;

		keepOffsetFromPlayer.ignoreAxes = Axes3D.None;
		//stomp.Begin();
	}
	/// <summary>Updates AztecBoss's instance at each Physics Thread's frame.</summary>
	private void FixedUpdate()
	{
		keepOffsetFromPlayer.KeepOffset();
	}

	/// <summary>Callback internally called when the Boss advances stage.</summary>
	protected override void OnStageChanged()
	{
		float halfHP = health.maxHP * 0.5f;

		leftEyeHealth.SetMaxHP(halfHP, true);
		rightEyeHealth.SetMaxHP(halfHP, true);

		switch(currentStage)
		{
			case STAGE_1:
			break;

			case STAGE_2:
			break;

			case STAGE_3:
			break;
		}
	}

	/// <summary>Callback invoked when the Stomp's state change.</summary>
	/// <param name="_state">New Stomp's State.</param>
	private void OnStompStateChange(StompState _state)
	{
		Axes3D axes = Axes3D.None;

		switch(_state)
		{
			case StompState.Unactive:
			axes = keepOffsetFromPlayer.ignoreAxes;
			axes &= ~Axes3D.Z;
			keepOffsetFromPlayer.ignoreAxes = axes;
			break;

			case StompState.Stomping:
			axes = keepOffsetFromPlayer.ignoreAxes;
			axes |= Axes3D.Z;
			keepOffsetFromPlayer.ignoreAxes = axes;
			break;

			case StompState.Returning:
			this.DispatchCoroutine(ref coroutine);
			break;

			case StompState.OnGround:
			this.StartCoroutine(RubbleShootingRoutine(), ref coroutine);
			break;
		}
	}

	/// <summary>Callback invoked when the Tackle's state change.</summary>
	/// <param name="_state">New Tackle's State.</param>
	private void OnTackleStateChange(TackleState _state)
	{
		Axes3D axes = Axes3D.None;

		switch(_state)
		{
			case TackleState.Deactivated:
			axes = keepOffsetFromPlayer.ignoreAxes;
			axes &= ~Axes3D.Z;
			axes &= ~Axes3D.Y;
			keepOffsetFromPlayer.ignoreAxes = axes;
			break;

			case TackleState.Activated:
			axes = keepOffsetFromPlayer.ignoreAxes;
			axes |= Axes3D.Z;
			axes |= Axes3D.Y;
			keepOffsetFromPlayer.ignoreAxes = axes;
			break;
		}
	}

	/// <summary>Callback invoked when one of the Eyes receives a Health event.</summary>
	/// <param name="_health">Eye's Health instance that invoked the event.</param>
	/// <param name="_event">Type of Health Event.</param>
	/// <param name="_amount">Amount of health change [0.0f by default].</param>
	private void OnHealthInstanceEvent(Health _health, HealthEvent _event, float _amount = 0.0f)
	{
		switch(_event)
		{
			case HealthEvent.Depleted:
			_health.transform.localRotation = Quaternion.Lerp(Quaternion.identity, targetRotation, 1.0f - _health.hpRatio);
			health.GiveDamage(_amount);
			break;

			case HealthEvent.FullyDepleted:
			_health.transform.localRotation = targetRotation;
			break;
		}
	}

	/// <returns>String representing enemy's stats.</returns>
	public override string ToString()
	{
		StringBuilder builder = new StringBuilder();

		builder.AppendLine(base.ToString());
		builder.Append("Left Eye's Health: ");
		builder.AppendLine(leftEyeHealth.ToString());
		builder.Append("Right Eye's Health: ");
		builder.AppendLine(rightEyeHealth.ToString());

		return builder.ToString();
	}

#region Coroutines:
	/// <summary>Mandala Shooting's Routine.</summary>
	private IEnumerator MandalaShootingRoutine()
	{
		while(true)
		{
			if(!shootMandalas.onCooldown)
			{
				shootMandalas.ChangeRandomProjectileIndex();
				shootMandalas.Shoot();
			}
			yield return null;
		}
	}

	/// <summary>Projectile Shooting's Routine.</summary>
	private IEnumerator ProjectileShootingRoutine()
	{
		Vector3 direction = player.mateo.transform.position - shootProjectile.transform.position;
		
		while(true)
		{
			if(!shootProjectile.onCooldown)
			{
				shootProjectile.transform.rotation = Quaternion.LookRotation(direction);
				direction = player.mateo.transform.position - shootProjectile.transform.position;
			}
		
			yield return null; 
		}
	}

	/// <summary>Laser Beam's Emission Routine.</summary>
	private IEnumerator LaserBeamEmissionRoutine()
	{
		dischargeLaserBeam.transform.forward = transform.forward;
		dischargeLaserBeam.Begin();

		while(true)
		{
			if(dischargeLaserBeam.state != LaserState.OnCooldown)
			dischargeLaserBeam.PointTowards(player.mateo.transform.position);
			yield return null;
		}
	}

	/// <summary>Rubble Shooting's Routine.</summary>
	private IEnumerator RubbleShootingRoutine()
	{
		SecondsDelayWait wait = new SecondsDelayWait(rubbleShotInterval.Random());;
		Vector3 point = Vector3.zero;

		while(true)
		{
			point = VVector3.RandomPointOnSphere(player.ProjectForwardPosition(parabolaTime), radiusMargin.Random());
			shootParabolaProjectile.ChangeRandomProjectileIndex();
			shootParabolaProjectile.Shoot(point, parabolaTime);

			while(wait.MoveNext()) yield return null;

			wait.Reset();
			wait.waitDuration = rubbleShotInterval.Random();
		}
	}
#endregion

}
}