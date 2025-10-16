using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        Square firstSquare = new Square("Red", 5);
        Rectangle firstRectangle = new Rectangle("Blue", 4, 6);
        Circle firstCircle = new Circle("Green", 3);

        Console.WriteLine("Testing individual shapes:");
        Console.WriteLine($"Square color: {firstSquare.GetColor()}, area: {firstSquare.GetArea():0.##}");
        Console.WriteLine($"Rectangle color: {firstRectangle.GetColor()}, area: {firstRectangle.GetArea():0.##}");
        Console.WriteLine($"Circle color: {firstCircle.GetColor()}, area: {firstCircle.GetArea():0.##}");

        List<Shape> shapes = new List<Shape>
        {
            firstSquare,
            firstRectangle,
            firstCircle,
            new Square("Yellow", 2.5)
        };

        Console.WriteLine("\nIterating through all shapes:");
        foreach (Shape shape in shapes)
        {
            Console.WriteLine($"Color: {shape.GetColor()}, area: {shape.GetArea():0.##}");
        }
    }
}
