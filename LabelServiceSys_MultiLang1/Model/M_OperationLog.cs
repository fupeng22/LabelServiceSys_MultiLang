using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class M_OperationLog
    {
        public int opID
        {
            get;
            set;
        }

        public int op_urID
        {
            get;
            set;
        }

        public string op_urStaffNum
        {
            get;
            set;
        }

        public string op_urUnitCode
        {
            get;
            set;
        }

        public string opJobNameIdLv1
        {
            get;
            set;
        }

        public string opJobNameIdLv2
        {
            get;
            set;
        }

        public int opType
        {
            get;
            set;
        }

        public string opContent
        {
            get;
            set;
        }

        public DateTime opDateTime
        {
            get;
            set;
        }
    }
}
