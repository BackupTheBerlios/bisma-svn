using System;
//using System.Runtime.InteropServices;

namespace KCommon.Net.FTP
{
	public interface IFtpItem
	{
		string		Name		{get;set;}
		string		FullName	{get;}
		bool		IsFile		{get;}
		bool		IsDirectory {get;}
	}
}