using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Net;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Json;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CRUDRestauranteREST
{
    [Activity(Label = "CRUDRestauranteREST", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        List<Models.Restaurante> lista;
        ArrayAdapter<Models.Restaurante> adapter;
        private JsonValue aeho;
        private async Task<JsonValue> Get()
        {
            // Create an HTTP web request using the URL:
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri("http://10.21.0.137/20131011110061/api/restaurante"));
            //request.ContentType = "10.21.0.137/20131011110061/api/restaurante";
            request.Method = "GET";

            // Send the request to the server and wait for the response:
            using (WebResponse response = await request.GetResponseAsync())
            {
                // Get a stream representation of the HTTP web response:
                using (Stream stream = response.GetResponseStream())
                {
                    // Use this stream to build a JSON document object:
                    JsonValue jsonDoc = await Task.Run(() => JsonObject.Load(stream));
                    Console.Out.WriteLine("Response: {0}", jsonDoc.ToString());

                    // Return the JSON document:
                    return jsonDoc;
                    
                    

                }
            }
        }
        protected async override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            Button get = FindViewById<Button>(Resource.Id.button1);

            get.Click += delegate { get.Text = string.Format("{0} clicks!"); };

            ListView select = FindViewById<ListView>(Resource.Id.lulu);

            aeho = await Get();

            List<Models.Restaurante> list = JsonConvert.DeserializeObject<List<Models.Restaurante>>(aeho);

            ListView mozovo = FindViewById<ListView>(Resource.Id.lulu);

            ArrayAdapter adapter = new ArrayAdapter<Models.Restaurante>(this, Android.Resource.Layout.SimpleListItem1, list);
            mozovo.Adapter = adapter;

            /*string content = aeho.ToString(); 
            mozovo.Text = content;*/

            //ArrayAdapter <string> asa = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, new int[] { Android.R},aeho.ToString());
           
        }
    }
}

