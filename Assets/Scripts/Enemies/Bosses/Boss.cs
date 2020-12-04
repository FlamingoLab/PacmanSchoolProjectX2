using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Voidless;

namespace Flamingo
{
public class Boss : Enemy
{
	public const int STAGE_1 = 1; 							/// <summary>Stage 1's ID.</summary>
	public const int STAGE_2 = 2; 							/// <summary>Stage 2's ID.</summary>
	public const int STAGE_3 = 3; 							/// <summary>Stage 3's ID.</summary>
	public const int STAGE_4 = 4; 							/// <summary>Stage 4's ID.</summary>
	public const int STAGE_5 = 5; 							/// <summary>Stage 5's ID.</summary>

	[Space(5f)]
	[Header("Boss' Attributes:")]
	[SerializeField] private int _stages; 					/// <summary>Boss' Stages.</summary>
	[SerializeField] private float[] _healthDistribution; 	/// <summary>Health Distribution across the Stages.</summary>
	private int _currentStage; 								/// <summary>Boss' Current Stage.</summary>

	/// <summary>Gets and Sets stages property.</summary>
	public int stages
	{
		get { return _stages; }
		set { _stages = value; }
	}

	/// <summary>Gets and Sets currentStage property.</summary>
	public int currentStage
	{
		get { return _currentStage; }
		set { _currentStage = value; }
	}

	/// <summary>Gets and Sets healthDistribution property.</summary>
	public float[] healthDistribution
	{
		get { return _healthDistribution; }
		set { _healthDistribution = value; }
	}

	/// <summary>Resets Enemy's instance to its default values.</summary>
	public virtual void Reset()
	{
		base.Reset();
		currentStage = 0;
		AdvanceStage();
	}

	/// <summary>Callback internally called right after Awake.</summary>
	protected override void OnAwake()
	{
		currentStage = 0; 	/// For good measure
		AdvanceStage();
	}

	/// <summary>Callback invoked when the health of the character is depleted.</summary>
	protected override void OnHealthEvent(HealthEvent _event, float _amount = 0.0f)
	{
		switch(_event)
		{
			case HealthEvent.FullyDepleted:
			if(currentStage < stages) AdvanceStage();
			else
			{
				this.RemoveStates(ID_STATE_ALIVE);
				OnObjectDeactivation();
			}
			break;
		}
	}

	/// <summary>Advances Stage.</summary>
	protected void AdvanceStage()
	{
		currentStage = Mathf.Min(currentStage, stages);
		health.SetMaxHP(healthDistribution[currentStage++], true);
		OnStageChanged();
	}

	/// <summary>Callback internally called when the Boss advances stage.</summary>
	protected virtual void OnStageChanged() { /*...*/ }

	/// <returns>String representing enemy's stats.</returns>
	public override string ToString()
	{
		StringBuilder builder = new StringBuilder();

		builder.AppendLine(base.ToString());
		builder.Append("Current Stage: ");
		builder.Append(currentStage.ToString());

		return builder.ToString();
	}
}
}