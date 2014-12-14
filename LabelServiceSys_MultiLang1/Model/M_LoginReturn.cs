using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
   public  class M_LoginReturn
    {
       public string UserNumber
       {
           get;
           set;
       }

       public int IsDVIR
       {
           get;
           set;
       }

       public string IsDVIRDesc
       {
           get;
           set;
       }

       public bool IsDVIRDesc_b
       {
           get;
           set;
       }

       public string WorkProcess_L1_Code
       {
           get;
           set;
       }

       public string WorkProcess_L2_Code
       {
           get;
           set;
       }

       public string WorkProcess_L1_Text
       {
           get;
           set;
       }

       public string WorkProcess_L2_Text
       {
           get;
           set;
       }

       public Boolean IsLogin
       {
           get;
           set;
       }

       public int IsLogin_i
       {
           get;
           set;
       }

       public string WorkstationId
       {
           get;
           set;
       }

       public int getWorkstationId
       {
           get;
           set;
       }
    }
}
