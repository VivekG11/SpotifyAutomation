using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System;
using RestSharp;
using Newtonsoft.Json;
using System.Collections.Generic;
using SpotifyAutomation;

namespace ValidateGetRequest
{
    [TestClass]
    public class ValidateSpotifyRequests
    {
        public string token;
        IRestClient client = new RestClient();
        IRestRequest request = new RestRequest();
        public static IRestResponse response;
        public string user_id;
        public string UserName;

        public static string myToken = "Bearer BQCw6jBg3Hdm7MBW0IJ8dQHmfIYY2BYOr47lR20kKF_2lZX_h0Kp8tmI5Bdj40BdojpdLfVKbOy8BKFasdQT-wkmYBIitPR7EFAsHgM5bOyKOtLCj0YzRnxKXQKhJxfxrZtmonvonB6RBSchF1GJKgA5zT8NM8kLh-wCPvZ594lOn80t6eDenzw2-nY1NkjJQ8_pZ3tcNaZAGydr2CVkBxyabNiUdUWIjETjtoGbnQ";

        [TestInitialize]
        public void setup()
        {
            token = myToken;
        }


        [TestMethod]
        public void ReturnUserDetails()
        {
            
            string getUrl = "https://api.spotify.com/v1/me";
            request = new RestRequest(getUrl);
            request.AddHeader("Authorization", "Token" + token);



            response = client.Get(request);
            IRestResponse<List<Details>> restResponse = client.Get<List<Details>>(request);
            var info = restResponse.Data;
            foreach(var i in info)
            {
               user_id = i.Id;
               UserName = i.Name;
            }


            //Assert statement comparing actual and expected value
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Console.WriteLine(response.Content);
            Console.WriteLine("User Id is :"+user_id+" and userName is"+UserName);


        }

        [TestMethod]
        public void CreatePlaylist()
        {
            //Url to create a playlist 
            string postUrl = "https://api.spotify.com/v1/users/" + user_id + "/playlists";
            string jsonData = "{\"name\": \"Vivek's Third Playlist\"," +
                                    "\"description\": \"Latest Songs\"," +
                                    "\"public\" : \"false\"}";


            request = new RestRequest(postUrl);
            request.AddHeader("Authorization", "Token" + token);
            request.AddJsonBody(jsonData);


            response = client.Post(request);


            // IRestResponse<List<Details>> responseData = client.Get<List<Details>>(request);
            Assert.AreEqual(201, (int)response.StatusCode);

            var res = JsonConvert.DeserializeObject<dynamic>(response.Content);
            if (response.IsSuccessful)
            {
                Console.WriteLine("PlaylistId :" + res.id);

                Console.WriteLine(response.Content);
            }
            else
            {
                System.Console.WriteLine(response.ErrorException);
                System.Console.WriteLine(response.ErrorMessage);
            }
        }

        [TestMethod]
        public void AddItemtoPlaylist()
        {
            string playlistId = "6kTgoV8sWmts33CB7VcBtK";
            string uri = "spotify:track:7uNnlVit5qDvfOje0pqICF";
            string postUrl = "https://api.spotify.com/v1/playlists/"+playlistId+"/tracks?uris="+uri;
      //  https://api.spotify.com/v1/playlists/2surb9o4zDYqsICbov4m8l/tracks?uris=spotify%3Atrack%3A5sbooPcNgIE22DwO0VNGUJ

            request = new RestRequest(postUrl);
            request.AddHeader("Authorization", "Token" + token);
            request.AddParameter("uris", uri);


            response = client.Post(request);
            if (response.IsSuccessful)
            {
               
                Console.WriteLine(response.Content);
            }
            else
            {
                System.Console.WriteLine(response.ErrorException);
                System.Console.WriteLine(response.ErrorMessage);
            }
        }

        [TestMethod]
        public void ReturnTrackBasedOnSearch()
        {
           
            string url = @"https://api.spotify.com/v1/search?q=Rock&type=track";

            request = new RestRequest(url);
            request.AddHeader("Authorization", "Token" + token);
           request.AddParameter("q", "Rock");
           request.AddParameter("type", "track");

            response = client.Get(request);

            if (response.IsSuccessful)
            {

                Console.WriteLine(response.Content);
            }
            else
            {
                System.Console.WriteLine(response.ErrorException);
                System.Console.WriteLine(response.ErrorMessage);
            }
        }

        [TestMethod]
        public void ReturnPlaylistItems()
        {
            string id = "2surb9o4zDYqsICbov4m8l";
            string url = @"https://api.spotify.com/v1/playlists/"+id+"/tracks";

            request = new RestRequest(url);
            request.AddHeader("Authorization", "Token" + token);

            //Exectuing the request
            response = client.Get(request);

            Assert.AreEqual(200, (int)response.StatusCode);

            if (response.IsSuccessful)
            {

                Console.WriteLine(response.Content);
                Console.WriteLine("Total number of songs are :"+response.Content);
            }

            else
            {
                System.Console.WriteLine(response.ErrorException);
                System.Console.WriteLine(response.ErrorMessage);
            }
        }

    }
}
