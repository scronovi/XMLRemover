using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Linq;

class Program
{
    static void Main()
    {

        Console.Write("Sökväg till XML-Mapp: ");
        string folderPath = Console.ReadLine();


        if (Directory.Exists(folderPath))
        {
            var xmlFiles = Directory.GetFiles(folderPath, "*.xml");

            foreach (var file in xmlFiles)
            {
                try
                {
                    XDocument doc = XDocument.Load(file);

                    var elementsInDoc = doc.Descendants("NonUsageTransactions")
                        .ToList();
                    
                    //.Where(e => e.Value.Trim() == "Row")

                    if (elementsInDoc != null)
                    {
                        foreach (var element in elementsInDoc)
                        {
                            if (element.Descendants("InvoiceText") != null) {
                                var elementDesc = element.Descendants("InvoiceText");
                                foreach (var desc in elementDesc)
                                {
                                    if (desc.Value.Contains("nadsavgift"))
                                    {
                                        if (desc.Parent.Parent.Name == "NonUsageTransactions") 
                                        {
                                            desc.Parent.Parent.Remove();
                                            Console.WriteLine($"Tar bort: {desc.Parent.Parent}");
                                        }
                                    }
                                }

                            }
                        }
                    }
                    doc.Save(file);
                    Console.WriteLine($"Klar: {file}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error med {file}: {ex.Message}");
                }
            }
        }
        else
        {
            Console.WriteLine("Hittade ingen mapp.");
        }
    }

}
