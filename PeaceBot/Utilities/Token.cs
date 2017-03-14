using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;

namespace PeaceBot.Utilities
{
    public class Token
    {
        public static string GetToken(string tokenKey)
        {
            //Assume failure
            string returnValue = string.Empty;

            //Retrive all of the tokens
            NameValueCollection tokens = ConfigurationManager.GetSection("Tokens") as NameValueCollection;

            //Find and store the token in the returnValue string.
            foreach (string key in tokens.Keys)
            {
                if (tokenKey == key)
                {
                    returnValue = tokens[key];
                }
            }
            return returnValue;
        }
    }
}
