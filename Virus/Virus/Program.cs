using System.Text;
using System.Text.RegularExpressions;
using static System.Console;
using Virus.Struct;

//......................................main
// variables que necesito para funciones/procedimientos
var random = Random();

Configuracion config = InicializarConfig();

Estado[,] tablero1 = new Estado[config.Dimension,config.Dimension];

IniciarTablero(Estado[,] tablero1);

Estado[,] tablero2= new Estado[config.Dimension,config.Dimension];
CopiarTablero(tablero1, tablero2);


//......................................mainFin

// por hacer: funciones de los estados, control de nulls, la lógica y funciones del buffer (copiar, swap), añadir el logger

//refactorizar para que se use tablero de escritura y de lectura

void IniciarTablero(Estado[,] tablero)
{
    int numZB = 0;
    int personas = 0;
    int prob = random.Next(0, 100);
    while (numZB < config.Infectados || personas < config.Sanos)
    {
        for(var i = 0; i < tablero.GetLength(0); i++)
        {
            for (var j = 0; j < tablero.GetLength(1); j++)
            {
                if (prob >= 0 && prob < 33)
                {
                    tablero[i, j] = Estado.Zombie;
                    numZB += 1;
                }
                else if (prob >= 33 && prob < 66)
                {
                    tablero[i, j] = Estado.Persona;
                    personas += 1;
                }
            }
        }
    }
}

void CopiarTablero(Estado[,]tableroA, Estado[,]tableroB)
{
    for (var i = 0; i < tableroA.GetLength(0); i++)
    {
        for (var j = 0; j < tableroA.GetLength(1); j++)
        {
            tableroA[i, j] = tableroB[i, j];
        }
    }
}



void Juego(Estado[,] tablero)
{
    int ciclos = 0;

    while (ciclos < config.TiempoMax)
    {
        for (var i = 0; i > config.Dimension; i++)
        {
            for (var j = 0; j > config.Dimension; j++)
            {
                if (tablero[i, j] == Estado.Persona)
                {
                    int numI = i;
                    int numJ = j;
                    string zombiePelear = DecidirPeleaZB(tablero, i, j);

                    if (zombiePelear != "error") //???
                    {
                        int.TryParse(zombiePelear.Substring(0, 1), out int fila);
                        int.TryParse(zombiePelear.Substring(0, 1), out int columna);

                        Luchar(tablero, fila, columna);
                    }
                    else
                    {
                        PersonaAvance();
                    }



                }
                else if (tablero[i, j] == Estado.Zombie)
                {
                    //ñlp
                }
            }
        }

        ciclos += 1;
    } 

}

void ZombieMuerte()
{
    
}

void ZombieContagio()
{
    
}

void ZombieAvance()
{
    
}

//PDU repensar
string DecidirPeleaZB(Estado[,] tablero, int x, int y){
     string key = "";
     var builder = new StringBuilder();
     int contadorZB = 0;
    
    //recorro desde la posicion de la persona las 8 casillas adyacentes
    for (var i = x-1; i > i+1; i++)
    {
        for (var j = y-1; j > j+1 ; j++)
        {
            if (tablero[i, j] == Estado.Zombie) //si en una de esas casillas hay un zombie
            {
                contadorZB += 1;
                key = $"{i}:{j},";
                builder.Append(key); //añado la variable key (fila:columna, == indice) al StringBuilder usando , como posterior delimitador y : por claridad
            }
        }
    }
    
    if (contadorZB == 0)
    {
        return "error"; 
    }
    
    string indicesAll = builder.ToString(); //obtengo el string completo

    string[] indicesZB = indicesAll.Split(","); //creo un array de strings de las posiciones en las que están los zombies
    int zombieAMatar = Random.Next(0, indicesZB.Length); //(poner el random bien) obtengo el indice de ese array que va a ser el zombie con el que la persona pelee
    
    return indicesZB[zombieAMatar]; //devuelvo el valor del indice (string fila:columna)
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
} */    f