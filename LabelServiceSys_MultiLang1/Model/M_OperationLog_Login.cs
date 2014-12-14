using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class M_OperationLog_Login
    {
        public int lgopID
       {
           get;
           set;
       }

        public int lgop_urID
       {
           get;
           set;
       }

        public string lgopJobNameIdLv1
       {
           get;
           set;
       }

        public string lgopJobNameIdLv2
       {
           get;
           set;
       }

        public string lgopContent
       {
           get;
           set;
       }

        public DateTime lgopDateTime
       {
           get;
           set;
       }

        public int lgopIsDvir
       {
           get;
           set;
       }
    }
}
