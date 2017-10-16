using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Xunit;
using Amazon.Lambda.TestUtilities;

using Alexa.NET.Request;
using Alexa.NET.Response;
using Alexa.NET.Request.Type;

namespace Tilt.Tests
{
    public class FunctionTest
    {
        /// <summary>
        /// Builds a test SkillRequest
        /// </summary>
        /// <param name="newSession">Whether or not this is a new session</param>
        /// <param name="sessionAttributes">Any attributes in the session</param>
        /// <param name="intentRequest">The request type and any slot details</param>
        /// <returns>The test SkillRequest</returns>
        private static SkillRequest BuildSkillRequest(bool newSession, Dictionary<string, object> sessionAttributes, IntentRequest intentRequest)
        {
            return new SkillRequest()
            {
                Session = new Session()
                {
                    New = newSession,
                    SessionId = "SessionId.00000000-0000-0000-0000-000000000000",
                    Attributes = sessionAttributes,
                    Application = new Application()
                    {
                        ApplicationId = "amzn1.ask.skill.00000000-0000-0000-0000-000000000000"
                    },
                    User = new User()
                    {
                        UserId = "amzn1.ask.account.TESTUSERID",
                        AccessToken = "TESTACCESSTOKEN"
                    }
                },
                Request = intentRequest,
                Context = new Context()
                {
                    AudioPlayer = new PlaybackState()
                    {
                        PlayerActivity = "IDLE"
                    },
                    System = new AlexaSystem()
                    {
                        Application = new Application()
                        {
                            ApplicationId = "amzn1.ask.skill.00000000-0000-0000-0000-000000000000"
                        },
                        User = new User()
                        {
                            UserId = "amzn1.ask.account.TESTUSERID",
                            AccessToken = "TESTACCESSTOKEN"
                        },
                        Device = new Device()
                        {
                            SupportedInterfaces = new Dictionary<string, object>()
                        }
                    }
                },
                Version = "1.0"
            };
        }

        [Fact]
        public async Task TestGetTilt_Google()
        {
            /*
            {
	            "session": {
		            "new": true,
		            "sessionId": "SessionId.00000000-0000-0000-0000-000000000000",
		            "application": {
			            "applicationId": "amzn1.ask.skill.00000000-0000-0000-0000-000000000000"
		            },
		            "attributes": {},
		            "user": {
			            "userId": "amzn1.ask.account.TESTUSERID",
                        "accessToken": "TESTACCESSTOKEN"
		            }
	            },
	            "request": {
		            "type": "IntentRequest",
		            "requestId": "EdwRequestId.00000000-0000-0000-0000-000000000000",
		            "intent": {
			            "name": "GetTiltIntent",
			            "slots": {}
		            },
		            "locale": "en-GB",
		            "timestamp": "2017-08-09T19:42:02Z"
	            },
	            "context": {
		            "AudioPlayer": {
			            "playerActivity": "IDLE"
		            },
		            "System": {
			            "application": {
				            "applicationId": "amzn1.ask.skill.00000000-0000-0000-0000-000000000000"
			            },
			            "user": {
				            "userId": "amzn1.ask.account.TESTUSERID",
                            "accessToken": "TESTACCESSTOKEN"
			            },
			            "device": {
				            "supportedInterfaces": {}
			            }
		            }
	            },
	            "version": "1.0"
            }
            */

            Dictionary<string, object> sessionAttributes = new Dictionary<string, object>();
            IntentRequest intentRequest = new IntentRequest
            {
                Type = "IntentRequest",
                RequestId = "EdwRequestId.00000000-0000-0000-0000-000000000000",
                Intent = new Intent()
                {
                    Name = "GetTiltIntent"
                },
                Locale = "en-GB",
                Timestamp = DateTime.Parse("2017-08-09T19:42:02Z")
            };

            SkillRequest skillRequest = BuildSkillRequest(true, sessionAttributes, intentRequest);

            var function = new Function();
            var context = new TestLambdaContext();

            Utils.Init(skillRequest, context);
            Utils.GoogleApplicationName = "Tilt";
            Utils.SpreadsheetId = "TESTSPREADSHEETID";
            Utils.TiltDatabase = new TiltDatabaseGoogle();

            SkillResponse skillResponse = await function.HandleRequest();

            var outputSpeech = (PlainTextOutputSpeech)skillResponse.Response.OutputSpeech;

            Assert.Equal("The shopping list contains eggs", outputSpeech.Text);
            Assert.Equal("PlainText", outputSpeech.Type);
        }

