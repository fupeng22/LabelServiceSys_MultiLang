using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
   public class M_tb_Setting_WorkProcess_L1
    {
       public int int_id
       {
           get;
           set;
       }

       public string vchar_gateway
       {
           get;
           set;
       }
       public string vchar_WorkProcess_L1_Code
       {
           get;
           set;
       }
       public string vchar_WorkProcess_L1_Text
       {
           get;
           set;
       }
       public int bit_IsUse
       {
           get;
           set;
       }
       public string vchar_created_user
       {
           get;
           set;
       }

       public DateTime dttm_created_dttm
       {
           get;
           set;
       }
    }
}
