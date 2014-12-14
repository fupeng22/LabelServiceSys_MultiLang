using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class M_OperationLog_Pcid
    {
        public int pcidopID
       {
           get;
           set;
       }

        public int pcidop_urID
       {
           get;
           set;
       }

        public string pcidopJobNameIdLv1
       {
           get;
           set;
       }

        public string pcidopJobNameIdLv2
       {
           get;
           set;
       }

        public string pcidopContent
       {
           get;
           set;
       }

        public DateTime pcidopDateTime
       {
           get;
           set;
       }

        public int pcidopIsDvir
       {
           get;
           set;
       }
    }
}
