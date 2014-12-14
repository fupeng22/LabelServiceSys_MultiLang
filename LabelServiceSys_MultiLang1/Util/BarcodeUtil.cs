using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Util
{
    public class BarcodeUtil
    {
        public Enum_Barcode ParseBarcode(string Barcode)
        {
            string strTemp1_9 = "";
            string strTemp10 = "";
            string strTemp1 = "";
            string strTemp2_10 = "";
            string strTemp2_9 = "";
            Regex reg = null;
            Match match = null;
            if (Barcode.Length == 10 || (Barcode.Length >= 21 && (Barcode.Substring(1, 2) == "JD" || Barcode.Substring(2, 2) == "JD" || Barcode.Substring(3, 2) == "JD")))
            {
                if (Barcode.Length == 10)
                {
                    reg = new Regex("^[0-9]+$");
                    match = reg.Match(Barcode);
                    if (match.Success)
                    {
                        strTemp1_9 = Barcode.Substring(0, 9);
                        strTemp10 = Barcode.Substring(9, 1);
                        if ((Convert.ToInt32(strTemp1_9) % 7) == Convert.ToInt32(strTemp10))
                        {
                            return Enum_Barcode.Hawb;
                        }
                        else
                        {
                            return Enum_Barcode.UnKnown;
                        }
                    }
                    else
                    {
                        strTemp1 = Barcode.Substring(0, 1);
                        strTemp2_10 = Barcode.Substring(1, 9);
                        strTemp2_9 = Barcode.Substring(1, 8);
                        strTemp10 = Barcode.Substring(9, 1);
                        if (strTemp1 == "H")
                        {
                            reg = new Regex("^[0-9]+$");
                            match = reg.Match(strTemp2_10);
                            if (match.Success)
                            {
                                if ((Convert.ToInt32(strTemp2_9) % 7) == Convert.ToInt32(strTemp10))
                                {
                                    return Enum_Barcode.HU;
                                }
                                else
                                {
                                    return Enum_Barcode.UnKnown;
                                }
                            }
                            else
                            {
                                return Enum_Barcode.UnKnown;
                            }
                        }
                        else
                        {
                            return Enum_Barcode.UnKnown;
                        }
                    }
                }
                else
                {
                    return Enum_Barcode.Pcid;
                }
            }
            else
            {
                return Enum_Barcode.UnKnown;
            }
        }
    }
}