        [Fact]
        public async Task TestAddEggsToTilt_Google()
        {
            /*
            {
	            "session": {
		            "new": true,
		            "sessionId": "SessionId.00000000-0000-0000-0000-000000000000",
		            "application": {
			            "applicationId": "amzn1.ask.skill.00000000-0000-0000-0000-000000000000"
		            },
		            "attributes": {},
		            "user": {
			            "userId": "amzn1.ask.account.TESTUSERID",
                        "accessToken": "TESTACCESSTOKEN"
		            }
	            },
	            "request": {
		            "type": "IntentRequest",
		            "requestId": "EdwRequestId.00000000-0000-0000-0000-000000000000",
		            "intent": {
			            "name": "AddToTiltIntent",
			            "slots": {
				            "TiltItems": {
					            "name": "TiltItems",
					            "value": "eggs"
				            }
			            }
		            },
		            "locale": "en-GB",
		            "timestamp": "2017-08-09T20:25:03Z"
	            },
	            "context": {
		            "AudioPlayer": {
			            "playerActivity": "IDLE"
		            },
		            "System": {
			            "application": {
				            "applicationId": "amzn1.ask.skill.00000000-0000-0000-0000-000000000000"
			            },
			            "user": {
				            "userId": "amzn1.ask.account.TESTUSERID",
                            "accessToken": "TESTACCESSTOKEN"
			            },
			            "device": {
				            "supportedInterfaces": {}
			            }
		            }
	            },
	            "version": "1.0"
            }
            */

            Dictionary<string, object> sessionAttributes = new Dictionary<string, object>();
            IntentRequest intentRequest = new IntentRequest
            {
                Type = "IntentRequest",
                RequestId = "EdwRequestId.00000000-0000-0000-0000-000000000000",
                Intent = new Intent()
                {
                    Name = "AddToTiltIntent",
                    Slots = new Dictionary<string, Slot>()
                        {
                            {
                                "TiltItems",
                                new Slot()
                                {
                                    Name = "TiltItems",
                                    Value = "eggs"
                                }
                            }
                        }
                },
                Locale = "en-GB",
                Timestamp = DateTime.Parse("2017-08-09T19:42:02Z")
            };

            SkillRequest skillRequest = BuildSkillRequest(true, sessionAttributes, intentRequest);

            var function = new Function();
            var context = new TestLambdaContext();

            Utils.Init(skillRequest, context);
            Utils.GoogleApplicationName = "Tilt";
            Utils.SpreadsheetId = "TESTSPREADSHEETID";
            Utils.TiltDatabase = new TiltDatabaseGoogle();

            SkillResponse skillResponse = await function.HandleRequest();

            var outputSpeech = (PlainTextOutputSpeech)skillResponse.Response.OutputSpeech;

            Assert.Equal("Did you say eggs?", outputSpeech.Text);
            Assert.Equal("PlainText", outputSpeech.Type);
        }

