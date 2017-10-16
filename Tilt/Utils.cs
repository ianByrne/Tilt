using Alexa.NET.Request;
using Amazon.Lambda.Core;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

//using Amazon.DynamoDBv2;
//using Amazon.DynamoDBv2.Model;

namespace Tilt
{
    /// <summary>
    /// Helper class to mostly handle global vars
    /// </summary>
    public static class Utils
    {
        public static SkillRequest Input;
        public static string AccessToken;
        public static string GoogleApplicationName;
        public static string SpreadsheetId;
        public static string UserId;
        public static ITiltDatabase TiltDatabase;
        private static ILambdaContext _context;

        /// <summary>
        /// Sets up the logger and env vars
        /// </summary>
        /// <param name="input">The SkillRequest passed by Alexa</param>
        /// <param name="context">The context passed to the function by AWS</param>
        public static void Init(SkillRequest input, ILambdaContext context)
        {
            Input = input;
            AccessToken = input.Session.User.AccessToken;
            UserId = input.Session.User.UserId;
            SpreadsheetId = Environment.GetEnvironmentVariable("spreadsheetId");
            _context = context;

            string session = JsonConvert.SerializeObject(Input.Session.Attributes);
            Log(session).Wait();
        }

        /// <summary>
        /// Writes a log line to Cloud Watch
        /// </summary>
        /// <param name="message">The message to log</param>
        public static async Task Log(string message)
        {
            // Cloud Watch
            _context.Logger.LogLine(message);
        }
    }
}
