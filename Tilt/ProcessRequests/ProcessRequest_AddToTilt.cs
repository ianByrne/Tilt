using Alexa.NET;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tilt
{
    public static partial class ProcessRequest
    {
        /// <summary>
        /// Confirms and then adds an item to the list
        /// </summary>
        /// <returns>Bunch of words</returns>
        public static async Task<SkillResponse> AddToTilt(bool confirmed)
        {
            await Utils.Log("AddToTilt request made");

            string outputSpeech = "";
            string repromptSpeech = "";
            string item = "";

            if (confirmed)
            {
                item = Utils.Input.Session.Attributes["AddToTiltIntent"].ToString();
                
                await Utils.TiltDatabase.AddItem(item);

                await Utils.Log($"Added: {item}");

                outputSpeech = $"{item} was added to the shopping list";
                repromptSpeech = "Say add milk to add milk";
            }
            else
            {
                var slots = ((IntentRequest)Utils.Input.Request).Intent.Slots;
                item = slots["TiltItems"].Value.ToString();

                await Utils.Log($"Heard: {item}");

                outputSpeech = $"Did you say {item}?";
                repromptSpeech = outputSpeech;
            }

            var speech = new PlainTextOutputSpeech();
            speech.Text = outputSpeech;

            var repromptMessage = new PlainTextOutputSpeech();
            repromptMessage.Text = repromptSpeech;

            var repromptBody = new Reprompt();
            repromptBody.OutputSpeech = repromptMessage;

            var response = ResponseBuilder.Ask(speech, repromptBody);

            if (!confirmed)
            {
                response.SessionAttributes = new Dictionary<string, object>();
                response.SessionAttributes.Add("AddToTiltIntent", item);
            }

            return response;
        }
    }
}
