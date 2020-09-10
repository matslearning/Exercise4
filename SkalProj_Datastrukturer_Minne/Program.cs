using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Schema;

namespace SkalProj_Datastrukturer_Minne
{
    // Först litet kort om kodningen. Det hade i och för sig varit intressant att göra någon form av
    // återanvändningsbart menysystem istället för den copy/paste som jag har gjort här.
    // Antagligen skulle man t.ex. kunna utgå från en Dictionary, där Key är menyvalets siffra och
    // Value kanske kan vara en Action. Om vi senare får fler uppgifter där menyval skall användas, så
    // kan det nog bli aktuellt.
    //
    // Här kommer svar på frågorna:
    //
    // Inledande tre frågor: Stacken och heapen
    //
    // 1. Stacken fungerar som en minnesplats, där det som lagras läggs på hög. Det som senast lades
    //    på stacken ligger ”överst”. Principen för en stack är Last In First Out (LIFO). Det värde som
    //    lades allra sist på stacken, är det som "släpps" tillbaka först. Heapen fungerar istället som en
    //    lagringsplats i minnet där allt går att nå ”direkt” oavsett när det lades dit. Heapen rensas
    //    då och då från ”gammalt” innehåll med hjälp av Garbage Collection.
    //
    // 2. Reference Types ligger alltid på heapen. Value types kan ligga antingen på stacken eller på heapen,
    //    beroende på var någonstans värdetypen har deklarerats.
    //
    // 3. Metoden ReturnValue() hanterar värdetyper (int) och metoden ReturnValue2() hanterar
    //    referenstyper (instanser av klassen MyInt). I den senare metoden pekar y plötsligt
    //    på samma objekt som x pekar på:
    //
    //       y = x;
    //
    //    Alla eventuella ändringar av properties i y påverkar därför x, eftersom det är samma objekt.
    //
    //
    // Övning 1: ExamineList()
    //
    // 1. Japp, den implementationen är nu färdigkodad.
    //
    // 2. Listans kapacitet, som från början är 4, ökar om nästa element inte skulle få plats.
    //    I första fallet ökar kapaciteten till 8 när man försöker lägga till element nummer 5.
    //
    // 3. Kapaciteten dubbleras och fortsätter att dubbleras varje gång arrayen fylls.
    //
    // 4. Varje utökning skapar (antagligen?) en helt ny array. Om detta skulle ske för varje nytt element,
    //    så skulle det innebära en prestandaförlust.
    //
    // 5. Nej, kapaciteten minskar inte när element tas bort ur listan.
    //
    // 6. Det är mest fördelaktigt om man har ett fast antal element och inte behöver förändra antalet.
    //    Även när man har väldigt stora mängder data, lär det vara fördelaktigt med en array.
    //
    //
    // Övning 2: ExamineQueue()
    //
    // 1. Simuleringen har jag lagt i png-filen Queue.png
    //
    // 2. Nu är den färdigimplementerad.
    //
    //
    // Övning 3: ExamineStack()
    //
    // 1. Simuleringen har jag lagt i filen Stack.png.
    //
    //    Stack är här en dålig idé eftersom sista person på plats kommer att expedieras först. :)
    //    Kalle skulle bli expedierad endast om ingen "ny" har hunnit dyka upp mellan expedieringarna.
    //
    // 2. Färdigimplementerad. Här följde jag övnings-pdf:ens instruktioner, dvs jag gjorde en
    //    metod som tar en mening och vänder på den. I själva kommentarerna för skalprojektet stod det
    //    dock att man skulle göra så som för queue-metoden, dvs en loop fast med push och pop.
    //    Det hade annars varit enkelt att göra en sådan loop - säkert 90% copy/paste från queue.
    //
    //
    // Övning 4: CheckParenthesis()
    //
    // 1. Här föredrar jag att använda Stack för att hålla ordning på parentesföljden.
    //    Själva reglerna kändes lämpligt att lagra i en Dictionary.
    //    

