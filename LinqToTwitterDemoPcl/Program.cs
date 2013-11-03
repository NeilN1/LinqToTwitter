﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LinqToTwitter;

namespace LinqToTwitterDemoPcl
{
    class Program
    {
        static void Main()
        {
            Task verifyTask = TestLinqToTwitterAsync();
            verifyTask.Wait();

            Console.Write("\nPress any key to continue...");
            Console.ReadKey();
        }

        async static Task TestLinqToTwitterAsync()
        {
            var auth = ChooseAuthenticationStrategy();

            await auth.AuthorizeAsync();

            var ctx = new TwitterContext(auth);

            //var tweets = await
            //    (from tweet in ctx.Status
            //     where tweet.Type == StatusType.Home &&
            //           tweet.Count == 5
            //     select tweet)
            //    .ToListAsync();

            //tweets.ForEach(tweet =>
            //    Console.WriteLine("\nName:\n{0}\nTweet:{1}\n", tweet.ScreenName, tweet.Text));

            //var searchResponse = await
            //    (from search in ctx.Search
            //     where search.Type == SearchType.Search &&
            //           search.Query == "LINQ to Twitter"
            //     select search)
            //    .SingleAsync();

            //searchResponse.Statuses.ForEach(tweet =>
            //    Console.WriteLine("\nName:\n{0}\nTweet:{1}\n", tweet.ScreenName, tweet.Text));

            //string statusText = "Testing TweetAsync in LINQ to Twitter - " + DateTime.Now;

            //Status newTweet = await ctx.TweetAsync(statusText);

            //byte[] imageBytes = File.ReadAllBytes(@"..\..\Images\200xColor_2.png");

            //Status newTweet = await ctx.TweetWithMediaAsync(statusText, false, imageBytes);

            //Console.WriteLine("\nName:\n{0}\nTweet:{1}\n", newTweet.ScreenName, newTweet.Text);

            //User user = await ctx.UpdateAccountImageAsync(imageBytes, "200xColor_2.png", "png", false);

            //Console.WriteLine("\nName:\n{0}\nImage URL:{1}\n", user.ScreenNameResponse, user.ProfileBackgroundImageUrl);

            //Console.WriteLine("\nStreamed Content: \n");
            //int count = 0;

            //await
            //    (from strm in ctx.Streaming
            //     where strm.Type == StreamingType.Filter &&
            //         //strm.Follow == "15411837"
            //         //strm.Language == "fr,jp,en" &&
            //           strm.Track == "twitter"//,JoeMayo,linq2twitter,microsoft,google,facebook"
            //     select strm)
            //    .StartAsync(async strm =>
            //    {
            //        if (strm.Status != TwitterErrorStatus.Success)
            //        {
            //            Console.WriteLine(strm.Error.ToString());
            //            return;
            //        }

            //        Console.WriteLine(strm.Content + "\n");

            //        if (count++ >= 25)
            //        {
            //            strm.CloseStream();
            //        }
            //    });

            //await
            //    (from strm in ctx.Streaming
            //     where strm.Type == StreamingType.Sample
            //     select strm)
            //    .StartAsync(async strm =>
            //    {
            //        if (strm.Status == TwitterErrorStatus.RequestProcessingException)
            //        {
            //            Console.WriteLine(strm.Error.ToString());
            //            return;
            //        }

            //        Console.WriteLine(strm.Content + "\n");

            //        if (count++ >= 10)
            //        {
            //            strm.CloseStream();
            //        }
            //    });

            //StreamContent strmContent = null;
            //await
            //    (from strm in ctx.Streaming
            //     where strm.Type == StreamingType.User
            //     select strm)
            //    .StartAsync(async strm =>
            //     {
            //         if (strmContent == null) strmContent = strm;

            //         if (strm.Status == TwitterErrorStatus.RequestProcessingException)
            //         {
            //             //WebException wex = strm.Error as WebException;
            //             //if (wex != null && wex.Status == WebExceptionStatus.ConnectFailure)
            //             //{
            //             //    Console.WriteLine(wex.Message + " You might want to reconnect.");
            //             //}

            //             Console.WriteLine(strm.Error.ToString());
            //             return;
            //         }

            //         string message = string.IsNullOrEmpty(strm.Content) ? "Keep-Alive" : strm.Content;
            //         Console.WriteLine((count + 1).ToString() + ". " + DateTime.Now + ": " + message + "\n");

            //         if (count++ == 10)
            //         {
            //             Console.WriteLine("Demo is ending. Closing stream...");
            //             strm.CloseStream();
            //         }
            //     });
        }
  
        static IAuthorizer ChooseAuthenticationStrategy()
        {
            Console.WriteLine("Authentication Strategy:\n\n");

            Console.WriteLine("  1 - Pin (default)");
            Console.WriteLine("  2 - Application-Only");
            Console.WriteLine("  3 - Single User");
            Console.WriteLine("  4 - XAuth");

            Console.Write("\nPlease choose (1, 2, 3, or 4): ");
            ConsoleKeyInfo input = Console.ReadKey();
            Console.WriteLine("");

            IAuthorizer auth = null;

            switch (input.Key)
            {

                case ConsoleKey.D1:
                    auth = DoPinOAuth();
                    break;
                case ConsoleKey.D2:
                    auth = DoApplicationOnly();
                    break;
                //case ConsoleKey.D3:
                //    auth = DoSingleUserAuth();
                //    break;
                //case ConsoleKey.D4:
                //    auth = DoXAuth();
                //    break;
                default:
                    auth = DoPinOAuth();
                    break;
            }

            return auth;
        }
  
        static IAuthorizer DoPinOAuth()
        {
            var auth = new PinAuthorizer()
            {
                CredentialStore = new InMemoryCredentialStore
                {
                    ConsumerKey = ConfigurationManager.AppSettings["twitterConsumerKey"],
                    ConsumerSecret = ConfigurationManager.AppSettings["twitterConsumerSecret"]
                },
                GoToTwitterAuthorization = pageLink => Process.Start(pageLink),
                GetPin = () =>
                {
                    // this executes after user authorizes, which begins with the call to auth.Authorize() below.
                    Console.WriteLine("\nAfter authorizing this application, Twitter will give you a 7-digit PIN Number.\n");
                    Console.Write("Enter the PIN number here: ");
                    return Console.ReadLine();
                }
            };
            return auth;
        }

        static IAuthorizer DoApplicationOnly()
        {
            var auth = new ApplicationOnlyAuthorizer()
            {
                CredentialStore = new InMemoryCredentialStore
                {
                    ConsumerKey = ConfigurationManager.AppSettings["twitterConsumerKey"],
                    ConsumerSecret = ConfigurationManager.AppSettings["twitterConsumerSecret"]
                },
            };
            return auth;
        }
    }
}