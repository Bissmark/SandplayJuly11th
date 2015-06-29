using UnityEngine;
using System.Collections;

public class Manager<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T _instance = null;
	public static T Instance
	{
		get { return _instance; }
	}
	
	public static bool Exists
	{
		get { return ( _instance != null ); }
	}
	
	// NOTE! Derived classes need to call base.Awake and base.OnDestroy in overrides
	protected virtual void Awake()
	{
		DebugUtils.Assert( _instance == null );
		_instance = this as T;
	}
	
	// NOTE! Derived classes need to call base.Awake and base.OnDestroy in overrides
	protected virtual void OnDestroy()
	{
		DebugUtils.Assert( _instance == this );
		_instance = null;
	}
}