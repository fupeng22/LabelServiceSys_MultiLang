using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class M_OperationLog_HU
    {
        public int huopID
       {
           get;
           set;
       }

        public int huop_urID
       {
           get;
           set;
       }

        public string huopJobNameIdLv1
       {
           get;
           set;
       }

        public string huopJobNameIdLv2
       {
           get;
           set;
       }

        public string huopContent
       {
           get;
           set;
       }

        public DateTime huopDateTime
       {
           get;
           set;
       }

        public int huopIsDvir
       {
           get;
           set;
       }
    }
}
