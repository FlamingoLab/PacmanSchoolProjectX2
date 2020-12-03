using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
public enum HealthEvent
{
    Depleted,
    Replenished,
    InvincibilityEnds,
    FullyDepleted,
    FullyReplenished
}

/// <summary>Event invoked when a Health's event has occured.</summary>
/// <param name="_event">Type of Health Event.</param>
/// <param name="_amount">Amount of health that changed [0.0f by default].</param>
public delegate void OnHealthEvent(HealthEvent _event, float _amount = 0.0f);

/// <summary>Event invoked when an event of a Health's instance has occured.</summary>
/// <param name="_health">Health's Instance.</param>
/// <param name="_event">Type of Health Event.</param>
/// <param name="_amount">Amount of health that changed [0.0f by default].</param>
public delegate void OnHealthInstanceEvent(Health _health, HealthEvent _event, float _amount = 0.0f);

public class Health : MonoBehaviour
{
	public event OnHealthEvent onHealthEvent;                   /// <summary>OnHealthEvent Event Delegate.</summary>
    public event OnHealthInstanceEvent onHealthInstanceEvent;   /// <summary>OnHealthInstanceEvent Event Delegate.</summary>

    [SerializeField] private float _maxHP;                      /// <summary>Maximum amount of Health.</summary>
    [SerializeField] private float _invincibilityDuration;      /// <summary>Cooldown duration when on invincible mode.</summary>
    private Cooldown _cooldown;                                 /// <summary>Cooldown's reference.</summary>
    private float _hp;                                          /// <summary>Current HP.</summary>

#region Getters/Setters:
    /// <summary>Gets and Sets maxHP property.</summary>
    public float maxHP
    {
    	get { return _maxHP; }
    	set { _maxHP = value; }
    }

    /// <summary>Gets and Sets invincibilityDuration property.</summary>
    public float invincibilityDuration
    {
        get { return _invincibilityDuration; }
        set { _invincibilityDuration = value; }
    }

    /// <summary>Gets and Sets hp property.</summary>
    public float hp
    {
    	get { return _hp; }
    	set { _hp = value; }
    }

    /// <summary>Gets hpRatio property.</summary>
    public float hpRatio { get { return hp / maxHP; } }

    /// <summary>Gets invincibilityProgress property.</summary>
    public float invincibilityProgress { get { return cooldown.progress; } }

    /// <summary>Gets and Sets cooldown property.</summary>
    public Cooldown cooldown
    {
        get { return _cooldown; }
        private set { _cooldown = value; }
    }

    /// <summary>Gets onInvincibility property.</summary>
    public bool onInvincibility { get { return cooldown.onCooldown; } }
#endregion

    /// <summary>Resets Health's instance to its default values.</summary>
    public void Reset()
    {
        OnCooldownEnds();
        hp = maxHP;
    }

    /// <summary>Health's instance initialization when loaded [Before scene loads].</summary>
    private void Awake()
    {
    	hp = maxHP;
        cooldown = new Cooldown(this, invincibilityDuration);
        cooldown.OnCooldownEnds.AddListener(OnCooldownEnds);
    }

    /// <summary>Assigns damage to this Health Container.</summary>
    /// <param name="_damage">Damage to inflict.</param>
    /// <param name="_applyInvincibility">Apply Invincibility? True by default.</param>
    public void GiveDamage(float _damage, bool _applyInvincibility = true)
    {
        /// If the current state is onInvincibility or the damage to receive is less or equal than '0', do nothing.
        if(onInvincibility || _damage <= 0.0f) return;

        _damage = _damage.Clamp(0.0f, hp);
    	hp -= _damage;
    	
    	if(hp > 0.0f)
        {
            if(onHealthEvent != null) onHealthEvent(HealthEvent.Depleted, _damage);
            if(onHealthInstanceEvent != null) onHealthInstanceEvent(this, HealthEvent.Depleted, _damage);

            if(invincibilityDuration > 0.0f && _applyInvincibility)
            BeginInvincibilityCooldown();

        } else if(hp == 0.0)
        {
            if(onHealthEvent != null) onHealthEvent(HealthEvent.FullyDepleted);
            if(onHealthInstanceEvent != null) onHealthInstanceEvent(this, HealthEvent.FullyDepleted, _damage);
        }
    }

    /// <summary>Replenishes Health.</summary>
    /// <param name="_amount">Amount to replenish.</param>
    public void ReplenishHealth(float _amount)
    {
        if(_amount <= 0.0f) return;

        _amount = _amount.Clamp(0.0f, maxHP - hp);
        hp += _amount;

        if(hp < maxHP)
        {
            if(onHealthEvent != null) onHealthEvent(HealthEvent.Replenished, _amount);
            if(onHealthInstanceEvent != null) onHealthInstanceEvent(this, HealthEvent.Replenished, _amount);

        } else if(hp == maxHP)
        {
            if(onHealthEvent != null) onHealthEvent(HealthEvent.FullyReplenished, _amount);
            if(onHealthInstanceEvent != null) onHealthInstanceEvent(this, HealthEvent.FullyReplenished, _amount);
        }
    }

    /// <summary>Sets Maximum HP.</summary>
    /// <param name="_maxHP">Max HP.</param>
    /// <param name="_resetHP">Reset current HP [false by default].</param>
    public void SetMaxHP(float _maxHP, bool _resetHP = false)
    {
        maxHP = _maxHP;
        if(_resetHP) Reset();
    }

    /// <summary>Begins BeginInvincibility's Cooldown.</summary>
    private void BeginInvincibilityCooldown()
    {
        cooldown.Begin();
    }

    /// <summary>Callback internally invoked when the Invincibility Cooldown ends.</summary>
    private void OnCooldownEnds()
    {
        //if(cooldown != null) cooldown.End(); /// This provokes a StackOverflowException...
        if(onHealthEvent != null) onHealthEvent(HealthEvent.InvincibilityEnds);
        if(onHealthInstanceEvent != null) onHealthInstanceEvent(this, HealthEvent.InvincibilityEnds);
    }

    /// <returns>String representing this Health's instance.</returns>
    public string ToString()
    {
        StringBuilder builder = new StringBuilder();

        builder.AppendLine("Health:");
        builder.Append("Current HP: ");
        builder.AppendLine(hp.ToString());
        builder.Append("Max HP: ");
        builder.AppendLine(maxHP.ToString());
        builder.Append("On Invincibility: ");
        builder.AppendLine(onInvincibility.ToString());

        return builder.ToString();
    }
}
}