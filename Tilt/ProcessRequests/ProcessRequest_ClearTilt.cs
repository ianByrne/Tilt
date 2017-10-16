using Alexa.NET;
using Alexa.NET.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tilt
{
    public static partial class ProcessRequest
    {
        /// <summary>
        /// Clears the shopping list and asks if they want to add something else
        /// </summary>
        /// <returns>Bunch of words</returns>
        public static async Task<SkillResponse> ClearTilt(bool confirmed)
        {
            await Utils.Log("ClearTilt request made");

            string outputSpeech = "";
            string repromptSpeech = "";

            if (confirmed)
            {
                await Utils.TiltDatabase.ClearItems();

                await Utils.Log("List cleared");

                outputSpeech = "The shopping list has been cleared.";
                repromptSpeech = "Say add milk to add milk";
            }
            else
            {
                await Utils.Log("Confirming clear");

                outputSpeech = "Are you sure you want to clear the list?";
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
                response.SessionAttributes.Add("ClearTiltIntent", true);
            }

            return response;
        }
    }
}
