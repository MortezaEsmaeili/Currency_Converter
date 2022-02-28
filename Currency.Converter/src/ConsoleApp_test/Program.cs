// See https://aka.ms/new-console-template for more information
using QuickGraph;
using QuickGraph.Algorithms;

Console.WriteLine("Hello, World!");

var cities = new AdjacencyGraph<string, Edge<string>>(); // a graph of cities
cities.AddVerticesAndEdge(new Edge<string>("A", "B"));
cities.AddVerticesAndEdge(new Edge<string>("A", "D"));
cities.AddVerticesAndEdge(new Edge<string>("B", "C"));
//cities.AddVerticesAndEdge(new Edge<string>("B", "A"));
cities.AddVerticesAndEdge(new Edge<string>("D", "C"));

//Func<Edge<string>, double> cityDistances = e => {}; //e.Target + e.Source; // a delegate that gives the distance between cities

string sourceCity = "A"; // starting city
string targetCity = "C"; // ending city

// vis can create all the shortest path in the graph
// and returns a delegate vertex -> path
var tryGetPath = cities.ShortestPathsDijkstra(esi, sourceCity);

IEnumerable<Edge<string>> path;
if (tryGetPath(targetCity, out path))
    foreach (var e in path)
    {
        Console.WriteLine(e);
    }

Console.ReadLine();

double esi(Edge<string> e)
{
    if (e.Target == "C" && e.Source == "B")
        return 2;
    return 1;
}