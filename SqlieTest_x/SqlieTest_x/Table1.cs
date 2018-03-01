using System;
using System.Collections.Generic;
using System.Text;

namespace SqlieTest_x
{
    public class Table1
    {
        public int IdUsuario { get; set; }
        public string Longitud { get; set; }
        public string Latitud { get; set; }
        public DateTime FechaAlta { get; set; }
    }

    public class Tablas
    {
        public List<Table1> Table1 { get; set; }
    }

    public class Root
    {
        public object DatosEnvio { get; set; }
        public object DatosEnvioJson { get; set; }
        public object DatosEnvioJsonDatos { get; set; }
        public object DatosEnvioJsonTitulos { get; set; }
        public object tabla { get; set; }
        public Tablas tablas { get; set; }
        public string bandera { get; set; }
        public string mensaje { get; set; }
    }
}
