using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace DiscordBot
{
    class ComCreate
    {
        public string name ="";
        private string date = DateTime.Today.ToString();
        private string time = "00:00";
        private string tz = "UTC";
        private int timeOverflow = 0;

        public ComCreate(string pname, string pdate, string ptime, string ptz)
        {
            name = pname;
            date = pdate;
            time = ptime;
            tz = ptz;
        }
        
        public ComCreate(string pname, string pdate, string ptime)
        {
            name = pname;
            date = pdate;
            time = ptime;
        }

        public string[] parse()
        {
            string t = parseTime();
            string d = parseDate();
            return new string[] { name, t, d };
        }

        public string toString()
        {
            string[] pieces = parse();
            string s = name + " on " + pieces[2] + " at " + pieces[1];
            return s;
        }

//--------------------------------PRIVATE METHODS-------------------------------
        private string parseTime()
        {
            string[] timeInfo = time.Split(':');
            int min;
            int hr = Int32.Parse(timeInfo[0]);
            if (timeInfo.Length < 2)
                min = 0;
            else
                min = Int32.Parse(timeInfo[1]);

            int seperation = parseTimezone();
            hr += seperation;

            if(hr < 0)
            {
                timeOverflow = -1;
                hr = 24 - hr;
            }

            if (hr >= 24)
            {
                hr = hr - 24;
                timeOverflow = 1;
            }

            return "" + hr +":"+ min + " UTC";
            //We're passed a time in format XX:XX AM (12hr) or XX:XX (24hr), and a timezone
            //We want to return a time in the same format, but in the timezone UTC
            /* Steps:
             * 1: Split the time we're passed into multiple different chunks on the first and second colon, as well as the space
             * 2: This gives us [HH],[MM],[SS],[(AM/PM)]
             * 2.5: Maybe throw an error if this array is longer than 4?
             * 3: Individually parse each element, getting us up to three integers and one string
             * 3.5: Deal with AM/PM fuckery
             * 4: Compare the timezone we're passed to all the different timezones in the world (maybe something already exists for this?)
             * 4.5: Consider seperating this out into its own private method, which returns an integer seperation (the hours of seperation between the timezone and UTC)
             * 5: Add seperation to the integer for hours
             * 6: If hours becomes <0 or >=24, set timeOverflow to true to indicate a change in date
             * 7: Put all of the info back together into a string like so: "HH:MM:SS AM/PM"
             * 8: 
             */
        }

        private int parseTimezone()
        {
            int seperation;
            if (Int32.TryParse(tz, out seperation))
            {
                return seperation;
            }
            //create a list of common timezones
            /*
            switch (tz.ToUpper()) //Eventually this will be replaced with reading from a file of timezones
            {
                case "UTC":
                    return 0;
                case "CET":
                    return 1;
                case "EET":
                    return 2;
                case "FET":
                    return 3;
                case "AMT":
                    return 4;
                case "PKT":
                    return 5;
                case "BST":
                    return 6;
                case "THA":
                    return 7;
                case "WST":
                    return 8;
                case "JST":
                    return 9;
                case "AEST":
                    return 10;
                case "NFT":
                    return 11;
                case "NZST":
                    return 12;
                case "PHOT":
                    return 13;
                case "CVT":
                    return -1;
                case "FNT":
                    return -2;
                case "BRT":
                    return -3;
                case "AST":
                    return -4;
                case "CDT":
                    return -5;
                case "CST":
                    return -6;
                case "MST":
                    return -7;
                case "PST":
                    return -8;
                case "AKST":
                    return -9;
                case "HAST":
                    return -10;
                case "SST":
                    return -11;
                case "BIT":
                    return -12;                
            }*/
            return 0;
        }

        private string parseDate()
        {
            DateTime fDate = DateTime.Parse(date);
            fDate.AddDays(timeOverflow);
            return fDate.ToShortDateString();
            //several ways to write dates to be parsed
            //output should be in form: Month Day, Year
        }
        //date: (string date) r String?
        //time: (time, timezone) r String?
    }
}
