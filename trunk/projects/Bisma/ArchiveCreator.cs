/*
 * Created by SharpDevelop.
 * User: Florian
 * Date: 24/09/2004
 * Time: 11:49
 *
 */
using System;
using System.Collections;
using System.Text;
using System.Configuration;
using System.IO;
using System.Windows.Forms;
using System.Xml;

using ICSharpCode.SharpZipLib.Zip;

namespace palad1.bisma{
	
    /// <summary>
    /// BISMA: Backups on Internet Saved My ...
    /// </summary>
    public class ArchiveCreator{
        public void createArchive(FileInfo[] arFiles,palad1.bisma.IBismaSerializer serializer){
            DateTime start=DateTime.Now;
            long uncompressedLength=0;
			
			
            // log
            StringBuilder sb=new StringBuilder();
            sb.AppendFormat("<?xml version='1.0'?>\n<bisma v='1.0' date='{0}'>\n",start);
			
            // create the zip
            ZipOutputStream zos=new ZipOutputStream(serializer.getArchiveOutputStream());
            zos.SetLevel(9);
			
            // add each specified file
            foreach(FileInfo file in arFiles){
                if(!file.Exists){
                    sb.AppendFormat(" <err path=\"{0}\" comment='does not exist'/>\n",file.FullName);
                }else{
                    try{
                        this.addFileInArchive(zos,sb,file);
                        uncompressedLength+=file.Length;
                    }catch(Exception ex){
                        sb.AppendFormat(" <err path=\"{0}\" comment='Exception:{1}'/>\n",file.FullName,ex.ToString().Replace("'","&quote;"));
                    }
                }
            }
            // close the log
            sb.AppendFormat(" <perf elapsed='{0}ms' archived='{1}'/>\n</bisma>",((TimeSpan)(DateTime.Now-start)).Milliseconds,uncompressedLength/1024);
            zos.SetComment(sb.ToString());
            zos.Close();
			
            // commit
            serializer.commitOutput();
        }
        /// <summary>
        /// adds a file in an archive
        /// </summary>
        /// <param name="zop">the Zip OutputStream</param>
        /// <param name="sb">the stringbuilder containing the xml representation of the archive</param>
        /// <param name="file">the file to archive</param>
        private void addFileInArchive(ZipOutputStream zos, StringBuilder sb , FileInfo file){
            byte[] buffer=new Byte[4096];
            int offset=0,read=0;
			
            Console.Out.WriteLine(file.FullName);
            // create the next entry
            String entryName=file.FullName;
            switch(entryName[1]){
                case ':': // drive letter
                    sb.AppendFormat(" <file type='local' path=\"{0}\"/>\n",entryName);
                    entryName="file\\"+entryName;
                    break;
                case '\\': // UNC
                    sb.AppendFormat(" <file type='unc' path=\"{0}\"/>\n",entryName);
                    entryName="unc\\"+entryName;
                    break;
            }
            // create the entry
            zos.PutNextEntry(new ZipEntry(entryName));
            BufferedStream bs=new BufferedStream(new FileStream(file.FullName,FileMode.Open,FileAccess.Read));
			
            // zip the file
            while((read=bs.Read(buffer,0,buffer.Length))>0){
                zos.Write(buffer,0,read); //
                offset+=read;
            }
            bs.Close();
        }
		
		
        public static void getFiles(DirectoryInfo dir,ArrayList files){
            files.AddRange(dir.GetFiles());
            foreach(DirectoryInfo child in dir.GetDirectories())
                getFiles(child,files);
        }
        public static void Main(string[] args){
            string localDirPath=arg[0];
            
            

            ArchiveCreator cr=new ArchiveCreator();     

            ArrayList arFiles=new ArrayList();
            getFiles(new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.Personal)),arFiles);
            FileInfo[] files=(FileInfo[])arFiles.ToArray(typeof(FileInfo));
            try{
                FileSerializer fs=new FileSerializer(@"c:\bisma.zip");
                cr.createArchive(files,fs);
                MessageBox.Show("File OK");
				

                ZipInputStream zis=new ZipInputStream(new FileStream("c:/bisma.zip",FileMode.Open,FileAccess.Read));
                XmlDocument doc=new XmlDocument();
                ZipEntry entry=null;

                ZipFile zf=new ZipFile("c:/bisma.zip");
                doc.LoadXml(zf.ZipFileComment);
                
                Console.Out.WriteLine(doc.InnerXml);
                
                FTPSerializer ftps=new FTPSerializer("localhost",21,null,"test.zip","user","pass");
                cr.createArchive(files,ftps);
                MessageBox.Show("FTP OK");
                				
                SFTPSerializer sftps=
                    new SFTPSerializer
                    (@"C:\Program Files\putty\pscp.exe",
                    "localhost",
                    22,
                    null,
                    "test.zip",
                    "palad1",
                    "mypass");
                cr.createArchive(files,sftps);
                MessageBox.Show("SFTP OK");
            }catch(Exception e){
                MessageBox.Show(e.ToString());
            }
        }
    }
}