        [Fact]
        public async Task TestConfirmAddEggsToTilt_Google()
        {
            /*
            {
	            "session": {
		            "new": false,
		            "sessionId": "SessionId.00000000-0000-0000-0000-000000000000",
		            "application": {
			            "applicationId": "amzn1.ask.skill.00000000-0000-0000-0000-000000000000"
		            },
		            "attributes": {
                        "AddToTiltIntent": "eggs"
                    },
		            "user": {
			            "userId": "amzn1.ask.account.TESTUSERID",
                        "accessToken": "TESTACCESSTOKEN"
		            }
	            },
	            "request": {
		            "type": "IntentRequest",
		            "requestId": "EdwRequestId.00000000-0000-0000-0000-000000000000",
		            "intent": {
			            "name": "AMAZON.YesIntent",
			            "slots": {}
			            }
		            },
		            "locale": "en-GB",
		            "timestamp": "2017-08-09T20:25:03Z"
	            },
	            "context": {
		            "AudioPlayer": {
			            "playerActivity": "IDLE"
		            },
		            "System": {
			            "application": {
				            "applicationId": "amzn1.ask.skill.00000000-0000-0000-0000-000000000000"
			            },
			            "user": {
				            "userId": "amzn1.ask.account.TESTUSERID",
                            "accessToken": "TESTACCESSTOKEN"
			            },
			            "device": {
				            "supportedInterfaces": {}
			            }
		            }
	            },
	            "version": "1.0"
            }
            */

            Dictionary<string, object> sessionAttributes = new Dictionary<string, object>
            {
                { "AddToTiltIntent", "eggs" }
            };

            IntentRequest intentRequest = new IntentRequest
            {
                Type = "IntentRequest",
                RequestId = "EdwRequestId.00000000-0000-0000-0000-000000000000",
                Intent = new Intent()
                {
                    Name = "AMAZON.YesIntent",
                    Slots = new Dictionary<string, Slot>()
                },
                Locale = "en-GB",
                Timestamp = DateTime.Parse("2017-08-09T19:42:02Z")
            };

            SkillRequest skillRequest = BuildSkillRequest(false, sessionAttributes, intentRequest);

            var function = new Function();
            var context = new TestLambdaContext();

            Utils.Init(skillRequest, context);
            Utils.GoogleApplicationName = "Tilt";
            Utils.SpreadsheetId = "TESTSPREADSHEETID";
            Utils.TiltDatabase = new TiltDatabaseGoogle();

            SkillResponse skillResponse = await function.HandleRequest();

            var outputSpeech = (PlainTextOutputSpeech)skillResponse.Response.OutputSpeech;

            Assert.Equal("eggs was added to the shopping list", outputSpeech.Text);
            Assert.Equal("PlainText", outputSpeech.Type);
        }

        [Fact]
        public async Task TestClearTilt_Google()
        {
            /*
            {
	            "session": {
		            "new": true,
		            "sessionId": "SessionId.00000000-0000-0000-0000-000000000000",
		            "application": {
			            "applicationId": "amzn1.ask.skill.00000000-0000-0000-0000-000000000000"
		            },
		            "attributes": {},
		            "user": {
			            "userId": "amzn1.ask.account.TESTUSERID",
                        "accessToken": "TESTACCESSTOKEN"
		            }
	            },
	            "request": {
		            "type": "IntentRequest",
		            "requestId": "EdwRequestId.00000000-0000-0000-0000-000000000000",
		            "intent": {
			            "name": "ClearTiltIntent",
			            "slots": {}
		            },
		            "locale": "en-GB",
		            "timestamp": "2017-08-09T20:30:43Z"
	            },
	            "context": {
		            "AudioPlayer": {
			            "playerActivity": "IDLE"
		            },
		            "System": {
			            "application": {
				            "applicationId": "amzn1.ask.skill.00000000-0000-0000-0000-000000000000"
			            },
			            "user": {
				            "userId": "amzn1.ask.account.TESTUSERID",
                            "accessToken": "TESTACCESSTOKEN"
			            },
			            "device": {
				            "supportedInterfaces": {}
			            }
		            }
	            },
	            "version": "1.0"
            }
            */

            Dictionary<string, object> sessionAttributes = new Dictionary<string, object>();

            IntentRequest intentRequest = new IntentRequest
            {
                Type = "IntentRequest",
                RequestId = "EdwRequestId.00000000-0000-0000-0000-000000000000",
                Intent = new Intent()
                {
                    Name = "ClearTiltIntent"
                },
                Locale = "en-GB",
                Timestamp = DateTime.Parse("2017-08-09T19:42:02Z")
            };

            SkillRequest skillRequest = BuildSkillRequest(true, sessionAttributes, intentRequest);

            var function = new Function();
            var context = new TestLambdaContext();

            Utils.Init(skillRequest, context);
            Utils.GoogleApplicationName = "Tilt";
            Utils.SpreadsheetId = "TESTSPREADSHEETID";
            Utils.TiltDatabase = new TiltDatabaseGoogle();

            SkillResponse skillResponse = await function.HandleRequest();

            var outputSpeech = (PlainTextOutputSpeech)skillResponse.Response.OutputSpeech;

            Assert.Equal("Are you sure you want to clear the list?", outputSpeech.Text);
            Assert.Equal("PlainText", outputSpeech.Type);
        }

