using Alexa.NET;
using Alexa.NET.Response;
using System.Threading.Tasks;

namespace Tilt
{
    public static partial class ProcessRequest
    {
        /// <summary>
        /// Returns what's currently on the list
        /// </summary>
        /// <returns>Bunch of words</returns>
        public static async Task<SkillResponse> GetTilt()
        {
            await Utils.Log("GetTilt request made");

            string outputSpeech = "";
            
            var items = await Utils.TiltDatabase.GetItems();

            if (items.Count > 0)
            {
                outputSpeech = $"The shopping list contains {string.Join(", ", items)}";
            }
            else
            {
                outputSpeech = "The shopping list is empty";
            }

            var speech = new PlainTextOutputSpeech();
            speech.Text = outputSpeech;

            var repromptMessage = new PlainTextOutputSpeech();
            repromptMessage.Text = "Say add milk to add milk";

            var repromptBody = new Reprompt();
            repromptBody.OutputSpeech = repromptMessage;

            var response = ResponseBuilder.Ask(speech, repromptBody);

            return response;
        }
    }
}
