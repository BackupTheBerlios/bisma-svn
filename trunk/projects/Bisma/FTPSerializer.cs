/*
 * Created by SharpDevelop.
 * User: Florian
 * Date: 24/09/2004
 * Time: 18:10
 *
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

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

using KCommon.Net.FTP;

namespace palad1.bisma
{
	/// <summary>
	/// Description of SFTPSerializer.
	/// </summary>
	public class FTPSerializer : IBismaSerializer
	{
		string host;
		short port;
		string path;
		string archiveName;
		string userName;
		string password;
		
		Session ftpSession;
		
		public FTPSerializer(string host,short port,string path, string archiveName,string userName,string password){
			this.host=host;
			this.port=port;
			this.path=path;
			this.archiveName=archiveName;
			this.userName=userName;
			this.password=password;
			
			// create the session
			this.ftpSession=new Session();
			this.ftpSession.Server=this.host;
			this.ftpSession.Port=port;
			
			// logon
			this.ftpSession.Connect(this.userName,this.password);						
					
			// cwd
			string[] arPath=this.path==null?new string[0]:path.Split(new char[]{'/'});
			foreach(string subdir in arPath){
				ftpSession.CurrentDirectory=ftpSession.CurrentDirectory.FindSubdirectory(subdir,true);
				
			}
		}
		
		public Stream getArchiveOutputStream(){
			return ftpSession.CurrentDirectory.CreateFileStream(this.archiveName);
		}
		
		private void commit(){
			this.ftpSession.Close();
		}
		
		public void commitInput(){
			commit();			
		}
		
		public void commitOutput(){
			commit();
		}
		
		public Stream getArchiveInputStream(){
			FtpFile file= ftpSession.CurrentDirectory.FindFile(this.archiveName);
			return file.GetInputStream();
		}
	}
}
