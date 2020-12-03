using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(ViewconeSightSensor))]
public class Enemy : PoolGameObject, IStateMachine
{
	public const int ID_STATE_ALIVE = 1 << 1; 				/// <summary>Alive's State Flag.</summary>
	public const int ID_STATE_IDLE = 1 << 2; 				/// <summary>Idle's State Flag.</summary>
	public const int ID_STATE_PLAYERONSIGHT = 1 << 3; 		/// <summary>Player On Sight's State Flag.</summary>
	public const int ID_STATE_FOLLOWPLAYER = 1 << 4; 		/// <summary>Follow Player's State Flag.</summary>
	public const int ID_STATE_ATTACK = 1 << 5; 				/// <summary>Attack's State Flag.</summary>

	protected static Player _player; 						/// <summary>Player's Static reference.</summary>

	private ViewconeSightSensor _sightSensor; 				/// <summary>SightSensor's Component.</summary>
	private Health _health; 								/// <summary>Health's Component.</summary>
	private int _state; 									/// <summary>Agent's Current State.</summary>
	private int _previousState; 							/// <summary>Agent's Previous State.</summary>
	private int _ignoreResetMask; 							/// <summary>State flags to ignore.</summary>

#region Getters/Setters:
	/// <summary>Gets and Sets player property.</summary>
	public static Player player
	{
		get { return _player; }
		protected set { _player = value; }
	}

	/// <summary>Gets and Sets state property.</summary>
	public int state
	{
		get { return _state; }
		set { _state = value; }
	}

	/// <summary>Gets and Sets previousState property.</summary>
	public int previousState
	{
		get { return _previousState; }
		set { _previousState = value; }
	}

	/// <summary>Gets and Sets ignoreResetMask property.</summary>
	public int ignoreResetMask
	{
		get { return _ignoreResetMask; }
		set { _ignoreResetMask = value; }
	}

	/// <summary>Gets sightSensor Component.</summary>
	public ViewconeSightSensor sightSensor
	{ 
		get
		{
			if(_sightSensor == null) _sightSensor = GetComponent<ViewconeSightSensor>();
			return _sightSensor;
		}
	}

	/// <summary>Gets health Component.</summary>
	public Health health
	{
		get
		{
			if(_health == null) _health = GetComponent<Health>();
			return _health;
		}
	}
#endregion

	/// <summary>Resets Enemy's instance to its default values.</summary>
	public virtual void Reset()
	{
		this.ChangeState(ID_STATE_ALIVE);
		health.Reset();
	}

	/// <summary>Callback invoked when Enemy's script is instantiated.</summary>
	private void Awake()
	{
		health.onHealthEvent += OnHealthEvent;
		sightSensor.onColliderSighted += OnColliderSighted;
		sightSensor.onColliderOccluded += OnColliderOccluded;
		sightSensor.onColliderOutOfSight += OnColliderOutOfSight;

		OnAwake();
	}

	/// <summary>Callback invoked when Enemy's instance is going to be destroyed and passed to the Garbage Collector.</summary>
	private void OnDestroy()
	{
		health.onHealthEvent -= OnHealthEvent;
		sightSensor.onColliderSighted -= OnColliderSighted;
		sightSensor.onColliderOccluded -= OnColliderOccluded;
		sightSensor.onColliderOutOfSight -= OnColliderOutOfSight;
	}

	/// <summary>Callback invoked when scene loads, one frame before the first Update's tick.</summary>
	private void Start()
	{
		if(player == null) player = Game.player;
	}

	/// <summary>Callback invoked each Physic's thread step.</summary>
	private void FixedUpdate()
	{
		/*Vector3 position = transform.position + ((player.transform.position - transform.position).normalized * movementRadius);
		Vector3 movementDirection = VVector3.DirectionWithAxes(position, transform.position, movementAxes);
		float arrivalWeight = steeringVehicle.GetArrivalWeight(transform.position + movementDirection, player.ship.steeringVehicle.radiusRange, Time.fixedDeltaTime);
		Vector3 seek = steeringVehicle.GetSeekForce(transform.position + movementDirection, arrivalWeight);

		rotation.RotateTowards(position);
		steeringVehicle.rigidbody.MovePosition(steeringVehicle.rigidbody.position + (seek * Time.deltaTime));
		steeringVehicle.UpdateVelocity(seek);*/
	}

	/// <summary>Callback internally called right after Awake.</summary>
	protected virtual void OnAwake(){ /*...*/ }

#region IFiniteStateMachineCallbacks:
	/// <summary>Enters int State.</summary>
	/// <param name="_state">int State that will be entered.</param>
	public virtual void OnEnterState(int _state)
	{
	}
	
	/// <summary>Leaves int State.</summary>
	/// <param name="_state">int State that will be left.</param>
	public virtual void OnExitState(int _state)
	{
	}

	/// <summary>Callback invoked when new state's flags are added.</summary>
	/// <param name="_state">State's flags that were added.</param>
	public virtual void OnStatesAdded(int _state)
	{
	}

	/// <summary>Callback invoked when new state's flags are removed.</summary>
	/// <param name="_state">State's flags that were removed.</param>
	public virtual void OnStatesRemoved(int _state)
	{
	}
#endregion

#region Callbacks:
	/// <summary>Callback invoked when the health of the character is depleted.</summary>
	protected virtual void OnHealthEvent(HealthEvent _event, float _amount = 0.0f)
	{
		switch(_event)
		{
			case HealthEvent.FullyDepleted:
			this.RemoveStates(ID_STATE_ALIVE);
			OnObjectDeactivation();
			break;
		}
	}

	/// <summary>Callback invoked when a Collider is sighted.</summary>
	/// <param name="_collider">Collider sighted.</param>
	protected virtual void OnColliderSighted(Collider _collider)
	{
		if(_collider.gameObject.layer == Game.data.playerLayer) this.AddStates(ID_STATE_PLAYERONSIGHT);
	}

	/// <summary>Callback invoked when a Collider is occluded.</summary>
	/// <param name="_collider">Collider occluded.</param>
	protected virtual void OnColliderOccluded(Collider _collider)
	{

	}

	/// <summary>Callback invoked when a Collider is out of sight.</summary>
	/// <param name="_collider">Collider out of sight.</param>
	protected virtual void OnColliderOutOfSight(Collider _collider)
	{
		if(_collider.gameObject.layer == Game.data.playerLayer) this.RemoveStates(ID_STATE_PLAYERONSIGHT);
	}
#endregion

	/// <returns>States to string</returns>
	public virtual string StatesToString()
	{
		StringBuilder builder = new StringBuilder();

		builder.AppendLine("State Mask:");
		builder.Append("Alive: ");
		builder.AppendLine(this.HasState(ID_STATE_ALIVE).ToString());
		builder.Append("Idle: ");
		builder.AppendLine(this.HasState(ID_STATE_IDLE).ToString());
		builder.Append("PlayerOnSight: ");
		builder.AppendLine(this.HasState(ID_STATE_PLAYERONSIGHT).ToString());
		builder.Append("FollowPlayer: ");
		builder.AppendLine(this.HasState(ID_STATE_FOLLOWPLAYER).ToString());
		builder.Append("Attack: ");
		builder.Append(this.HasState(ID_STATE_ATTACK).ToString());

		return builder.ToString();
	}

	/// <returns>String representing enemy's stats.</returns>
	public override string ToString()
	{
		StringBuilder builder = new StringBuilder();

		builder.Append(name);
		builder.AppendLine(" Enemy: ");
		builder.AppendLine(StatesToString());
		builder.AppendLine();
		builder.AppendLine(health.ToString());

		return builder.ToString();
	}
}
}