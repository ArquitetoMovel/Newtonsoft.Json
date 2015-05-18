using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Newtonsoft.Json.PerformanceTest
{
    [Serializable]
    public class Item : ABSVerdura, IFruta
    {
        private string _nome;

        public Item() { }

        public Item(string nome)
        {
            _nome = nome;
        }

        [Newtonsoft.Json.JsonProperty("ItemOf")]
        public string Nome { get { return _nome; } set { _nome = value; } }

    }
}
