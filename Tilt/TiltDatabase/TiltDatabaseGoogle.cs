using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tilt
{
    /// <summary>
    /// A Tilt database using Google Sheets
    /// </summary>
    public class TiltDatabaseGoogle : ITiltDatabase
    {
        private readonly string[] _scopes = { SheetsService.Scope.Spreadsheets };
        private SheetsService _service { get; set; }

        /// <summary>
        /// Connects to Google and creates a new SheetsService
        /// </summary>
        public TiltDatabaseGoogle()
        {
            if (Utils.AccessToken == "TESTACCESSTOKEN")
            {
                // Do not attempt to connect to Google if this is a test
            }
            else
            {
                var credential = GoogleCredential.FromAccessToken(Utils.AccessToken).CreateScoped(_scopes);

                _service = new SheetsService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = Utils.GoogleApplicationName
                });

                // TO DO check if spreadsheet already exists, and create if not?
            }
        }

        /// <summary>
        /// Creates a new sheet named Tilt
        /// </summary>
        /// <returns>Task</returns>
        private async Task CreateSheet()
        {
            if (Utils.AccessToken == "TESTACCESSTOKEN")
            {
                // Do not attempt to create a sheet if this is a test
            }
            else
            {
                Spreadsheet requestBody = new Spreadsheet();
                requestBody.Properties = new SpreadsheetProperties()
                {
                    Title = "Tilt"
                };

                SpreadsheetsResource.CreateRequest request = _service.Spreadsheets.Create(requestBody);

                Spreadsheet response = await request.ExecuteAsync();

                Utils.Log(JsonConvert.SerializeObject(response)).Wait();
                Utils.Log($"New sheetId: {response.SpreadsheetId}").Wait();
            }
        }

        /// <summary>
        /// Returns the items in the shopping list
        /// </summary>
        /// <returns>A list of items</returns>
        public async Task<List<string>> GetItems()
        {
            List<string> items = new List<string>();

            if (Utils.SpreadsheetId == "TESTSPREADSHEETID")
            {
                // If this is a test, return some test data
                items.Add("eggs");
            }
            else
            {
                // The items are listed in the first column (A) of the sheet named 'Tilt'
                string range = "Tilt!A:A";
                SpreadsheetsResource.ValuesResource.GetRequest request = _service.Spreadsheets.Values.Get(Utils.SpreadsheetId, range);

                ValueRange response = await request.ExecuteAsync();
                IList<IList<Object>> rows = response.Values;
                if (rows != null && rows.Count > 0)
                {
                    foreach (var row in rows)
                    {
                        // The first index of the row [0] is the first column (A)
                        items.Add(row[0].ToString());
                    }
                }
                else
                {
                    // No items found
                }
            }

            return items;
        }

        /// <summary>
        /// Adds items to the shopping list
        /// </summary>
        /// <param name="items">The list of items to add</param>
        /// <returns>Task</returns>
        public async Task AddItem(string item)
        {
            if (Utils.SpreadsheetId == "TESTSPREADSHEETID")
            {
                // Do not attempt to add anything if this is a test
            }
            else
            {
                // The items are listed in the first column (A) of the sheet named 'Tilt'
                string range = "Tilt!A:A";
                ValueRange valueRange = new ValueRange();
                IList<IList<object>> rows = new List<IList<object>>();

                IList<object> cells = new List<object>();
                cells.Add(item);
                rows.Add(cells);

                valueRange.Values = rows;

                SpreadsheetsResource.ValuesResource.AppendRequest request = _service.Spreadsheets.Values.Append(valueRange, Utils.SpreadsheetId, range);
                request.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.RAW;

                var response = await request.ExecuteAsync();
            }
        }

        /// <summary>
        /// Clears the shopping list
        /// </summary>
        /// <returns>Task</returns>
        public async Task ClearItems()
        {
            if (Utils.SpreadsheetId == "TESTSPREADSHEETID")
            {
                // Do not attempt to clear anything if this is a test
            }
            else
            {
                // The items are listed in the first column (A) of the sheet named 'Tilt'
                string range = "Tilt!A:A";
                ClearValuesRequest valueRequest = new ClearValuesRequest();

                SpreadsheetsResource.ValuesResource.ClearRequest request = _service.Spreadsheets.Values.Clear(valueRequest, Utils.SpreadsheetId, range);

                var response = await request.ExecuteAsync();
            }
        }
    }
}
