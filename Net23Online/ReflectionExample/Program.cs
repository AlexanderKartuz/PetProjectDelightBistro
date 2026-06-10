using ReflectionExample;
using System.Reflection;

var olga = new Girl(16);
var lera = new Girl(30);

Console.WriteLine("Olga is adult: " + olga.IsAdult());
Console.WriteLine("Lera is adult: " + lera.IsAdult());

var type = olga.GetType();
var typeV2 = typeof(Girl);

Console.WriteLine("Type name: " + type.Name);
Console.WriteLine("Type FullName: " + type.FullName);

var methods = type.GetMethods(BindingFlags.NonPublic
    | BindingFlags.Public
    | BindingFlags.Instance);

foreach (var method in methods)
{
    Console.WriteLine($"{(method.IsPublic ? "public" : "private")} {method.ReturnType} {method.Name} ({method.GetParameters().Count()})");
}

var fields = type.GetFields(BindingFlags.NonPublic
    | BindingFlags.Public
    | BindingFlags.Instance);

foreach (var field in fields)
{
    Console.WriteLine($"{(field.IsPublic ? "public" : "private")} {field.FieldType} {field.Name}");
}

var fieldAge = type.GetField("_age", BindingFlags.NonPublic | BindingFlags.Instance);

var sercretAgeBefore = fieldAge.GetValue(lera);
Console.WriteLine("_age: " + sercretAgeBefore);

fieldAge.SetValue(lera, 50);

var sercretAgeAfter = fieldAge.GetValue(lera);
Console.WriteLine("_age: " + sercretAgeAfter);
