using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Util;
using SQLDAL;
using System.IO;
using System.Threading;
using LabelServiceSys.Models;
using LabelServiceSys.Filter;

namespace LabelServiceSys.Controllers
{
     [ErrorAttribute]
    public class MonitorController : Controller
    {
        //
        // GET: /Scan/

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public string Scan(string devicetype, string isdvir, string usernumber, string data, string msn)
        {
            //string strRet = "{\"Result\":\"error\",\"message\":\"处理失败,原因未知\"}";
            string strRet = "{\"Result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("Monitor_Controller_ErrorMessage1") + "\"}";
            string str_devicetype = "";
            string str_isdvir = "";
            string str_usernumber = "";
            string str_data = "";
            string str_msn = "";

            //using (StreamWriter sw = new StreamWriter(Server.MapPath("~/txt/" + "log.txt"), true))
            //{

            //    sw.Write(data + "---" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n");
            //}

            try
            {
                if (string.IsNullOrEmpty(devicetype) || string.IsNullOrEmpty(isdvir) || string.IsNullOrEmpty(usernumber) || string.IsNullOrEmpty(data) || string.IsNullOrEmpty(msn))
                {

                }
                else
                {
                    str_devicetype = Server.UrlDecode(devicetype);
                    str_isdvir = Server.UrlDecode(isdvir);
                    str_usernumber = Server.UrlDecode(usernumber);
                    str_data = Server.UrlDecode(data);
                    str_msn = Server.UrlDecode(msn);
                    BarcodeUtil barcodeUtil = new BarcodeUtil();
                    switch (barcodeUtil.ParseBarcode(str_data))
                    {
                        case Enum_Barcode.UnKnown:
                            (new Thread(new ThreadStart(delegate()
                            {
                                new T_OperationLog_Other().insertOperationLog_OtherFromUsernum(str_usernumber, str_data, str_isdvir);

                            }))).Start();
                            //strRet = "{\"Result\":\"ok\",\"message\":\"未知条码处理成功\"}";
                            strRet = "{\"Result\":\"ok\",\"message\":\"" + LangHelper.GetLangbyKey("Monitor_Controller_SuccessMessage1") + "\"}";
                            //if (new T_OperationLog_Other().insertOperationLog_OtherFromUsernum(str_usernumber, str_data, str_isdvir))
                            //{
                            //    strRet = "{\"Result\":\"ok\",\"message\":\"未知条码处理成功\"}";
                            //}
                            break;
                        case Enum_Barcode.Hawb:
                            (new Thread(new ThreadStart(delegate()
                            {
                                new T_OperationLog_Hawb().insertOperationLog_HawbFromUsernum(str_usernumber, str_data, str_isdvir);

                            }))).Start();
                            //if (new T_OperationLog_Hawb().insertOperationLog_HawbFromUsernum(str_usernumber, str_data, str_isdvir))
                            //{
                            //    strRet = "{\"Result\":\"ok\",\"message\":\"分单号处理成功\"}";
                            //}
                            //strRet = "{\"Result\":\"ok\",\"message\":\"分单号处理成功\"}";
                            strRet = "{\"Result\":\"ok\",\"message\":\"" + LangHelper.GetLangbyKey("Monitor_Controller_SuccessMessage2") + "\"}";
                            break;
                        case Enum_Barcode.Pcid:
                            (new Thread(new ThreadStart(delegate()
                            {
                                new T_OperationLog_Pcid().insertOperationLog_PcidFromUsernum(str_usernumber, str_data, str_isdvir);

                            }))).Start();
                            //if (new T_OperationLog_Pcid().insertOperationLog_PcidFromUsernum(str_usernumber, str_data, str_isdvir))
                            //{
                            //    strRet = "{\"Result\":\"ok\",\"message\":\"件号处理成功\"}";
                            //}
                           // strRet = "{\"Result\":\"ok\",\"message\":\"件号处理成功\"}";
                            strRet = "{\"Result\":\"ok\",\"message\":\"" + LangHelper.GetLangbyKey("Monitor_Controller_SuccessMessage3") + "\"}";
                            break;
                        case Enum_Barcode.HU:
                            (new Thread(new ThreadStart(delegate()
                            {
                                new T_OperationLog_HU().insertOperationLog_HUFromUsernum(str_usernumber, str_data, str_isdvir);

                            }))).Start();
                            //if (new T_OperationLog_HU().insertOperationLog_HUFromUsernum(str_usernumber, str_data, str_isdvir))
                            //{
                            //    strRet = "{\"Result\":\"ok\",\"message\":\"袋号处理成功\"}";
                            //}
                            //strRet = "{\"Result\":\"ok\",\"message\":\"袋号处理成功\"}";
                            strRet = "{\"Result\":\"ok\",\"message\":\"" + LangHelper.GetLangbyKey("Monitor_Controller_SuccessMessage4") + "\"}";
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                //strRet = "{\"Result\":\"error\",\"message\":\"处理失败,原因:" + ex.Message + "\"}";
                strRet = "{\"Result\":\"error\",\"message\":\"" + LangHelper.GetLangbyKey("Monitor_Controller_ErrorMessage1") + ":" + ex.Message + "\"}";
            }

            return strRet;
        }

    }
}
