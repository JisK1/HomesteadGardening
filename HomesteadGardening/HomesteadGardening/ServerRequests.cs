using System;
using System.Xml.Linq;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;


namespace HomesteadGardening
{
    static class ServerRequests
    {
        //for these variables you need to manually create a class that is ignored by github and add theses variables.
        //The ip to connect to the web_server.
        private const string ip = IgnoredVariables.ip;
        private const int timeout = IgnoredVariables.timeout;
        
        
        /// <summary>
        /// Attempts to authenticate the user. Returns true if the user 
        /// is authenticated with the server, false upon error or rejection.
        /// </summary>
        /// <param name="password">Password to include in the query.</param>
        /// <param name="userName">Username to include in the query.</param>
        /// <returns></returns>
        public static bool Authenticate(string password, string userName) {
            //The URL that will be posted to the server.
            string message = "https://" + ip + "/driver/auth/authenticate.php?username=" + userName + "&pwHsh=" + GetHash(password, userName);
            string responseFromServer = "";
            try {
                // Create a request for the URL. 		
                WebRequest request = WebRequest.Create(message);
                request.Timeout = timeout;
                // Get the response.
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                // Get the stream containing content returned by the server.
                Stream dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                responseFromServer = reader.ReadToEnd();
                // Cleanup the streams and the response.
                reader.Close();
                dataStream.Close();
                response.Close();
                dynamic jObject = null;

                //check if the response 
                if (!responseFromServer.Contains("Failure")) {

                    //What to do if the response comes back correctly.                

                    return true;
                } else {
                    return false;
                } 
            } catch (WebException) {

                return false;
            } catch (Exception e) {

                //output exeption

                return false;
            }
        }

        public static String SendData(string Name, string text) {
            //The URL that will be posted to the server.
            string message = "http://" + ip + "/appTest.php?name=" + Name + "&text=" + text;
            string responseFromServer = "";
            try {
                // Create a request for the URL. 		
                WebRequest request = WebRequest.Create(message);
                request.Timeout = timeout;
                // Get the response.
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                // Get the stream containing content returned by the server.
                Stream dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                responseFromServer = reader.ReadToEnd();
                // Cleanup the streams and the response.
                reader.Close();
                dataStream.Close();
                response.Close();

                //check if the response 
                if (!responseFromServer.Contains("Failure")) {

                    //What to do if the response comes back correctly.                

                    return responseFromServer;
                } else {
                    return responseFromServer;
                }
            } catch (WebException) {
                Console.WriteLine("--------------");
                Console.WriteLine(message);
                Console.WriteLine("--------------");
                return "webExeption";
            } catch (Exception e) {

                //output exeption

                return "Exception";
            }
        }


        //returns a string of the hashed password.
        private static string GetHash(string input, string userName) {
            using (SHA256 sha256Hash = SHA256.Create()) {
                HashAlgorithm hashAlgorithm = sha256Hash;
                // Convert the input string to a byte array and compute the hash.
                byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(GetSalt(userName) + input));

                // Create a new stringbuilder to collect the bytes
                // and create a string.
                var sBuilder = new StringBuilder();

                // Loop through each byte of the hashed data 
                // and format each one as a hexadecimal string.
                for (int i = 0; i < data.Length; i++) {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                // Return the hexadecimal string.
                return sBuilder.ToString();
            }
        }

        /// <summary>
        /// Gets the salt for a user in order to hash their password.
        /// </summary>
        /// <param name="userName">Username from which to get the salt.</param>
        /// <returns>string containing the salt.</returns>
        public static string GetSalt(string userName) {
            string message = "https://" + ip + "/driver/auth/getSalt.php?username=" + userName;
            string responseFromServer = "";
            try {
                // Create a request for the URL. 		
                WebRequest request = WebRequest.Create(message);
                request.Timeout = timeout;
                // Get the response.
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                // Display the status.
                // Get the stream containing content returned by the server.
                Stream dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                responseFromServer = reader.ReadToEnd();
                // Display the content.
                // Cleanup the streams and the response.
                reader.Close();
                dataStream.Close();
                response.Close();

                dynamic jObject = null;


                if (!responseFromServer.Contains("Failure")) {
                    return "not Fail";
                } else {
                    return "error";
                }
            } catch (WebException) {
               

                return "No Internet connection.";
            } catch (Exception e) {
                    //output exeption
                    return "Error Connecting to Server";
            }
        }

    }
}
