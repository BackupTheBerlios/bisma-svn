/*
 * Created by SharpDevelop.
 * User: Florian
 * Date: 24/09/2004
 * Time: 18:03
 *
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.IO;
using System.Diagnostics;

using KCommon.Net.FTP;

namespace palad1.bisma
{
	/// <summary>
	/// Description of SFTPSerializer.
	/// </summary>
	public class SFTPSerializer : IBismaSerializer
	{

		
		string puttyPath;
		string host;
		short port;
		string path;
		string archiveName;
		
		string userName;
		string password;
		
		
		private FileStream fsOut;
		private string tmpName;
		
		public SFTPSerializer(string puttyPath,string host,short port,string path, string archiveName,string userName,string password){
			this.puttyPath=puttyPath;
			this.host=host;
			this.port=port;
			
			if(path!=null && !path.EndsWith("/"))
				this.path=path+"/";
			else
				this.path=path;
			
			this.archiveName=archiveName;
			this.userName=userName;
			this.password=password;
		}
		
		public Stream getArchiveOutputStream(){
			this.tmpName=Path.GetTempFileName();
			this.fsOut=new FileStream(this.tmpName,FileMode.Create,FileAccess.Write);
			return fsOut;
		}
		
		public void commitOutput(){
			Process procCmd = new Process();
			procCmd.StartInfo.FileName =  this.puttyPath;
			procCmd.StartInfo.Arguments =
				string.Format(
				              "-q -P {0} -pw {1} -C {2} {3}@{4}:{5}{6}",
				              this.port,//0
				              this.password,//1
				              this.tmpName,//2
				              this.userName,//3
				              this.host,//4
				              this.path,//5
				              this.archiveName//6
				             );
			
			procCmd.StartInfo.CreateNoWindow = true;
			procCmd.StartInfo.UseShellExecute = false;
			procCmd.StartInfo.RedirectStandardOutput = false;
			procCmd.StartInfo.RedirectStandardError = false;
			procCmd.Start();
			procCmd.WaitForExit();
			
			
		}
		
		public void commitInput(){
			throw new Exception("not implemented");
		}
		
		public Stream getArchiveInputStream(){
			throw new Exception("not implemented");
		}
	}
}
