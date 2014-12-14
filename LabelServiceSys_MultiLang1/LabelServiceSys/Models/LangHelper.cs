using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.Reflection;
using System.Web.Mvc;

namespace LabelServiceSys.Models
{
    public static class LangHelper
    {
        //界面普通文字的多语言
        public static string GetLangbyKey(this HtmlHelper htmlhelper, string key)
        {
            Type resourceType = (Thread.CurrentThread.CurrentUICulture.Name == "en-US") ? typeof(Resources.en_US) : typeof(Resources.zh_CN);
            PropertyInfo p = resourceType.GetProperty(key);
            if (p != null)
                return p.GetValue(null, null).ToString();
            else
                return "undefined";
        }

        //js定义多语言弹出框
        public static string LangOutJsVar(this HtmlHelper htmlhelper, string key)
        {
            Type resourceType = (Thread.CurrentThread.CurrentUICulture.Name == "en-US") ? typeof(Resources.en_US) : typeof(Resources.zh_CN);
            PropertyInfo p = resourceType.GetProperty(key);
            if (p != null)
                return  string.Format("var {0} ='{1}'", key, p.GetValue(null, null).ToString());
            else
                return string.Format("var {0} ='{1}'", key, "undefined");
        }

        //界面普通文字的多语言
        public static string GetLangbyKey( string key)
        {
            Type resourceType = (Thread.CurrentThread.CurrentUICulture.Name == "en-US") ? typeof(Resources.en_US) : typeof(Resources.zh_CN);
            PropertyInfo p = resourceType.GetProperty(key);
            if (p != null)
                return p.GetValue(null, null).ToString();
            else
                return "undefined";
        }

        //js定义多语言弹出框
        public static string LangOutJsVar( string key)
        {
            Type resourceType = (Thread.CurrentThread.CurrentUICulture.Name == "en-US") ? typeof(Resources.en_US) : typeof(Resources.zh_CN);
            PropertyInfo p = resourceType.GetProperty(key);
            if (p != null)
                return string.Format("var {0} ='{1}'", key, p.GetValue(null, null).ToString());
            else
                return string.Format("var {0} ='{1}'", key, "undefined");
        }

        public static string WhichLang()
        {
            return Thread.CurrentThread.CurrentUICulture.Name == "en-US" ? string.Format("var pub_WhichLang='{0}'", "en-US") : string.Format("var pub_WhichLang='{0}'", "zh-CN");
        }

        public static string GetWhichLang()
        {
            return Thread.CurrentThread.CurrentUICulture.Name == "en-US" ? string.Format("{0}", "en-US") : string.Format("{0}", "zh-CN");
        }
    }
}