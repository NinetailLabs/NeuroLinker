using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroLinker.Factories;
using NeuroLinker.Helpers;

namespace NeuroLinker.Runner
{
    internal class Program
    {
        #region Private Methods

        private static async Task Main(string[] args)
        {
            var httpClientFactory = new HttpClientFactory();
            var pageRetriever = new PageRetriever(httpClientFactory);
            var requestProcessor = new NeuroLinker.Workers.RequestProcessor(pageRetriever);

            var character = await requestProcessor.DoCharacterRetrieval(36828);

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        #endregion
    }
}