using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebInterface.QueryUtilities
{
    public static partial class MITSUBISHIQueryUtilities
    {
        public static string GenMITSUBISHIQueryStr(List<string> IOList)
        {
            // Generate query string for MITSUBISHI devices
            // Format depends on the specific MITSUBISHI protocol being used
            string queryStr;
            List<string> ls;
            string[] la;

            ls = new List<string>(IOList);
            // MITSUBISHI devices often use newline-separated device lists
            queryStr = string.Join("\n", ls);

            return queryStr;
        }

        public static List<int> ConvertMITSUBISHITransStr(string transStr, out bool result)
        {
            // Convert response string from MITSUBISHI device to integer list
            char splitChar = '\n';
            int[] tr = new int[0];
            transStr = transStr.Trim('\n');
            result = true;
            
            try
            {
                tr = transStr.Split(splitChar).Select(s => MITSUBISHITransParse(s)).ToArray();
            }
            catch
            {
                result = false;
            }

            return tr.ToList();
        }

        static int MITSUBISHITransParse(string s)
        {
            // Parse individual MITSUBISHI device response
            try
            {
                // Handle various MITSUBISHI data formats
                if (s.Contains(","))
                {
                    return Convert.ToInt32(s.Replace(",", "").Trim(' '));
                }
                else
                {
                    return int.Parse(s.Trim());
                }
            }
            catch
            {
                return 0; // Default value for parsing errors
            }
        }

        public static string FormatDeviceName(string deviceName)
        {
            // Format device names according to MITSUBISHI conventions
            // Examples: D100, X0, Y0, M100, etc.
            return deviceName.ToUpper().Trim();
        }

        public static bool ValidateDeviceName(string deviceName)
        {
            // Validate MITSUBISHI device name format
            // Common formats: D####, X###, Y###, M####, etc.
            string pattern = @"^[DXYMCTLFVZRWS]\d+$";
            return System.Text.RegularExpressions.Regex.IsMatch(deviceName.ToUpper(), pattern);
        }
    }
}