﻿using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CloudServices.Interfaces;

namespace AsyncAwait.Task2.CodeReviewChallenge.Models.Support
{
    public class ManualAssistant : IAssistant
    {
        private readonly ISupportService _supportService;

        public ManualAssistant(ISupportService supportService)
        {
            _supportService = supportService ?? throw new ArgumentNullException(nameof(supportService));
        }

        public async Task<string> RequestAssistanceAsync(string requestInfo)
        {
            try
            {
                //todo: use async await instead of using thread.sleep
                Task t = _supportService.RegisterSupportRequestAsync(requestInfo);
                Console.WriteLine(t.Status); // this is for debugging purposes
                Thread.Sleep(5000); // this is just to be sure that the request is registered
                return await _supportService.GetSupportInfoAsync(requestInfo)
                    .ConfigureAwait(false);
            }
            catch (HttpRequestException ex)
            {
                // todo: not use async await inside catch block
                return await Task.Run(async () => await Task.FromResult($"Failed to register assistance request. Please try later. {ex.Message}"));
            }
        }
    }
}
