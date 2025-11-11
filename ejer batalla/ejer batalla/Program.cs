using System;
using System.Text.RegularExpressions;
using static System.Console;

var random = new Random(); // Creo una instancia (obj) de la clase Random y la guardo en la variable random para poder usarla más adelante

//.....COMIENZA EL BLOQUE MAIN
//numero de hombres que los ejércitos tienen
int hombresDario = 1800;
int hombresAle = 1200;

//array para poder situarlos en un mapa (darles una posición)
int[] ejerDario = new int[3];
int[] ejerAle = new int[3];

//se entiende que Darío enviará varios hombes a todos los puntos, por lo tanto,
//solo se pide la segunda bandera de cada grupo 
WriteLine("Inserta el primer grupo de banderas: ");
var temp1 = Bandera();
string banderas1 = "a" + temp1;
WriteLine("Inserta el segundo grupo de banderas: ");
var temp2 = Bandera();
string banderas2 = "v" + temp2;
WriteLine("Inserta el tercer grupo de banderas: ");
var temp3 = Bandera();
string banderas3 = "r" + temp3;

WriteLine("----------------------------");
WriteLine("Banderas avistadas del ejército contrario: ");
InformeBanderas(banderas1, banderas2, banderas3);

//por cada grupo de banderas asigno x cantidad de hombres 
DistribuicionFinal(banderas1, ref hombresDario, ejerDario);
DistribuicionFinal(banderas2, ref hombresDario, ejerDario);
DistribuicionFinal(banderas3, ref hombresDario, ejerDario);
WriteLine("----------------------------");
WriteLine("Distribuición de Darío: ");
//imprimo el resultado de las funciones antteriores en formato informe
ImprimirDistribuicion(ejerDario);

//grupo 1 de alejandro
WriteLine("----------------------------");
int distribuicion1 = PreguntarAle(ref hombresAle); //dirección
WriteLine("------");
var temp4 = int.Parse(PreguntarAle2()); //nº hombres
WriteLine("------");
ejerAle[temp4] = distribuicion1;

//grupo 2
WriteLine("------");
int distribuicion2 = PreguntarAle(ref hombresAle); //dirección
WriteLine("------");
var temp5 = int.Parse(PreguntarAle2()); //nº hombres
WriteLine("------");
ejerAle[temp5] = distribuicion2;

//grupo 3
WriteLine("------");
int distribuicion3 = PreguntarAle(ref hombresAle); //dirección
WriteLine("------");
var temp6 = int.Parse(PreguntarAle2()); //nº hombres
WriteLine("------");
ejerAle[temp6] = distribuicion3;


WriteLine("----------------------------");
WriteLine("Distribuición de Alejandro: ");
ImprimirDistribuicion(ejerAle);

//una vez tengo todos los valores necesarios (cada grupo de hombres en su índice y array) puedo operar con ellos
WriteLine("----------------------------");
WriteLine("Comienza la batalla");

Batalla(ejerDario, ejerAle ); //función que calcula la victoria de las batallas

WriteLine("----------------------------");
WriteLine("Estado final del ejército de Dario:");
EstadoFinal(ejerDario);

WriteLine("----------------------------");
WriteLine("Estado final del ejército de Alejandro:");
EstadoFinal(ejerAle);

//............................................................. FIN BLOQUE MAIN


//............................................................. Funciones y procedimientos de la batalla

void EstadoFinal(int[] array)
{
    string posicion;

    for (int i = 0; i < array.Length; i++)
    {
        posicion = VariablesPosicion(i);
        WriteLine(posicion + "han quedado: " + array[i]);
    }
}

string VariablesPosicion(int num)
{
    string posicion = "";
    if (num == 0)
    {
        posicion = "En el flanco izquierdo ";
    }
    if (num == 1)
    {
        posicion = "En el flanco central ";
    }
    if (num == 2)
    {
        posicion = "En el flanco derecho ";
    }
    return posicion;
}

