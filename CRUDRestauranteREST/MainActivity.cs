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
using System.Text;

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

        /*private async Task<string> Get()
        {
            // Create an HTTP web request using the URL:
            // Uri uri = new Uri("http://10.21.0.137/20131011110061/api/restaurante");
            Uri uri = new Uri("http://localhost:3906/api/restaurante");
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);

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
        }*/


        /*private void btnPost_Click(object sender, RoutedEventArgs e)
        {
            TestePost();
        }

        private void btnGet_Click(object sender, RoutedEventArgs e)
        {
            TesteGet();
        }*/

        public void TestePost()
        {
            Models.Restaurante x = new Models.Restaurante
            {
                Id = 1000,
                Descricao = "Teste"
            };
            string r = "=" + JsonConvert.SerializeObject(x);
            Post(r);
        }

        private void Post(string r)
        {
            // Create a request using a URL that can receive a post. 
            //WebRequest request = WebRequest.Create("http://10.21.0.137/20131011110061/api/restaurante");
            WebRequest request = WebRequest.Create("http://localhost:3906/api/restaurante");

            // Set the Method property of the request to POST.
            request.Method = "POST";

            // Create POST data and convert it to a byte array.
            string postData = r;
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            // Set the ContentType property of the WebRequest.
            request.ContentType = "application/x-www-form-urlencoded";
            // Set the ContentLength property of the WebRequest.
            request.ContentLength = byteArray.Length;
            // Get the request stream.
            Stream dataStream = request.GetRequestStream();
            // Write the data to the request stream.
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.
            dataStream.Close();
            // Get the response.
            WebResponse response = request.GetResponse();
            // Display the status.
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            // Get the stream containing content returned by the server.
            dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            // Display the content.
            Console.WriteLine(responseFromServer);
            // Clean up the streams.
            reader.Close();
            dataStream.Close();
            response.Close();
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Teste();
            SetContentView(Resource.Layout.Main);

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
            //int layout = Resource.Layout.ListItem;
            int layout = Resource.Layout.Main;
            //SimpleAdapter adapter = new SimpleAdapter(this, dados, layout, from, to);
            //this.ListAdapter = adapter;

            //ListView lv = FindViewById<ListView>(Resource.Id.lista);
            //lv.Adapter = adapter;

            
            EditText txtid = FindViewById<EditText>(Resource.Id.txtId);
            EditText txtdesc = FindViewById<EditText>(Resource.Id.txtDescricao);
            // ArrayList for data row
            // SimpleAdapter mapping static data to views in xml file
            SimpleAdapter adapter = new SimpleAdapter(this, dados, Resource.Layout.ListItem, from, to);

            //this.ListAdapter = adapter;
            ListView.Adapter = adapter;


            //http://www.worldbestlearningcenter.com/tips/Android-ListView-SimpleAdapter.htm
        }

        public void TesteGet()
        {
            string aeho = Get2();
            List<Models.Restaurante> list = JsonConvert.DeserializeObject<List<Models.Restaurante>>(aeho);
            //dataGrid.ItemsSource = list;
        }

        

        private string Get2()
        {
            // Create a request for the URL. 
            WebRequest request = WebRequest.Create("http://localhost:3906/api/restaurante");

            // If required by the server, set the credentials.
            // request.Credentials = CredentialCache.DefaultCredentials;

            // Get the response.
            WebResponse response = request.GetResponse();
            // Display the status.
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            // Get the stream containing content returned by the server.
            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            // Display the content.
            Console.WriteLine(responseFromServer);
            // Clean up the streams and the response.
            reader.Close();
            response.Close();

            return responseFromServer;
        }
    }
}

//http://stacktips.com/tutorials/xamarin/listview-example-in-xamarin-android

