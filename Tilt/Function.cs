using Amazon.Lambda.Core;
using System.Threading.Tasks;

// https://github.com/timheuer/alexa-skills-dotnet
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace Tilt
{
    public class Function
    {
        /// <summary>
        /// The main/start method
        /// </summary>
        /// <param name="input">Input from Alexa</param>
        /// <param name="context">Context from Lambda</param>
        /// <returns>Alexa response</returns>
        public async Task<SkillResponse> FunctionHandler(SkillRequest input, ILambdaContext context)
        {
            // TO DO mobile app
            // TO DO implement dynamoDb instead of sheets
            // TO DO try to remember how to package these things - dotnet lambda package -f netstandard1.6
            Utils.Init(input, context);
            Utils.GoogleApplicationName = "Tilt";
            Utils.TiltDatabase = new TiltDatabaseGoogle();

            SkillResponse skillResponse = await HandleRequest();

            return skillResponse;
        }

        /// <summary>
        /// Checks the request type and performs the appropriate task
        /// </summary>
        /// <returns>The processed response</returns>
        public async Task<SkillResponse> HandleRequest()
        {
            SkillResponse response = new SkillResponse();

            await Utils.Log($"Request type: {Utils.Input.Request.GetType().ToString()}");

            // DEFAULT LAUNCH REQUEST
            if (Utils.Input.Request is LaunchRequest)
            {
                response = await ProcessRequest.Launch();
            }
            // INTENT REQUEST
            else if (Utils.Input.Request is IntentRequest)
            {
                string intentRequestName = ((IntentRequest)Utils.Input.Request).Intent.Name;
                var session = Utils.Input.Session.Attributes;

                // GET TILT - return the shopping list
                if (intentRequestName == "GetTiltIntent")
                {
                    response = await ProcessRequest.GetTilt();
                }
                // INITIAL ADD TO TILT - check if Alexa heard the correct item to add
                else if (intentRequestName == "AddToTiltIntent")
                {
                    response = await ProcessRequest.AddToTilt(false);
                }
                // CONFIRMED ADD TO TILT - add item to list
                else if (intentRequestName == "AMAZON.YesIntent" && session.ContainsKey("AddToTiltIntent"))
                {
                    response = await ProcessRequest.AddToTilt(true);
                }
                // INITIAL CLEAR TILT - check if Alexa heard to clear the list
                else if (intentRequestName == "ClearTiltIntent")
                {
                    response = await ProcessRequest.ClearTilt(false);
                }
                // CONFIRMED CLEAR TILT - clear the list
                else if (intentRequestName == "AMAZON.YesIntent" && session.ContainsKey("ClearTiltIntent"))
                {
                    response = await ProcessRequest.ClearTilt(true);
                }
                // HEARD INCORRECTLY - restart
                else if (intentRequestName == "AMAZON.NoIntent")
                {
                    response = await ProcessRequest.Launch();
                }
                // ERROR
                else
                {
                    await Utils.Log($"Unknown IntentRequest: {intentRequestName}");
                    response = await ProcessRequest.Error();
                }
            }
            // SESSION ENDED REQUEST
            else if (Utils.Input.Request is SessionEndedRequest)
            {
                response = await ProcessRequest.SessionEnded();
            }
            // UNKNOWN REQUEST
            else
            {
                await Utils.Log("Unknown request type");
                response = await ProcessRequest.Error();
            }

            return response;
        }
    }
}