void Batalla(int[] array1, int[] array2)
{
    int contador = 0;
    int probVictoria;
    string posicion;

    for (var i = 0; i < array1.Length; i++)
    {
        posicion = VariablesPosicion(i);
        if (array1[i] > array2[i])
        {
            array1[i] = Convert.ToInt32(array1[i] * 0.50);
            array2[i] = Convert.ToInt32(array2[i] * 0.40);
            probVictoria = random.Next(1, 10);

            if (probVictoria >= 1 && probVictoria <= 5)
            {
                contador += 1;
                WriteLine(posicion + "La victoria es de Alejandro Magno");
            }
            else
            {
                WriteLine(posicion + "Alejandro Magno ha perdido");
            }
        }

        if (array1[i] <= array2[i])
        {
            array1[i] = Convert.ToInt32(array1[i] * 0.40);
            array2[i] = Convert.ToInt32(array2[i] * 0.70);
            probVictoria = random.Next(1, 10);

            if (probVictoria >= 1 && probVictoria <= 7)
            {
                contador += 1;
                WriteLine(posicion + "La victoria es de Alejandro Magno");
            }
            else
            {
                WriteLine(posicion + "Alejandro Magno ha perdido");
            }
        }
    }

    if (contador > 1)
    {
        WriteLine("La batalla la ha ganado Alejandro Magno");
    }
    else
    {
        WriteLine("Alejandro Magno ha perdido la batalla");
    }
}


//............................................................. Funciones y procedimientos de la organización del ejército de Alejandro

string PreguntarAle2()
{
    WriteLine("¿A qué flanco los quieres mandar?");
    string respuesta = "";  // si no inicializas respuesta a "" entonces sería null -> ( string respuesta; == null) porque string va por referencia 
    bool isValid;
    do
    {
        respuesta = Console.ReadLine();
        isValid = LeerIndice(respuesta);
        if (!isValid)
            WriteLine("Error: el flanco debe ser 0, 1 o 2");
    } while (!isValid);
    return respuesta;
}

bool LeerIndice(string mensaje)
{
    var patron = @"^[0-2]$";
    var regex = new Regex(patron);
    bool isValid = regex.IsMatch(mensaje);
    return isValid;
}

int PreguntarAle(ref int hombresAle)
{
    WriteLine("¿Cuántos hombres quieres enviar?");
    string respuesta = Console.ReadLine();
    int hombresEnviados = 400; //si no llegase a funcionar el TryParse pero si el resto al menos se podría seguir el programa
    while(!int.TryParse(respuesta, out hombresEnviados) || hombresEnviados > hombresAle)
    {
        WriteLine($"Error, puedes enviar hasta un máximo de {hombresAle}");
        respuesta = Console.ReadLine();
    }

    hombresAle = hombresAle - hombresEnviados;
    
    return hombresEnviados;
}



bool LeerNum(string mensaje)
{
    var patron = @"^([0-9]|[1-9][0-9]{1,2}|1[01][0-9]{2}|1200)$";
    var regex = new Regex(patron);
    bool isValid = regex.IsMatch(mensaje);
    return isValid;
}


//............................................................. Funciones y procedimientos de la organización del ejército de Dario

void DistribuicionFinal(string bandera, ref int ejercito, int[] array)
{
    int indice = LeerPosicion(bandera);
    int valor = AsignarHombres(bandera, ref ejercito);
    array[indice] = valor;
}

void ImprimirDistribuicion(int[] array)
{
    string posicion;
    for (int i = 0; i < array.Length; i++)
    {
        posicion = VariablesPosicion(i);
        WriteLine(posicion + "hay: " + array[i] + " hombres");
    }
}

int AsignarHombres(string bandera, ref int ejercito)
{
    string bandera2 = bandera.Substring(1, 1);

    int hombresDistribuidos = 0;
    if (bandera2 == "a")
    {
        hombresDistribuidos = ejercito / 3;
        ejercito = ejercito - hombresDistribuidos;
    }
    if (bandera2 == "v")
    {
        hombresDistribuidos = ejercito / 2;
        ejercito = ejercito - hombresDistribuidos;
    }
    if (bandera2 == "r")
    {
        hombresDistribuidos = ejercito;
        ejercito = ejercito - hombresDistribuidos;
    }

    return hombresDistribuidos;
}

int LeerPosicion(string bandera) //leo la primera bandera 
{
    string parte1 = bandera.Substring(0, 1);

    switch (parte1)
    {
        case "a":
            return 0;
        case "v":
            return 1;
        case "r":
            return 2;
        default:
            return -1;
    }
}

//............................................................. Funciones y procedimientos del avistamiento de banderas
bool VerificarStr(string input)
{
    var patron = @"^[avr]$";
    var regex = new Regex(patron);
    return regex.IsMatch(input);
}

string Bandera()
{
    string bandera = Console.ReadLine().Trim().ToLower();
    bool isValid = VerificarStr(bandera);
    while (!isValid)
    {
        WriteLine("Error, inserta la letra correcta");
        bandera = Console.ReadLine().Trim().ToLower();
        isValid = VerificarStr(bandera);
    }
    
    return bandera;
}

void InformeBanderas(string b1, string b2, string b3)
{
    WriteLine("Banderas: " + b1);
    WriteLine("Banderas: " + b2);
    WriteLine("Banderas: " + b3);
}