        [Fact]
        public async Task TestConfirmClearTilt_Google()
        {
            /*
            {
	            "session": {
		            "new": false,
		            "sessionId": "SessionId.00000000-0000-0000-0000-000000000000",
		            "application": {
			            "applicationId": "amzn1.ask.skill.00000000-0000-0000-0000-000000000000"
		            },
		            "attributes": {
                        "ClearTiltIntent": true
                    },
		            "user": {
			            "userId": "amzn1.ask.account.TESTUSERID",
                        "accessToken": "TESTACCESSTOKEN"
		            }
	            },
	            "request": {
		            "type": "IntentRequest",
		            "requestId": "EdwRequestId.00000000-0000-0000-0000-000000000000",
		            "intent": {
			            "name": "AMAZON.YesIntent",
			            "slots": {}
			            }
		            },
		            "locale": "en-GB",
		            "timestamp": "2017-08-09T20:25:03Z"
	            },
	            "context": {
		            "AudioPlayer": {
			            "playerActivity": "IDLE"
		            },
		            "System": {
			            "application": {
				            "applicationId": "amzn1.ask.skill.00000000-0000-0000-0000-000000000000"
			            },
			            "user": {
				            "userId": "amzn1.ask.account.TESTUSERID",
                            "accessToken": "TESTACCESSTOKEN"
			            },
			            "device": {
				            "supportedInterfaces": {}
			            }
		            }
	            },
	            "version": "1.0"
            }
            */

            Dictionary<string, object> sessionAttributes = new Dictionary<string, object>
            {
                { "ClearTiltIntent", true }
            };

            IntentRequest intentRequest = new IntentRequest
            {
                Type = "IntentRequest",
                RequestId = "EdwRequestId.00000000-0000-0000-0000-000000000000",
                Intent = new Intent()
                {
                    Name = "AMAZON.YesIntent",
                    Slots = new Dictionary<string, Slot>()
                },
                Locale = "en-GB",
                Timestamp = DateTime.Parse("2017-08-09T19:42:02Z")
            };

            SkillRequest skillRequest = BuildSkillRequest(false, sessionAttributes, intentRequest);

            var function = new Function();
            var context = new TestLambdaContext();

            Utils.Init(skillRequest, context);
            Utils.GoogleApplicationName = "Tilt";
            Utils.SpreadsheetId = "TESTSPREADSHEETID";
            Utils.TiltDatabase = new TiltDatabaseGoogle();

            SkillResponse skillResponse = await function.HandleRequest();

            var outputSpeech = (PlainTextOutputSpeech)skillResponse.Response.OutputSpeech;

            Assert.Equal("The shopping list has been cleared.", outputSpeech.Text);
            Assert.Equal("PlainText", outputSpeech.Type);
        }
    }
}
