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

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            LoadContent();

            //Button button = FindViewById<Button>(Resource.Id.btnSelect);
            //button.Click += delegate { Teste(); };


            //Atualizar conteúdo
            Button btnGet = FindViewById<Button>(Resource.Id.btnGet);
            btnGet.Click += delegate { LoadContent(); };
            //Adicionar restaurante
            Button btnPost = FindViewById<Button>(Resource.Id.btnPost);
            btnPost.Click += delegate { Postar(); };
            //Update de conteúdo
            Button btnPut = FindViewById<Button>(Resource.Id.btnPut);
            btnPut.Click += delegate { Put(); };

        }
        //MÉTODO GET HHTPWEBREQUEST
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
        //MÉTODO GET WEBREQUEST
        private string Get2()
        {
            // Create a request for the URL. 
            WebRequest request = WebRequest.Create("http://10.21.0.137/20131011110061/api/restaurante");

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

        private void Put()
        {
            EditText txtId = FindViewById<EditText>(Resource.Id.txtId);
            EditText txtDesc = FindViewById<EditText>(Resource.Id.txtDescricao);
            Models.Restaurante x = new Models.Restaurante
            {
                Id = 1,
                Descricao = "Alo",
            };

            var request = (HttpWebRequest)WebRequest.Create(postUrl);
            request.Method = "PUT";
            request.ContentType = "application/xml";
            if (x != null)
            {
                request.ContentLength = Size(x);
                Stream dataStream = request.GetRequestStream();
                Serialize(dataStream, x);
                dataStream.Close();
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string returnString = response.StatusCode.ToString();
        }

        public void Serialize(Stream output, object input)
        {
            var ser = new DataContractSerializer(input.GetType());
            ser.WriteObject(output, input);
        }
    

        /*var bytes = Encoding.ASCII.GetBytes(r);
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("http://10.21.0.137/20131011110061/api/restaurante"));
        request.Method = "PUT";
        request.ContentType = "application/x-www-form-urlencoded";
        using (var requestStream = request.GetRequestStream())
        {
            requestStream.Write(bytes, 0, bytes.Length);
        }
        var response = (HttpWebResponse)request.GetResponse();*/

        /*using (var client = new System.Net.WebClient())
        {
            client.UploadData("http://10.21.0.137/20131011110061/api/restaurante", "PUT", r);
        }
        response = await client.PutAsync(uri, content);*/
    }

        public void Update()
        {
            EditText txtId = FindViewById<EditText>(Resource.Id.txtId);
            EditText txtDesc = FindViewById<EditText>(Resource.Id.txtDescricao);
            Models.Restaurante x = new Models.Restaurante
            {
                Id = int.Parse(txtId.Text),
                Descricao = txtDesc.Text,
            };
            string r = "=" + JsonConvert.SerializeObject(x);
            string postData = r;
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            Put();
        }

        //MÉTODO POST
        private void Post(string r)
        {
            // Create a request using a URL that can receive a post. 
            //WebRequest request = WebRequest.Create("http://10.21.0.137/20131011110061/api/restaurante");
            WebRequest request = WebRequest.Create("http://10.21.0.137/20131011110061/api/restaurante");

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

        //LOAD LISTA E PÁGINA
        public async void LoadContent()
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
            int[] to = new int[] { Resource.Id.idRest, Resource.Id.descRest };
            int layout = Resource.Layout.ListItem;

            EditText txtid = FindViewById<EditText>(Resource.Id.txtId);
            EditText txtdesc = FindViewById<EditText>(Resource.Id.txtDescricao);
            // ArrayList for data row
            // SimpleAdapter mapping static data to views in xml file
            SimpleAdapter adapter = new SimpleAdapter(this, dados, layout, from, to);

            ListView.Adapter = adapter;
        }

        public void Postar()
        {
            EditText txtId = FindViewById<EditText>(Resource.Id.txtId);
            EditText txtDesc = FindViewById<EditText>(Resource.Id.txtDescricao);
            Models.Restaurante x = new Models.Restaurante
            {
                Id = int.Parse(txtId.Text),
                Descricao = txtDesc.Text,
            };
            string r = "=" + JsonConvert.SerializeObject(x);
            Post(r);
        }

        

        public void TesteGet()
        {
            string aeho = Get2();
            List<Models.Restaurante> list = JsonConvert.DeserializeObject<List<Models.Restaurante>>(aeho);
            //dataGrid.ItemsSource = list;
        }
    }
}


