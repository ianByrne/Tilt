using Alexa.NET;
using Alexa.NET.Response;
using System.Threading.Tasks;

namespace Tilt
{
    public static partial class ProcessRequest
    {
        /// <summary>
        /// Asks the user what they want to do
        /// </summary>
        /// <returns>Bunch of words</returns>
        public static async Task<SkillResponse> Launch()
        {
            await Utils.Log("Default LaunchRequest made");

            var speech = new PlainTextOutputSpeech();
            speech.Text = "Howdy!";

            var repromptMessage = new PlainTextOutputSpeech();
            repromptMessage.Text = "Say add milk to add milk";

            var repromptBody = new Reprompt();
            repromptBody.OutputSpeech = repromptMessage;

            var response = ResponseBuilder.Ask(speech, repromptBody);

            return response;
        }
    }
}