    class Program
    {
        static ConsoleColor normalColor = ConsoleColor.Gray;
        static ConsoleColor headerColor = ConsoleColor.Cyan;
        static ConsoleColor errorColor = ConsoleColor.Red;
        static ConsoleColor visibilityColor = ConsoleColor.Yellow;
        static string errorMessage = "";
        /// <summary>
        /// The main method, vill handle the menues for the program
        /// </summary>
        /// <param name="args"></param>
        static void Main()
        {

            while (true)
            {
                Console.Clear();
                WriteHeader("Main Menu");
                Write($"@{errorMessage}", errorColor);
                errorMessage = "";
                Console.WriteLine("Please navigate through the menu by inputting the number \n(1, 2, 3 ,4, 0) of your choice"
                    + "\n1. Examine a List"
                    + "\n2. Examine a Queue"
                    + "\n3. Examine a Stack"
                    + "\n4. CheckParanthesis"
                    + "\n0. Exit the application");
                char input = ' '; //Creates the character input to be used with the switch-case below.
                try
                {
                    input = Console.ReadLine()[0]; //Tries to set input to the first char in an input line
                }
                catch (IndexOutOfRangeException) //If the input line is empty, we ask the users for some input.
                {
                    errorMessage += "Please enter some input!\n";
                }
                switch (input)
                {
                    case '1':
                        ExamineList();
                        break;
                    case '2':
                        ExamineQueue();
                        break;
                    case '3':
                        ExamineStack();
                        break;
                    case '4':
                        CheckParanthesis();
                        break;
                    case '0':
                        Environment.Exit(0);
                        break;
                    default:
                        errorMessage += "Please enter some valid input (0, 1, 2, 3, 4)\n";
                        break;
                }
            }
        }

        /// <summary>
        /// Examines the datastructure List
        /// </summary>
        static void ExamineList()
        {
            WriteHeader("Examine a List");

            Write("Please enter a string with @+@ or @-@ as first character!"
                + "\nUse @+@ for adding to the list and @-@ for removing from the list"
                + "\n\nExample: @+abcde@ will add the string '@abcde@' to the list"
                + "\n\nEnter @q@ for main menu\n\n", visibilityColor);

            var list = new List<string>();
            bool active = true;

            while(active)
            {
                Console.Write("Command: ");
                string input = Console.ReadLine();
                if (input.Length > 0)
                {
                    char nav = input[0];
                    string value = input.Substring(1);

                    switch (nav)
                    {
                        case '+':
                            list.Add(value);
                            break;

                        case '-':
                            list.Remove(value);
                            break;

                        case 'q':
                            active = false;
                            break;

                        default:
                            Write("Please use @+@ or @-@ or @q@\n", visibilityColor);
                            break;
                    }
                    if (active)
                    {
                        Write($"Count: @{list.Count}@  Capacity: @{list.Capacity}@\n", visibilityColor);
                    }
                }
            }
        }

