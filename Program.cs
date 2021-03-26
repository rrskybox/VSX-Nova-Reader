/*
* AAVSO Reader is a VSX client for assembling nova data
* 
* Author:           Rick McAlister
* Date:             03/21/2021
* Current Version:  0.1
* Developed in:     Visual Studio 2019
* Coded in:         C# 8.0
* App Envioronment: Windows 10 Pro x32 and x64 (DB12978)
* 
* Change Log:
* 
* 
* 
*/

using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using System.Xml.Linq;

namespace AAVSO_VSX_Reader
{
    class Program
    {

        const string VSXGETURL = "http://www.aavso.org/vsx/index.php?view=query.votable&";

        const string NOVA_VTYPE1 = "N";
        const string NOVA_VTYPE2 = "NA";
        const string NOVA_VTYPE3 = "NB";
        const string NOVA_VTYPE4 = "NC";
        const string NOVA_VTYPE5 = "NR";
        //const string QSO_VTYPE = "QSO";

        [STAThread]
        static void Main(string[] args)
        {
            string version;
            try
            { version = ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString(); }
            catch
            {
                //probably in debug mode
                version = "in Debug";
            }
            Console.WriteLine("AAVSO_VSX_Reader V " + version);
            TNStoClipboard();
        }

        static void TNStoClipboard()
        {
            // url of vsx and vsx-sandbox api
            // 
            string startYear = (DateTime.Now.Year - 1).ToString("0");
            string endYear = (DateTime.Now.Year).ToString("0"); ;

            string url_vsx_search = VSXGETURL;
            const string tsxHead = "SN      Host Galaxy      Date         R.A.    Decl.    Offset   Mag.   Disc. Ref.            SN Position         Posn. Ref.       Type  SN      Discoverer(s)";

            string contents1 = "";
            WebClient client = new WebClient();
            System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };

            string vsxURLquery1 = url_vsx_search + MakeVSXSearchQuery(NOVA_VTYPE1, startYear, endYear);

            try
            {
                contents1 = client.DownloadString(vsxURLquery1);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Download Error: " + ex.Message);
                return;
            };

            XElement xmlDoc1 = XElement.Parse(contents1);
            //Preset text output with header
            string cbText = tsxHead + "\n\n";
            IEnumerable<XElement> dList1 = xmlDoc1.Descendants("TR");
            cbText += StringafyRecords(dList1);
            int n = dList1.Count();

            System.Windows.Forms.Clipboard.SetText(cbText, TextDataFormat.UnicodeText);
        }

        public static string MakeSearchQuery()
        {
            //Returns a url string for querying the vsx website

            NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);

            queryString["format"] = "tsv";

            queryString["name"] = "";
            queryString["name_like"] = "0";
            queryString["isTNS_AT"] = "all";
            queryString["public"] = "all";
            queryString["unclassified_at"] = "0";
            queryString["classified_sne"] = "1";
            queryString["ra"] = "";
            queryString["decl"] = "";
            queryString["radius"] = "";
            queryString["coords_unit"] = "arcsec";
            queryString["groupid[]"] = "null";
            queryString["classifier_groupid[]"] = "null";
            queryString["objtype[]"] = "null";
            queryString["AT_objtype[]"] = "null";
            queryString["date_start[date]"] = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd");
            queryString["date_end[date]"] = DateTime.Now.ToString("yyyy-MM-dd");
            queryString["discovery_mag_min"] = "";
            queryString["discovery_mag_max"] = "";
            queryString["internal_name"] = "";
            queryString["redshift_min"] = "";
            queryString["redshift_max"] = "";
            queryString["spectra_count"] = "";
            queryString["discoverer"] = "";
            queryString["classifier"] = "";
            queryString["discovery_instrument[]"] = "";
            queryString["classification_instrument[]"] = "";
            queryString["hostname"] = "";
            queryString["associated_groups[]"] = "null";
            queryString["ext_catid"] = "";
            queryString["num_page"] = "50";

