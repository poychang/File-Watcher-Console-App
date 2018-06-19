using System;
using System.IO;
using System.Threading;

namespace ContinueCreateFileApp
{
    public class Program
    {
        static void Main(string[] args)
        {
            const string fileFolder = @"C:\TestFileWatcher";
            var filePah = $@"{fileFolder}\Output.csv";
            Console.WriteLine("Wait for 5 second to start.");
            Console.WriteLine("Continuous create 1000 line of random records, each record will wait almost 1 second to created.");
            Console.WriteLine($"Target File: {filePah}");

            CreateFile(filePah);
            //AppendTextSimulator(filePah);
            AppendTextFromSampleFileSimulator($@"{fileFolder}\sample.csv", filePah);

            Console.ReadLine();
        }

        private static void CreateFile(string filePah)
        {
            try
            {
                if (File.Exists(filePah))
                {
                    File.Delete(filePah);
                }

                using (var fs = File.Create(filePah))
                {

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static void AppendTextSimulator(string filePah)
        {
            Thread.Sleep(5000);

            var rand = new Random();
            var date = DateTime.Now;
            for (var i = 0; i < 1000; i++)
            {
                Thread.Sleep(rand.Next(1000, 1200));
                using (var sw = File.AppendText(filePah))
                {
                    sw.WriteLine($@"{i.ToString()}	{date.ToShortDateString()}	{DateTime.Now.TimeOfDay}	{rand.Next(90, 100)}	{rand.Next(90, 100)}	{rand.Next(90, 100)}	{rand.Next(90, 100)}	124.8	125.0	124.8	125.0	154.9	155.0	155.0	155.0	175.1	175.0	174.9	175.0	180.1	180.0	179.9	180.0	185.1	185.0	185.0	185.0	190.0	190.0	190.0	190.0	200.0	200.0	200.1	200.0	230.1	230.0	230.1	230.0	250.2	250.0	250.1	250.0	260.2	260.0	260.0	260.0	259.7	260.0	259.9	260.0	32.9	23.0	30	30	30	30	40	40	45	45	45	45	40	40	85.0	85.0	172.8	172.7	C:\WINCON\4928S-105HF-150NZ12.RCP	2	3	0	5	0	0	415	0	341818	0	1	0");
                }
            }
        }

        private static void AppendTextFromSampleFileSimulator(string inputFilePah, string outputFilePath)
        {
            Thread.Sleep(5000);

            foreach (var line in File.ReadAllLines(inputFilePah))
            {
                Thread.Sleep(new Random().Next(1000, 1200));
                using (var sw = File.AppendText(outputFilePath))
                {
                    sw.WriteLine($"{line}");
                }
            }
        }
    }
}