        /// <summary>
        /// Examines the datastructure Queue
        /// </summary>
        static void ExamineQueue()
        {
            WriteHeader("Examine a Queue");

            Write("Please enter the name of a person with @+@ as first character or enter a single @-@"
                + "\n\nUse @+@ for adding a person to the end of the queue and @-@ for removing the first person from the queue"
                + "\n\nExample: @+Berra@ will add @Berra@ to the queue"
                + "\n\n         @-@ will take away the person standing first in the queue"
                + "\n\nEnter @q@ for main menu\n\n", visibilityColor);

            var queue = new Queue<string>();
            bool active = true;

            while (active)
            {
                Console.Write("Command: ");
                string input = Console.ReadLine();
                if (input.Length > 0)
                {
                    char nav = input[0];
                    string value = input.Substring(1);

                    switch (nav)
                    {
                        case '+':
                            if(value.Length > 0)
                            {
                                queue.Enqueue(value);
                                Write($"Enqueued @{value}@ to the queue\n", visibilityColor);
                            }
                            else
                            {
                                Console.WriteLine($"String is empty and will not be added to the queue\n");
                            }
                            break;

                        case '-':
                            if(queue.Count>0)
                            {
                                var s = queue.Dequeue();
                                Write($"@{s}@ has left the queue and is now being attended to\n", visibilityColor);
                            }
                            else
                            {
                                Console.WriteLine($"Queue is empty");
                            }
                            break;

                        case 'q':
                            active = false;
                            break;

                        default:
                            Write("Please use @+@ or @-@ or @q@\n", visibilityColor);
                            break;
                    }
                    if (active)
                    {
                        Write($"Queue has {queue.Count} element{((queue.Count != 1)?"s":"")}:\n", visibilityColor);
                        foreach (string s in queue.ToArray())
                        {
                            Write($"@{s}\n", visibilityColor);
                        }
                        Console.WriteLine();
                    }
                }
            }
        }

    /// <summary>
    /// Examines the datastructure Stack
    /// </summary>
    static void ExamineStack()
        {
            WriteHeader("Examine a Stack");

            var stack = new Stack<char>();
            Write("Type a sentence, please: ");
            string input = Console.ReadLine();

            foreach(char c in input)
            {
                stack.Push(c);
            }

            var stackSize = stack.Count;

            var builder = new StringBuilder();
            for(int i=0; i<stackSize; i++)
            {
                builder.Append(stack.Pop());
            }

            Write($"@{builder}@\n\nPlease press a key to return to main menu!\n", visibilityColor);

            Console.ReadKey();
        }

        /// <summary>
        /// Checks if string has well formed parentheses
        /// </summary>
        static void CheckParanthesis()
        {
            WriteHeader("CheckParanthesis");

            var startPars = new Stack<char>(); // for keeping start parentheses

            var rules = new Dictionary<char, char>()
            {
                [')'] = '(',
                ['}'] = '{',
                [']'] = '['
            };

            Write("Please type a string with lots of () {} and []\n");
            string input = Console.ReadLine();

            bool passed = true;
            foreach (char c in input)
            {
                if (rules.Values.Contains(c)) // start parenthesis encountered
                {
                    startPars.Push(c);
                }

                if (rules.ContainsKey(c)) // end parenthesis encountered
                {
                    if(startPars.Count == 0 || startPars.Pop() != rules[c])
                    {
                        passed = false;
                        break;
                    }
                }
            }

            if (startPars.Count != 0)
            {
                passed = false;
            }

            Write("The string " + ((passed) ? "is well formed!" : "has errors!"));

            Write("\n\nPlease press a key to return to main menu!\n");
            Console.ReadKey();
        }

        /// <summary>
        /// Writes string and toggles between normal color and special color for every @ sign in the string
        /// Maybe a bit krystat but still pretty användbart liksom... :)
        /// </summary>
        /// <param name="text">Text to write</param>
        /// <param name="color">Special color</param>
        static void Write(string text, ConsoleColor specialColor)
        {
            bool isNormalColor = text[0] != '@'; // if string doesn't start with @ then normal color
            var parsed = text.Split("@", StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in parsed)
            {
                Console.ForegroundColor = (isNormalColor) ? normalColor : specialColor;
                Console.Write(s);
                isNormalColor = !isNormalColor; // toggle color
            }
            Console.ForegroundColor = normalColor;
        }

        /// <summary>
        /// Monochrome Write to complement the color version
        /// </summary>
        /// <param name="text">Text to write</param>
        static void Write(string text)
        {
            Console.ForegroundColor = normalColor;
            Console.Write(text);
        }

        /// <summary>
        /// Clear screen and display menu header text
        /// </summary>
        /// <param name="text">Menu header text</param>
        static void WriteHeader(string text)
        {
            Console.Clear();
            Write($"@****** {text} ******\n\n", headerColor);
        }
    }
}

