using System;
using System.Data;

namespace com.barbuchu.bisma{
    /// <summary>
    /// Defines a command line argument.
    /// </summary>
    public class ArgSpec{
        // TODO: add delegate to argument handler :)
        // DONE: OMG i did it, don't I have better things to do? Like actually implementing the backup and recovery tool?
                    
        /// <summary>
        /// The argument's name
        /// </summary>
        public string Name;

        /// <summary>
        /// The argument's value
        /// </summary>
        public string ValName;

        /// <summary>
        /// Documentation string
        /// </summary>
        public string Doc;

        /// <summary>
        /// Determines if the value is optional
        /// </summary>
        public bool ValOptionnal;

        /// <summary>
        /// Creates an ArgumentSpec
        /// </summary>
        /// <param name="name">Arg name</param>
        /// <param name="valName">Value name</param>
        /// <param name="valOptional">is the value optionnal?</param>
        /// <param name="doc">Documentation</param>
        /// <param name="onApplyArg">delegate in charge of applying the arg on the target</param>
        public ArgSpec(string name, string valName,bool valOptional,string doc,ApplyArgDelegate onApplyArg){
            this.Name=name;
            this.ValName=valName;
            this.Doc=doc;
            this.ValOptionnal=valOptional;
            this.OnApplyArg=new ApplyArgDelegate(onApplyArg);
        }
                      
        /// <summary>
        /// Defines a delegate archetype
        /// </summary>
        public delegate void ApplyArgDelegate(Object target,ArgSpec arg,String[] argValues);
            
        /// <summary>
        /// Event delegate called when the argument gets applied on the function.
        /// </summary>
        public ApplyArgDelegate OnApplyArg;
            

        /// <summary>
        /// applies the arguments on the specified target
        /// </summary>        
        /// <param name="target"></param>
        /// <param name="argumentSpecs"></param>
        /// <param name="args"></param>
        public static void ApplyArgs(String exe,String doc,Object target, String[] args, params ArgSpec[] argumentSpecs){
            try{
                ArgSpec currentArg=null;
                int firstValIndex=-1;
                // parse the args
                for(int i=0;i<args.Length;i++){               
                    // looking for an arg                    
                    if(args[i].StartsWith("-")){    
                        // check if previous arg exists
                        if(currentArg!=null){
                            String[] vals=new String[i-firstValIndex];
                            System.Array.Copy(args,firstValIndex,vals,0,vals.Length);
                            currentArg.OnApplyArg(target,currentArg,vals);                            
                        }

                        // get the next arg spec
                        currentArg=getArgSpec(args[i].Substring(1),argumentSpecs);
                        if(currentArg ==null)
                            throw new ArgumentException(String.Format("argument {0} not found",args[i]));                        
                    }else{
                        if(currentArg==null){
                            throw new ArgumentException(string.Format("{0} : missing -",args[i]));
                        }else{
                            // Stack up values
                            firstValIndex=i;
                        }
                    }
                }
            }catch(ArgumentException ex){
                Console.Out.WriteLine(ex.Message);
                showUsage(exe,doc,argumentSpecs);
                throw new ArgumentException(ex.Message,ex.ParamName,ex);
            }
        }
        /// <summary>
        /// Prints the usage
        /// </summary>
        private static void showUsage(string exe, string doc, ArgSpec[] Arguments){
            Console.Out.WriteLine(string.Format("{0} <options> : {1}",exe,doc));
            Console.Out.WriteLine("Options:");
            foreach(ArgSpec argSpec in Arguments){
                Console.Out.Write(" -{0} {1}{2}{3} : {4}",
                    argSpec.Name,
                    argSpec.ValOptionnal?"[":"<",
                    argSpec.ValName,
                    argSpec.ValOptionnal?"]":">",
                    argSpec.Doc                    
                    ); 
            }
        }

        /// <summary>
        /// Get the spec corresponding to the arg name
        /// </summary>
        /// <param name="argName">The argument name</param>
        /// <param name="Arguments">The argument list</param>
        /// <returns></returns>
        public static ArgSpec getArgSpec(String argName,ArgSpec[] Arguments){
            foreach(ArgSpec spec in Arguments){
                if(spec.Name.Equals(argName))
                    return spec;
            }
            return null;
        }    

    }//~argSpec
}//~ns