using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace com.barbuchu.bisma{


    public class BBackup{
    
        
        public BBackup(){
            
        }

        public void LoadConfig(String xmlConfigFilePath){
            
        }
        public void SaveConfig(String xmlConfigPath){
        
        }

        [STAThread]
        static int Main(String[] args) {

           
            BBackup bb=new BBackup();
            try{                
                ArgSpec.ApplyArgs(
                    "BBackup",
                    "Backups using the BISMA framework",
                    bb,
                    args,                    
                    new ArgSpec("exec","filename",false, "Execute the specified configuration file",new ArgSpec.ApplyArgDelegate(ExecArgDelegate))                    
                    );
            }catch(ArgumentException){
                return -2;
            }catch(Exception ex){
                Console.Out.WriteLine(ex);
                return -1;
            }
                        
            return 0;
        }

        

        /// <summary>
        /// Called when the exec arg is encountered 
        /// </summary>
        /// <param name="bb">the bb instance</param>
        /// <param name="arg">the argspec</param>
        /// <param name="argValues">the argument values</param>
        public static void ExecArgDelegate(Object bb,ArgSpec arg,String[] argValues){
            // HMM. WHERE AM I?
            Console.Out.WriteLine(arg.Name);

        }

        

    }
}
