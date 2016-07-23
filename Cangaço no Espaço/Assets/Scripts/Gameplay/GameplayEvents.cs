using System;
using System.Collections.Generic;

public class GameplayEvents
{
	//http://nubick-en.blogspot.com.br/2015/03/methods-of-organizing-interaction.html

	public static NimHealthEvent nimDied = new NimHealthEvent();
	public static NimHealthEvent nimOnStage = new NimHealthEvent();
	public static EnemyHealthEvent enemyDied = new EnemyHealthEvent();
}

/** LIST OF EVENTS **/

public class NimHealthEvent
{
	private readonly List<Action<NimHealth>> _callbacks = new List<Action<NimHealth>>();

	public void Subscribe(Action<NimHealth> callback){
		_callbacks.Add(callback);
	}

	public void Unsubscribe(){
		_callbacks.RemoveAll (AllOfThem);
	}

	private static bool AllOfThem(Action<NimHealth> s)
	{
		return true;
	}

	public void Publish(NimHealth unit){
		foreach (Action<NimHealth> callback in _callbacks)
			callback (unit);
	}
}


public class EnemyHealthEvent
{
	private readonly List<Action<EnemyHealth>> _callbacks = new List<Action<EnemyHealth>>();

	public void Subscribe(Action<EnemyHealth> callback){
		_callbacks.Add(callback);
	}

	public void Unsubscribe(){
		_callbacks.RemoveAll (AllOfThem);
	}

	private static bool AllOfThem(Action<EnemyHealth> s)
	{
		return true;
	}

	public void Publish(EnemyHealth unit){
		foreach (Action<EnemyHealth> callback in _callbacks)
			callback (unit);
	}
}