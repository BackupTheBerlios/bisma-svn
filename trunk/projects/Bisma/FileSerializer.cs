/*
 * Created by SharpDevelop.
 * User: Florian
 * Date: 24/09/2004
 * Time: 17:34
 *
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.IO;

namespace palad1.bisma
{
	/// <summary>
	/// Description of FileSerializer.
	/// </summary>
	public class FileSerializer : IBismaSerializer
	{
		private string filePath;
		public FileSerializer(string filePath){
		 	this.filePath=filePath;
		}
		
		public Stream getArchiveOutputStream(){
			return new FileStream(filePath,FileMode.Create,FileAccess.Write);
		}
		public void commitInput(){}
		public void commitOutput(){}
		
		public Stream getArchiveInputStream(){
		 	return new FileStream(filePath,FileMode.Create,FileAccess.Write);
		}
	}
}
