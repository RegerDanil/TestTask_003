using System;

namespace TestTask
{
    class Program
    {
        static void Main(string[] args)
        {
            do
            {
                try
                {
                    Console.WriteLine("Введите полный путь к файлу(Например: D:\\ServerSocket1.txt): ");
                    string pathFile = Console.ReadLine();
                    Parser parser = new Parser();
                    parser.Parse(pathFile);
                    Console.WriteLine($"УспОбмн -{Statistics.numberOfSuccessfulExchangeCycles}" +
                        $"\nНеУспОбм - {Statistics.numberOfUnsuccessfulExchangesCycles}" +
                        $"\nПередано - {Statistics.numberOfBytesPerTransfer}" +
                        $"\nПринято {Statistics.numberOfBytesPerReception}" +
                        $"\nИнтервалПростоя {Statistics.downtimeInterval.ToString()}" +
                        $"\nВремяПервого {Statistics.timeFirstCycle}" +
                        $"\nВремяПосленего {Statistics.timeLastCycle}");
                }
                catch
                {
                    Console.WriteLine("Неверно введен путь!");
                }
            } while (true);
        }
    }
}
