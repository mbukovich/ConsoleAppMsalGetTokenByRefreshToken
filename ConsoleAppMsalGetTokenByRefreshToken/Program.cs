using System;
using Microsoft.Identity.Client;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace ConsoleAppMsalGetTokenByRefreshToken
{
    class Program
    {
        // MSAL settings
        // Configure MSAL here
        //
        // Note: typically, it is recommended to configure MSAL in a separate configuration file. This particular application uses
        // private static member variables for the sake of simplicity. 
        private static string _clientId = "<paste in your client (application) id>"; // Note: it is important to use the same client id
                                                                                    // that was used to obtain the refresh token
        private static string _authority = "https://login.microsoftonline.com/organizations"; // you can change the 'organizations' part
                                                                                             // to common or your tenant id depending on
                                                                                             // your scenario
        private static string[] _scopes = { "User.Read" }; // add whatever scopes you need
        private static string _redirectUri = "<paste your redirect Uri here>"; // Depending on your scenario, you may or may not need a redirect uri
        private static string _clientSecret = "<paste in your client secret value here>"; // If you are using a public client, you won't need this.
        // Note: the client secret is only necessary if you are using a confidential client. If you needed to use a client secret or
        // certificate to get the refresh token, you will need to do the same here.


        // MSAL client
        private static IPublicClientApplication client;


        static void Main(string[] args)
        {
            // We create the Msal Client. In the particular case in this console app, I didn't need more than the client id and authority.
            // In your application you may need a redirect uri or other parameters.
            client = PublicClientApplicationBuilder.Create(_clientId)
                .WithAuthority(_authority)
                .Build();

            // We cast IByRefreshToken to our Msal Client
            IByRefreshToken clientWithRefresh = client as IByRefreshToken;

            // We get a refresh token to use. You can rewrite the getRefreshToken() method to do this anyway you would like.
            // In his particular console application, we just have the user paste the refresh token into the console.
            string refreshToken = getRefreshToken();

            String accessToken = "";
            AuthenticationResult result = null;

            try
            {
                Task<AuthenticationResult> asyncResults = clientWithRefresh.AcquireTokenByRefreshToken(_scopes, refreshToken).ExecuteAsync();
                result = asyncResults.Result;
                accessToken = result.AccessToken;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            // In this particular console app, we are not doing anything with the access token, just outputting it into the console.
            // However, now the Msal client has our refresh token and AcquireTokenSilent calls will work. 
            Console.WriteLine();
            Console.WriteLine("Obtained Access Token: ");
            Console.WriteLine(accessToken);
        }

        private static string getRefreshToken() 
        { 
            Console.WriteLine("Please paste your refresh Token hear with no quotes or spaces before are after: ");
            return Console.ReadLine();
        }
    }
}