            queryString["display[redshift]"] = "1";
            queryString["display[hostname]"] = "1";
            queryString["display[host_redshift]"] = "1";
            queryString["display[source_group_name]"] = "1";
            queryString["display[classifying_source_group_name]"] = "1";
            queryString["display[discovering_instrument_name]"] = "0";
            queryString["display[classifing_instrument_name]"] = "0";
            queryString["display[programs_name]"] = "0";
            queryString["display[internal_name]"] = "1";
            queryString["display[isTNS_AT]"] = "0";
            queryString["display[public]"] = "1";
            queryString["displa[end_pop_period]"] = "0";
            queryString["display[pectra_count]"] = "1";
            queryString["display[discoverymag]"] = "1";
            queryString["display[Bdiscmagfilter]"] = "1";
            queryString["display[discoverydate]"] = "1";
            queryString["display[discoverer ]"] = "1";
            queryString["display[sources]"] = "0";
            queryString["display[bibcode]"] = "0";
            queryString["display[ext_catalogs]"] = "0";

            return queryString.ToString(); // Returns "key1=value1&key2=value2", all URL-encoded
        }

        public static string MakeVSXSearchQuery(string vType, string startYear, string endYear)
        {
            //Returns a url string for querying the vsx website

            NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString(string.Empty);
            //NameValueCollection queryString = System.Web.HttpUtility.ParseQueryString("");

            //queryString[ "coords"]= "";
            //queryString[  "ident"] = "";
            //queryString[ "constid"] = "";
            //queryString["format"] = "s";
            //queryString[  "geom"] = "";
            //queryString[  "size"] = "";
            //queryString[  "unit"] = "";
            //queryString["vtype"] = vType;
            //queryString[  "stype"] = "";
            //queryString[  "maxhi"] = "";
            //queryString[ "maxlo"] = "";
            //queryString[  "perhi"] = "";
            //queryString[  "perlo"] = "";
            //queryString[  "ephi"] = "";
            //queryString[ "eplo"] = "";
            //queryString[  "riselo"] = "";
            //queryString[  "risehi"] = "";
            queryString["yrlo"] = startYear;
            queryString["yrhi"] = endYear;
            //queryString[  "filter"] = "";
            //queryString[ "order"] = "";

            return queryString.ToString(); // Returns "key1=value1&key2=value2", all URL-encoded
        }
        public static string FitFormat(string entry, int slotSize)
        {
            //Returns a string which is the entry truncated to the slot Size, if necessary
            if (entry == null) return "                    ".Substring(0, slotSize);
            if (entry.Length > slotSize)
                return entry.Substring(0, slotSize - 1).PadRight(slotSize);
            else
                return entry.PadRight(slotSize);
        }

        public static string ParseToSexidecimal(string sex, bool doRA)
        {
            //converts a string in decimal format to a string in sexidecimal format
            //  uses hours if doRA is true
            //  note the AAVSO reports RA in degrees
            double d = Convert.ToDouble(sex);
            int dsign = Math.Sign(d);
            double dAbs = Math.Abs(d);
            if (doRA) //Convert RA degrees to hours
            {
                dAbs = dAbs * 24.0 / 360.0;
            }
            int degHrs = (int)(dAbs);
            dAbs -= degHrs;
            int min = (int)(dAbs * 60);
            dAbs -= (min / 60.0);
            double sec = dAbs * 3600;
            string degHrOut = String.Format("{00}", (dsign * degHrs)).PadLeft(2, '0');
            string minOut = String.Format("{00}", min).PadLeft(2, '0');
            string secOut = sec.ToString("0.000").PadLeft(5, '0');
            //return (dsign * degHrs).ToString("D" + 2) + ":" + min.ToString("I" + 2) + ":" + sec.ToString("D" + 5);
            string leadingSign = "";
            if (!doRA && dsign >= 0)
                leadingSign = "+";
            string sexOut = leadingSign + degHrOut + ":" + minOut + ":" + secOut;
            return sexOut;
        }

        public static string StringafyRecords(IEnumerable<XElement> dList)
        {
            string cbText = "";
            foreach (XElement tr in dList)
            {
                AAVSOData votable = new AAVSOData(tr);
                //Duplicate the TSX photo input format -- i.e. make it the same as the Harvard IUA display format
                //  as it is copied into the clipboard
                //Create a text string to be filled in for the clipboard: Column headings and two newlines.
                if (votable.VarType[0] == 'N')
                {
                    //Create a name that might fit in a 8 char slot -- AAVSO's won't
                    //Put the actual name in the "Galaxy" slot where there is a lot more room
                    string novaName = String.Concat(votable.Name.Where(c => !Char.IsWhiteSpace(c)));
                    string raSex = ParseToSexidecimal(votable.Coords_J2000_RA, true);
                    string decSex = ParseToSexidecimal(votable.Coords_J2000_Dec, false);
                    //Name
                    cbText += FitFormat(novaName, 8).PadRight(8);
                    //Name of the Host Galaxy as Milky Way
                    cbText += FitFormat("Milky Way", 17);
                    //Discovery Date
                    cbText += FitFormat(votable.NovaYear, 12);
                    //Truncated RA and Dec for locale
                    cbText += FitFormat(raSex, 8);
                    cbText += FitFormat(decSex, 12);
                    //Offsets?
                    cbText += "       ";  //offsets
                                          //Magnitude
                    cbText += FitFormat(votable.MaxMag, 8);
                    //Ext_catalogs"
                    cbText += FitFormat("", 15);
                    //Actual RA/Dec location
                    cbText += FitFormat(raSex, 12);
                    cbText += FitFormat(decSex, 14);
                    //filler for Position Reference
                    cbText += "                 ";
                    //Supernova Type
                    cbText += FitFormat(votable.VarType, 6);
                    //Supernova Name (as derived from entry name
                    //cbText += FitFormat(votable.Name, 8);
                    //Discoverer
                    cbText += FitFormat(votable.Discoverer, 20);
                    //New Line
                    cbText += "\n";
                }
            }
            return cbText;
        }

        public class AAVSOData
        {
            public AAVSOData(XElement trRecord)
            {
                const string AUID = "auid";
                const string NAME = "name";
                const string CONST = "const";
                const string COORDS_J2000 = "radec2000";
                const string VARTYPE = "varType";
                const string MAXMAG = "maxMag";
                const string MAXPASS = "maxPass";
                const string MINMAG = "minMag";
                const string MINPASS = "minPass";
                const string EPOCH = "epoch";
                const string NOVAYR = "novaYr";
                const string PERIOD = "period";
                const string RISEDUR = "riseDur";
                const string SPECTYPE = "specType";
                const string DISC = "disc";
                //Load the class structure
                List<XElement> records = trRecord.Elements("TD").ToList();
                Auid = records[0].Value;
                Name = records[1].Value;
                Const = records[2].Value;
                Coords_J2000_RA = records[3].Value.Split(',')[0];
                Coords_J2000_Dec = records[3].Value.Split(',')[1];
                VarType = records[4].Value;
                MaxMag = records[5].Value;
                MaxMagPassband = records[6].Value;
                MinMag = records[7].Value;
                MinMagPassband = records[8].Value;
                Epoch = records[9].Value;
                NovaYear = records[10].Value;
                if (NovaYear == "") NovaYear = "1000";
                Period = records[11].Value;
                RiseDuration = records[12].Value;
                SpecType = records[13].Value;
                Discoverer = records[14].Value;
                return;
            }

            public string Auid { get; set; }
            public string Name { get; set; }
            public string Const { get; set; }
            public string Coords_J2000_RA { get; set; }
            public string Coords_J2000_Dec { get; set; }
            public string VarType { get; set; }
            public string MaxMag { get; set; }
            public string MaxMagPassband { get; set; }
            public string MinMag { get; set; }
            public string MinMagPassband { get; set; }
            public string Epoch { get; set; }
            public string NovaYear { get; set; }
            public string Period { get; set; }
            public string RiseDuration { get; set; }
            public string SpecType { get; set; }
            public string Discoverer { get; set; }
        }
    }

}

