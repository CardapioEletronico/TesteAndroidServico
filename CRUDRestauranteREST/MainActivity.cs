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
    public class MainActivity : ListActivity
    {
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
                    return jsonDoc.ToString();
                }
            }
        }

        private async void Post()
        {
            WebClient client = new WebClient();
            Uri uri = new Uri("http://10.21.0.137/20131011110061/api/restaurante");
            

        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Teste();
            //this.SetContentView(Resource.Layout.ListItem);

            //Button button = FindViewById<Button>(Resource.Id.btnSelect);
            //button.Click += delegate { Teste(); };
            
        }

        public async void Teste()
        {
            aeho = await Get();

            List<Models.Restaurante> list = JsonConvert.DeserializeObject<List<Models.Restaurante>>(aeho);
            IList<IDictionary<string, object>> dados = new List<IDictionary<string, object>>();
            foreach (Models.Restaurante r in list)
            {
                IDictionary<string, object> dado = new JavaDictionary<string, object>();
                dado.Add("Id", r.Id.ToString());
                dado.Add("Descricao", r.Descricao);
                dados.Add(dado);
            }

            string[] from = new String[] { "Id", "Descricao" };
            int[] to = new int[] {Resource.Id.idRest, Resource.Id.descRest };
            int layout = Resource.Layout.ListItem;
            SimpleAdapter adapter = new SimpleAdapter(this, dados, layout, from, to);
            this.ListAdapter = adapter;
        }
    }
}

//http://stacktips.com/tutorials/xamarin/listview-example-in-xamarin-android

