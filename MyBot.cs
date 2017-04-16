using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace DiscordBot
{
    class MyBot
    {
        DiscordClient discord;

        public MyBot()
        {
            discord = new DiscordClient(x =>
                {
                    
                    x.LogLevel = LogSeverity.Info;
                    x.LogHandler = Log;
                });

            discord.UsingCommands(x =>
            {
                x.PrefixChar = '!';                
                x.AllowMentionPrefix = true;
            });

            var commands = discord.GetService<CommandService>();

            commands.CreateCommand("time").Parameter("params", ParameterType.Multiple)
                .Do(async (e) =>
                {
                    await e.Channel.SendMessage(pickMethod(e));
                });

            
            commands.CreateCommand("test").Parameter("params", ParameterType.Multiple)
                .Do(async (e) =>
                {                    
                    await e.Channel.SendMessage("this is a test");
                });
            

            discord.ExecuteAndWait(async () =>
            {
                await discord.Connect("MzAyNjM0MTg0MzQ0NjAwNTg2.C9QI6g.WMRycgkDKXIauFYRiQz3ovC95AU", TokenType.Bot);
            });
        }

        private async Task testmethod(CommandEventArgs e)
        {
            //var channel = e.Server.FindChannels(e.Args[0], ChannelType.Text).FirstOrDefault();
            //var message = ConstructMessage(e, channel != null);
            var message = pickMethod(e);
            /*if (channel != null)
            //{
            //    await channel.SendMessage(message);
            //} else
            //{
                await e.Channel.SendMessage(message);
            }*/
        }

        private string pickMethod(CommandEventArgs e)
        {
            switch (e.Args[0].ToLower())
            {
                case "create":
                    return createMethod(e);
                case "whenis":
                    whenisMethod(e);
                    break;
                case "timetill":
                    timetillMethod(e);
                    break;
                case "list":
                    listMethod(e);
                    break;
                case "delete":
                    deleteMethod(e);
                    break;
                case "alarm":
                    alarmMethod(e);
                    break;
                case "register":
                    registerMethod(e);
                    break;
                default:
                    helpMethod(e);
                    break;
            }
            return "How did you get here?";            
        }

        private void Log(object sender, LogMessageEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
        
//COMMAND METHODS------------------------------------------------------------------------------------------------------------------------------------------
        private string helpMethod(CommandEventArgs e)
        {
            return "HELP!";
            //Kinda want to figure out a better way to do this than just hardcoding everything, maybe read from a file or use Command objects?
            throw new NotImplementedException();
        }

        private string createMethod(CommandEventArgs e)
        {
            int paramNum = e.Args.Length;
            if (paramNum == 1)
                return "This shouldn't be possible"; //The command to create a new event is !time new <\"Event Name\"> <\"Event Date\"> <Event Time> <Your Timezone>";            
            if(paramNum < 4)
            {
                return "Not Ready Yet"; //Should instead start a sequence of asking the user to fill out all the variables individually
            }
            ComCreate command = new ComCreate(e.Args[1], e.Args[2], e.Args[3], e.Args[4]);

            return "Added " + command.toString();
            
            //Analyze name, maybe return an event id?
            //Analyze date, make numbers into words, eliminate /'s and the like
            //Analyze time/timezone, write UTC times into file

            //use a class to hold all of this info

            //WRITE EVENT TO FILE
            //using system.io
            //file.createtext
            //file.writealltext()

            //return "Added " + name + " on " + date + " at " + time + "timezone";
        }

        private string whenisMethod(CommandEventArgs e)
        {
            //Exceptions:
            //e.Args[1] is null/doesn't exist?

            //pull event named e.Args[1] from file and return.
            return "Not ready yet";
        }

        private void timetillMethod(CommandEventArgs e)
        {
            //Exceptions:
            //e.Args[1] is null/doesn't exist?

            //pull time of event named e.Args[1] from file and compare to current time, return the difference.
            throw new NotImplementedException();
        }

        private void listMethod(CommandEventArgs e)
        {
            //Exceptions:
            //none

            //read file and list all upcoming events in a pm to the user
            //directory.getFiles
            throw new NotImplementedException();
        }

        private void alarmMethod(CommandEventArgs e)
        {
            //Exceptions:
            //e.Args is missing a date, time, or timezone

            //creates an event in alarms.txt and reminds user later or something
            //low priority
            throw new NotImplementedException();
        }

        private void deleteMethod(CommandEventArgs e)
        {

            throw new NotImplementedException();
        }

        private void registerMethod(CommandEventArgs e)
        {
            throw new NotImplementedException();
        }











    }

}

//change it to write to file when updated, and check if file exists when starting
/*Choose a function to perform
//new: Creates an event with a name, date, time, and timezone
//whenis: Returns the time of a named event with the user's timezone of choice
//timetill: Returns how many days/hours/minutes until a named event
//delete: Deletes an existing named event
//list: lists all events
//alarm: passed a date, time and timezone, remembers to pm a user at that time
//register: registers a user so that they may leave the timezone field empty
//help lists all commands in time (the default answer)

    last/lastevent: lets requester know when the last event occured
    history: creates a list of all past events to pm to the user

new: name, date, time, timezone
whenis: name, timezone(optional)
timetill: name
delete: name
list: none
alarm: date, time, timezone
register: timezone
help: none
*/

/*FUTURE VERSIONS:
 * Create a 'command' class, which contains information about each command (name, args, description)
 *  Follow this up by creating an 'add command method' and a list/array of commands
 *  change the switchboard into an if/else statement, where the if somehow has a command which can call a function based on the name of the command called
 *      (such as, if the parameter matches a command's name, then it will run the command's function? (Can you link a class to a function like that?)
 * 
 * Create a file for registration, which will be read prior to doing any of the following methods: new, whenis, alarm     
 * Add a system to manage permissions
 * 
 * Figure out how to put parameters in other brackets together (), [], {}, //, \\, <> - not urgent
 * 
 * Add function last or lastevent, which will tell you when the last event was and how long it's been since it passed
 * 
 * Make it so that any of the common prefix characters can be used with regex?
 * 
 * 
 *      
 *SCRAPPED CODE:
 * Construct message puts together all the parameters passed to the method, then returns them as a string
 * private string ConstructMessage(CommandEventArgs e, bool firstArgIsChannel)
        {
            string message = "";
            var name = e.User.Nickname != null ? e.User.Nickname : e.User.Name;
            var startIndex = firstArgIsChannel ? 1 : 0;

            for(int i = startIndex; i < e.Args.Length; i++)
            {
                message += e.Args[i].ToString() + " ";
            }

            var result = name + " says: " + message;
            return result;
        }
 */

