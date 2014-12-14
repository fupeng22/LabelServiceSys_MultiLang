using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Util
{
   public class PubVariables
    {
       /// <summary>
       /// 系统超级管理员集合
       /// </summary>
       public static string[] SysUserNames = ConfigurationManager.AppSettings["SysUsernames"].Replace('，',',').ToLower().Split(',');
    }
}
