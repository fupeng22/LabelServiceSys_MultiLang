using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
   public class M_OperationLog_Hawb
    {
       public int hawbopID
       {
           get;
           set;
       }

       public int hawbop_urID
       {
           get;
           set;
       }

       public string hawbopJobNameIdLv1
       {
           get;
           set;
       }

       public string hawbopJobNameIdLv2
       {
           get;
           set;
       }

       public string hawbopContent
       {
           get;
           set;
       }

       public DateTime hawbopDateTime
       {
           get;
           set;
       }

       public int hawbopIsDvir
       {
           get;
           set;
       }
    }
}
