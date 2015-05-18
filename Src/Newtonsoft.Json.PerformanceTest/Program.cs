using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Newtonsoft.Json.PerformanceTest
{
    class Program
    {
        static void Main(string[] args)
        {

            while(!Debugger.IsAttached)
            {
                Thread.Sleep(3000);
            }

            for (int i = 0; i < 55; i++)
            {


                var procFeira = new Thread(() => FazerFeira(i.ToString("00")));
                //procFeira.IsBackground = true;
                procFeira.Start();
                procFeira.Join();
            }


            //var procOFeira = new Thread(ProcessReader);
            //procOFeira.Start();


        }

        private static void ProcessReader()
        {
            DirectoryInfo d = new DirectoryInfo(@"c:\Temp\");
            var files = d.GetFiles("*.json");
        
            foreach (var fi in files)
            {
                Console.WriteLine(fi.Name);
                var procFeira = new Thread(() => ObterFeira(fi));
                procFeira.IsBackground = true;       
                procFeira.Start();
                //Thread.Sleep(500);
                

            }
        }

        private static object _lock = new object();

        private static void FazerFeira(string fid)
        {
           
            lock (_lock)
            {
                Console.WriteLine(fid);
                Feira f = new Feira();
                f.Dia = DateTime.Now;
                var itens = new List<Item>();
                itens.Add(new Banana());
                itens.Add(new Banana());
                itens.Add(new Banana());
                itens.Add(new Banana());
                itens.Add(new Banana());
                itens.Add(new Pera());
                itens.Add(new Pera());
                itens.Add(new Pera());
                itens.Add(new Pera());
                itens.Add(new Pera());
                itens.Add(new Pera());
                f.FrutasOuLegumes = itens;

                using (var fileIO = File.Open(@"C:\Temp\Feiras_" + fid+ ".json", FileMode.OpenOrCreate))
                {
                    var sett = new JsonSerializerSettings();
                        sett.MetadataPropertyHandling = MetadataPropertyHandling.Ignore;
                    sett.PropertyNameHandling = PropertyNameHandling.UsePropertyName;
                    string json = JsonConvert.SerializeObject(f, Formatting.Indented , sett);
                    EventLog.WriteEntry("Newtonsoft.Json.PerformanceTest", String.Format("Serialized Data W => {0} ", json), EventLogEntryType.Warning);
                    byte[] bytes = new byte[json.Length * sizeof(char)];
                    System.Buffer.BlockCopy(json.ToCharArray(), 0, bytes, 0, bytes.Length);
                    
                    fileIO.Write(bytes, 0, bytes.Length);
                }
            }
        }
        

        private static void ObterFeira(FileInfo fi)
        {
            lock (_lock)
            {
                string textJSon = ReadfileContent(fi.FullName);
                Console.WriteLine("Obter "+fi.Name);

                var json = textJSon;
                
                //EventLog.WriteEntry("Newtonsoft.Json.PerformanceTest", String.Format("Serialized Data Reader => {0} ", json), EventLogEntryType.Warning);
                var o = JsonConvert.DeserializeObject(json, typeof(Feira));
                var feira = (Feira)o;

                if (feira.FrutasOuLegumes == null)
                {
                    EventLog.WriteEntry("Newtonsoft.Json.PerformanceTest", "Nulo");
                }

                //EventLog.WriteEntry("Newtonsoft.Json.PerformanceTest", String.Format("{0} em: {1} => {2}", fi.Name, feira.Dia, feira.FrutasOuLegumes.Count), EventLogEntryType.Information);
                feira.FrutasOuLegumes.ForEach(i =>
                {
                    Console.WriteLine(i.Nome);
                    if(String.IsNullOrEmpty(i.Nome))
                        EventLog.WriteEntry("Newtonsoft.Json.PerformanceTest", String.Format("Nome do item nulo {0}", i.Nome), EventLogEntryType.Warning);
                });
            }
        }

        private static string ReadfileContent(string name)
        {
            var textJSon = string.Empty;
            using (var reader = new StreamReader(name, Encoding.Unicode))
            {
                textJSon = reader.ReadToEnd();
            }

            return textJSon;
        }
    }
}