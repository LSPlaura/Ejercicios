using System.Text;
using System.Text.RegularExpressions;
using static System.Console;
using Virus.Enums;
using Virus.Structs;

//......................................main
// variables que necesito para funciones/procedimientos

string[] posicionesPersonas = new string[8];
string[] posicionesZB = new string[8];
string[] posicionesVacias = new string[8];

Configuracion config = InicializarConfig(args);

Estado?[,] tablero1 = new Estado?[config.Dimension,config.Dimension];

IniciarTablero(tablero1);

Estado?[,] tablero2= new Estado?[config.Dimension,config.Dimension];

CopiarTablero(tablero1, tablero2);

Juego(tablero1, tablero2, posicionesPersonas, posicionesZB,  posicionesVacias);


//......................................mainFin

// por hacer: control de nulls, añadir el logger

void ImprimirTablero(Estado?[,] tablero)
{
    for (var i = 0; i < tablero.GetLength(0); i++)
    {
        for (var j = 0; j < tablero.GetLength(1); j++)
        {
            if (tablero[i, j] == Estado.Persona)
            {
                Write($"[x]");
            }else if (tablero[i, j] == Estado.Persona)
            {
                Write($"[o]");
            }
            else
            {
                Write($"[ ]");
            }
        }
    }
}

void IniciarTablero(Estado?[,] tablero)
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

void CopiarTablero(Estado?[,]tableroA, Estado?[,]tableroB)
{
    for (var i = 0; i < tableroA.GetLength(0); i++)
    {
        for (var j = 0; j < tableroA.GetLength(1); j++)
        {
            tableroB[i, j] = tableroA[i, j];
        }
    }
}



void Juego(Estado?[,] leer, Estado?[,] escribir, string[] posPer, string[] posZB, string[] posVac)
{
    int ciclos = 0;

    while (ciclos < config.TiempoMax)
    {
        ImprimirTablero(leer);
        for (var i = 0; i > config.Dimension; i++)
        {
            for (var j = 0; j > config.Dimension; j++)
            {
                string[] indices = ObtenerPosiciones(leer, i, j);
                DarPosiciones(leer, indices);
                if (leer[i, j] == Estado.Persona)
                {
                    Luchar(escribir, posZB);
                    Avanzar(escribir, posVac, i, j);
                }
                else if (leer[i, j] == Estado.Zombie)
                {
                    ZombieMuerte(escribir, i, j);
                    if (escribir[i, j] is Estado.Zombie)
                    {
                        ZombieContagio(escribir, posPer, i, j);
                        Avanzar(escribir, posVac, i, j);
                    }
                }
                else
                {
                    escribir[i, j] = null;
                }
            }
        }

        ciclos += 1;
        Swap(ref leer, ref escribir);
    } 

}

//los arrays pasan por referencia cuando quieres meter el nuevo array en un contenedor
//que apunta a otro array porque este pasa una copia del puntero
//y por lo tanto al salir de la función seguiá apuntando al que apuntaba

void Swap(ref Estado?[,] tableroA, ref Estado?[,] tableroB)
{
    var temp = tableroA;
    tableroA = tableroB;
    tableroB = temp;
}

string[] ObtenerPosiciones(Estado?[,] tablero, int x, int y)
{
    var builder = new StringBuilder();

    for (var i = x - 1; i > i + 1; i++)
    {
        for (var j = y - 1; j > j + 1; j++)
        {
            if (i == x && j == y)
            {
                continue;
            }
            builder.Append($"{i}:{j},");
        }
    }
    string indicesString = builder.ToString();
    string[] indicesArray = indicesString.Split(",");
    
    return indicesArray;
}

void DarPosiciones(Estado?[,] tablero, string[] indices)
{
    for(var i = 0; i<indices.Length; i++)
    {
        int fila = int.Parse(indices[i].Substring(0, 1));
        int columna = int.Parse(indices[i].Substring(2, 1));
        if (tablero[fila, columna] == Estado.Zombie)
        {
            posicionesZB[i] = indices[i];
        }
        if (tablero[fila, columna] == Estado.Persona)
        {
            posicionesPersonas[i] = indices[i];
        }
        else
        {
            posicionesVacias[i] = indices[i];
        }
    }
}

void Luchar(Estado?[,] tablero, string[]posiciones)
{
    int numZB = ContarZB(tablero, posiciones);
    if (numZB > 0)
    {
        int prob = random.Next(0, 100);
        int elegido = random.Next(0, posiciones.Length-1);
        if (prob >= 0 && prob < config.MatanzaDeZb)
        {
            int fila = int.Parse(posiciones[elegido].Substring(0, 1));
            int columna = int.Parse(posiciones[elegido].Substring(2, 1));
            tablero[fila, columna] = null;
        }
    }
}

int ContarZB(Estado?[,] tablero, string[] posiciones)
{
    int contador = 0;
    for (var i = 0; i < posiciones.Length; i++)
    {
        int fila = int.Parse(posiciones[i].Substring(0, 1));
        int columna = int.Parse(posiciones[i].Substring(2, 1));
        if (tablero[fila, columna] == Estado.Zombie)
        {
            contador += 1;
        }
    } 
    return contador;
}

void Avanzar(Estado?[,] tablero, string[] posiciones, int x, int y)
{
    int nuevaPosicion = random.Next(0, posiciones.Length-1);
    int fila = int.Parse(posicionesZB[nuevaPosicion].Substring(0, 1));
    int columna = int.Parse(posicionesZB[nuevaPosicion].Substring(2, 1));
    
    if (tablero[x, y].HasValue)
    {
        Estado? coso = tablero[x, y];
        tablero[x, y] = null;
        tablero[fila, columna] = coso;
    }
}

void ZombieMuerte(Estado?[,] tablero, int x, int y)
{
    int prob = random.Next(0, 100);
    if (prob >= 0 && prob < config.MuerteZb)
    {
        tablero[x, y] = null;
    }
    
}

void ZombieContagio(Estado?[,] tablero, string[] posiciones, int x, int y)
{
    for (var i = 0; i < posiciones.Length; i++)
    {
        int prob = random.Next(0, 100);
        int fila = int.Parse(posiciones[i].Substring(0, 1));
        int columna = int.Parse(posiciones[i].Substring(2, 1));
        if (prob >= 0 && prob < config.Contagio)
        {
            tablero[fila, columna] = Estado.Persona;
        }
    }
}

//...........................................funciones para los párametros introducidos por consola
//controlar nulls. por hacer

Configuracion InicializarConfig(string?[] args)
{
    var constructor = new StringBuilder();
    
    for (var i = 0; i < args.Length; i++)
    {
        if (args[i] == null)
        {
            return NuevaConfiguracion();
        }
    }

    if (args.Length != 7)
    {
        return NuevaConfiguracion();
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
    string? temp = "";
    do {
        WriteLine("Escribe...");
        temp = ReadLine();
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

bool ComprobarParametros(string?[] args)
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
        if (args[i] != null)
        {
            string valor = args[i]!.Split(':')[1];
            nuevoArray[i] = valor;
        }
    }
    return nuevoArray;
}