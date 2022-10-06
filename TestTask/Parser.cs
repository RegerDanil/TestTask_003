using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace TestTask
{
    class Parser
    {
        int count = 0;
        bool saveSecondList = false;
        bool isThereFirstCycle = false;

        List<String> currentListLines = new List<string>();

        List<String> firstListLines = new List<string>();
        List<String> secondListLines = new List<string>();

        List<String> successfulСycle = new List<string>();

        public void Parse(string _filePath)
        {
            using (StreamReader reader = new StreamReader(_filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                  {
                    currentListLines.Add(line);
                    count++;
                    if (count == 3)
                    {
                        count = 0;
                        var result = String.IsNullOrWhiteSpace(currentListLines[2]);
                        if (result == false)
                        {
                            CheckLine();
                        }
                        else
                        {
                            currentListLines.Clear();
                        }
                    }
                    }
                    if (line.Contains("03.07.2016"))
                    {
                        currentListLines.Clear();
                        currentListLines.Add(line);
                        count = 1;
                    }
                }
            }
        }

        private void CheckLine()
        {
            string line = currentListLines[2];
            if (line.Contains("Передача"))
            {
                Statistics.numberOfSuccessfulExchangeCycles++;
                Match match = Regex.Match(line, "Передача (.*?) байт.?:");
                Statistics.numberOfBytesPerTransfer += Convert.ToInt32(match.Groups[1].Value);
                SaveSuccessfulСycle();
            }
            else if (line.Contains("Принято"))
            {
                Statistics.numberOfSuccessfulExchangeCycles++;
                Match match2 = Regex.Match(line, @"Принято в Socket->ReceiveBuf\(\) (.*?) байт.?:");
                Statistics.numberOfBytesPerReception += Convert.ToInt32(match2.Groups[1].Value);
                SaveSuccessfulСycle();
            }
            else if (line.Contains("ssIdle"))
            {
                Statistics.numberOfUnsuccessfulExchangesCycles++;
            }
            else if (line.Contains("ssWaitAnswer"))
            {
                SaveFirstList();
            }
            else if (saveSecondList == true)
            {
                SaveSecondList();
                saveSecondList = false;
                CalculateTheTime();
            }
        }

        private void CalculateTheTime()
        {
            string line1 = firstListLines[0];
            string line2 = secondListLines[0];
            DateTime date1 = CreateDate(line1); //раньше
            DateTime date2 = CreateDate(line2);
            TimeSpan interval = date2.Subtract(date1);
            Statistics.downtimeInterval = Statistics.downtimeInterval.Add(interval);
        }
        private DateTime CreateDate(string _line)
        {
            Match match = Regex.Match(_line, @"(?<=03\.07\.2016 )(.*)");
            char[] delimiterChars = { ':', ':', '.' };
            string[] words = match.Groups[1].Value.Split(delimiterChars);
            int h = Convert.ToInt32(words[0]);
            int m = Convert.ToInt32(words[1]);
            int s = Convert.ToInt32(words[2]);
            int ms = Convert.ToInt32(words[3]);
            DateTime date = new DateTime(2016, 7, 3, h, m, s, ms);
            return date;
        }

        private void SaveSecondList()
        {
            secondListLines.Clear();
            secondListLines.AddRange(currentListLines.ToArray());

        }
        private void SaveFirstList()
        {
            firstListLines.Clear();
            firstListLines.AddRange(currentListLines.ToArray());
            saveSecondList = true;
        }

        private void SaveSuccessfulСycle()
        {
            successfulСycle.Clear();
            successfulСycle.AddRange(currentListLines.ToArray());
            while (isThereFirstCycle == false)
            {
                Statistics.timeFirstCycle = GetTimeCycle();
                isThereFirstCycle = true;
            }
            Statistics.timeLastCycle = GetTimeCycle();
        }
        private string GetTimeCycle()
        {
            string line = successfulСycle[0];
            Match match = Regex.Match(line, @"(?<=03\.07\.2016 )(.*)");
            return match.Groups[1].Value;
        }

    }
}
