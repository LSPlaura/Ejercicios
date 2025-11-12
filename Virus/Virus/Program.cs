using System.Text;
using System.Text.RegularExpressions;
using static System.Console;
using Virus.Struct;

//......................................main

Configuracion config = InicializarConfig();

//......................................mainFin

void Juego(Estado[,] tablero){
    //revisar si esto no seria manejado por los ciclos
    for (var i = 0; i > config.Dimension; i++)
    {
        for (var j = 0; j > config.Dimension; j++)
        {
            if (tablero[i, j] == Estado.Persona) // cannot access non-static field in static context?
            {
                int numI = i;
                int numJ = j;
                string zombiePelear = DecidirPeleaZB(tablero, i, j);

                if (zombiePelear != "error")
                {
                    int.TryParse(zombiePelear.Substring(0, 1), out int fila);
                    int.TryParse(zombiePelear.Substring(0, 1), out int columna);
                
                    Luchar(tablero, fila, columna);
                }
                
                NuevaPosicion();
                

                ComprobarPosicion();
                NuevaPosicion();


            }else if (tablero[i, j] == Estado.Zombie)
            {
                //ñlp
            }
        }
    }

}

string DecidirPeleaZB(Estado[,] tablero, int x, int y){
     string key = "";
     var builder = new StringBuilder();
    
    // hacer bien el recorrido
    for (var i = x-1; i > i+1; i++)
    {
        for (var j = y-1; j > j+1 ; j++)
        {
            if (tablero[i, j] == Estado.Zombie)
            {
                key = $"{i}:{j},";
                builder.Append(key);
            }
            else
            {
                return "error";
            }
        }
    }
    
    string indicesAll = builder.ToString();

    string[] indicesZB = indicesAll.Split(",");
    int zombieAMatar = Random.Next(0, indicesZB.Length); //poner el random. y ponerlo bien
    
    return indicesZB[zombieAMatar];
}

//...........................................funciones para los párametros introducidos por consola

//controlar nulls. por hacer
Configuracion InicializarConfig(string?[] args)
{
    var constructor = new StringBuilder();

    if (args.Length != 7)
    {
        return NuevaConfiguracion();
    }


    for (var i = 0; i < args.Length; i++)
    {
        if (args[i] == null)
        {
            return NuevaConfiguracion();
        }
    }

    var isValid = ComprobarParametros(args);

    if (!isValid)
    {
        return NuevaConfiguracion();
    }

    var nuevosParametros = ObtenerParametros(args);

    int nuevaDim = int.Parse(nuevosParametros[0]);
    int nuevosInfect = int.Parse(nuevosParametros[1]);
    int nuevosSanos = int.Parse(nuevosParametros[2]);
    int nuevoContagio = int.Parse(nuevosParametros[3]);
    int nuevoTiempo = int.Parse(nuevosParametros[4]);
    int nuevaMuerte = int.Parse(nuevosParametros[5]);
    int nuevaMatanza = int.Parse(nuevosParametros[6]);

    return new Configuracion
    {
        Dimension = nuevaDim,
        Infectados = nuevosInfect,
        Sanos = nuevosSanos,
        Contagio = nuevoContagio,
        TiempoMax = nuevoTiempo,
        MuerteZb = nuevaMuerte,
        MatanzaDeZb = nuevaMatanza,
    };
}

Configuracion NuevaConfiguracion()
{
    bool isValid;
    string temp = "";
    do {
        WriteLine("Escribe...");
        temp = Console.ReadLine();
        var regex = new Regex(@"^(dimension:([0-9]{1,2}|100)\s)(infectados:([0-9]{1,2}|100)\s)(sanos:([0-9]{1,2}|100)\s)(contagio:([0-9]{1,2}|100)\s)(tiempo:([0-9]{1,2}|100)\s)(muerteZB:([0-9]{1,2}|100)\s)(matanzaZB:([0-9]{1,2}|100))$");
        isValid = regex.IsMatch(temp);
    } while(!isValid);



    string[] array1 = temp.Split(' ');

    var nuevosParametros = ObtenerParametros(array1);

    int nuevaDim = int.Parse(nuevosParametros[0]);
    int nuevosInfect = int.Parse(nuevosParametros[1]);
    int nuevosSanos = int.Parse(nuevosParametros[2]);
    int nuevoContagio = int.Parse(nuevosParametros[3]);
    int nuevoTiempo = int.Parse(nuevosParametros[4]);
    int nuevaMuerte = int.Parse(nuevosParametros[5]);
    int nuevaMatanza = int.Parse(nuevosParametros[6]);

    return new Configuracion
    {
        Dimension = nuevaDim,
        Infectados = nuevosInfect,
        Sanos = nuevosSanos,
        Contagio = nuevoContagio,
        TiempoMax = nuevoTiempo,
        MuerteZb = nuevaMuerte,
        MatanzaDeZb = nuevaMatanza,
    };

}

bool ComprobarParametros(string[] args)
{
    // añado todos los strings a un string para luego poder usar regex
    var argsEnString = string.Join(" ", args);

    var regex = new Regex(
        @"^dimension:([0-9]|100)\s,infectados:([0-9]{1,2}|100)\s,sanos:([0-9]{1,2}|100)\s,tiempo:([0-9]{1,2}|100)\s,muerteZB:([0-9]{1,2}|100)\s,matanzaZB:([0-9]{1,2}|100)$");

    return regex.IsMatch(argsEnString);
}

string[] ObtenerParametros(string?[] args)
{
    
    string[] nuevoArray = new string[args.Length];

    for (var i = 0; i < args.Length; i++)
    {
        string valor = args[i].Split(':')[1];
        nuevoArray[i] = valor;
    }
    return nuevoArray;
} */    