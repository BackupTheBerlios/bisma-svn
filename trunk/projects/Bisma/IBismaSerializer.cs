/*
 * Created by SharpDevelop.
 * User: Florian
 * Date: 24/09/2004
 * Time: 17:26
 * 
 */

using System;
using System.IO;

namespace palad1.bisma
{
	/// <summary>
	/// Defines a common interface for serializers
	/// </summary>
	public interface IBismaSerializer{
		Stream getArchiveOutputStream();				
		Stream getArchiveInputStream();	
		
		void commitInput();
		void commitOutput();
	}	
}


