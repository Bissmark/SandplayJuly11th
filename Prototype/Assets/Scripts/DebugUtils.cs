/* 
 * Responsible for displaying asset messages
 */

#define DEBUG

using System;
using System.Diagnostics;

public class AssertException : Exception
{
	public AssertException()
	{
	}
	
	public AssertException( String a_Message )
		: base( a_Message )
	{
		
	}
}

public class DebugUtils
{
	[Conditional( "DEBUG" )]
	public static void Assert( bool a_Condition )
	{
		if ( !a_Condition )
		{
			throw new AssertException();
		}
	}
	
	[Conditional( "DEBUG" )]
	public static void Assert( bool a_Condition,
	                          String a_Message )
	{
		if ( !a_Condition )
		{
			throw new AssertException( a_Message );
		}
	}
}

